/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphaleonis.Win32.Vss;
using System.EnterpriseServices;
using Alphaleonis.Win32.Filesystem;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace AlphaShadow
{

   public class VssClient : IDisposable
   {
      #region Private Fields

      private List<VssWriterDescriptor> m_writerComponentsForRestore;
      private static IVssImplementation s_implementation;
      private VssVolumeSnapshotAttributes m_context = (VssVolumeSnapshotAttributes)VssSnapshotContext.Backup;
      private IVssBackupComponents m_backupComponents;
      private bool m_duringRestore;
      private List<VssWriterDescriptor> m_writers;
      private Guid m_latestSnapshotSetId;
      private IList<string> m_latestVolumeList;
      private List<Guid> m_latestSnapshotIdList = new List<Guid>();

      #endregion

      #region Constructor

      public VssClient(IUIHost host)
      {
         if (host == null)
            throw new ArgumentNullException("host", "host is null.");

         Host = host;
      }

      #endregion

      #region Properties

      public IUIHost Host { get; private set; }

      public static IVssImplementation Implementation
      {
         get
         {
            if (s_implementation == null)
            {
               s_implementation = VssUtils.LoadImplementation();
            }
            return s_implementation;
         }
      }

      #endregion

      #region Public Methods

      public void Initialize(VssVolumeSnapshotAttributes context, string xmlDoc = null, bool duringRestore = false)
      {
         Initialize((VssSnapshotContext)context, xmlDoc, duringRestore);
      }

      public void Initialize(VssSnapshotContext context, string xmlDoc = null, bool duringRestore = false)
      {
         m_backupComponents = Implementation.CreateVssBackupComponents();
         m_duringRestore = duringRestore;

         if (m_duringRestore)
         {
            Host.WriteVerbose("- Calling IVssBackupComponents.InitializeForRestore() {0} xml doc.", xmlDoc == null ? "without" : "with");
            m_backupComponents.InitializeForRestore(xmlDoc);
         }
         else
         {
            Host.WriteVerbose("- Calling IVssBackupComponents.InitializeForBackup() {0} xml doc.", xmlDoc == null ? "without" : "with");
            m_backupComponents.InitializeForBackup(xmlDoc);

            if (OperatingSystemInfo.IsAtLeast(OSVersionName.WindowsServer2003) && context != VssSnapshotContext.Backup)
            {
               Host.WriteLine("- Setting the VSS context to: {0}", context);
               m_backupComponents.SetContext(context);
            }
         }

         m_context = (VssVolumeSnapshotAttributes)context;

         m_backupComponents.SetBackupState(true, true, VssBackupType.Full, false);
      }

      public void QuerySnapshotSet(Guid snapshotSetId)
      {
         if (snapshotSetId == Guid.Empty)
            Host.WriteLine("- Querying all shadow copies in the system...");
         else
            Host.WriteLine("- Querying all shadow copies with the SnapshotSetID {0:B}...", snapshotSetId);

         Host.WriteVerbose("- Calling IVssBackupComponents.QuerySnapshots()");
         IEnumerable<VssSnapshotProperties> snapshots = m_backupComponents.QuerySnapshots();

         if (!snapshots.Any())
            Host.WriteHeader("There are no snapshots in the system.");

         foreach (VssSnapshotProperties props in snapshots)
         {
            if (snapshotSetId == Guid.Empty || props.SnapshotSetId == snapshotSetId)
            {
               PrintSnapshotProperties(props);
            }
         }
      }

      public void GetSnapshotProperties(Guid snapshotId)
      {
         Host.WriteLine("- Getting properties for snapshot with ID {0:B}", snapshotId);
         Host.WriteVerbose("- Calling IVssBackupComponents.GetSnapshotProperties({0:B})", snapshotId);
         VssSnapshotProperties props = m_backupComponents.GetSnapshotProperties(snapshotId);
         PrintSnapshotProperties(props);
      }

      public void CreateSnapshot(IEnumerable<string> volumeList, string outputXmlFile, IEnumerable<string> excludedWriterList, IEnumerable<string> includedWriterList)
      {
         bool snapshotWithWriters = (m_context & VssVolumeSnapshotAttributes.NoWriters) == 0;

         if (snapshotWithWriters)
         {
            // Gather writer metadata
            GatherWriterMetadata();

            // Select writer components based on the given shadow volume list
            SelectComponentsForBackup(volumeList, excludedWriterList, includedWriterList);
         }

         // Start the shadow set
         m_latestSnapshotSetId = m_backupComponents.StartSnapshotSet();
         Host.WriteLine("Creating shadow set {0:B}...", m_latestSnapshotSetId);

         // Add the specified volumes to the shadow set
         AddToSnapshotSet(volumeList);

         // Prepare for backup. 
         // This will internally create the backup components document with the selected components
         if (snapshotWithWriters)
            PrepareForBackup();

         // Create the shadow set 
         DoSnapshotSet();

         // Do not attempt to continue with delayed snapshot ...
         if ((m_context & VssVolumeSnapshotAttributes.DelayedPostSnapshot) != 0)
         {
            Host.WriteLine("Fast snapshot created. Exiting...");
            return;
         }

         // Saves the backup components document, if needed
         if (!String.IsNullOrEmpty(outputXmlFile))
            SaveBackupComponentsDocument(outputXmlFile);

         // List all the created shadow copies
         if ((m_context & VssVolumeSnapshotAttributes.Transportable) == 0)
         {
            Host.WriteLine("List of created shadow copies:");
            QuerySnapshotSet(m_latestSnapshotSetId);
         }
      }

      private void SaveBackupComponentsDocument(string outputXmlFile)
      {
         Host.WriteLine("Saving the backup components document ... ");
         
         File.WriteAllText(outputXmlFile, m_backupComponents.SaveAsXml(), Encoding.Unicode);
      }

      private void DoSnapshotSet()
      {
         Host.WriteLine("Creating the shadow (DoSnapshotSet)...");

         m_backupComponents.DoSnapshotSet();

         if ((m_context & VssVolumeSnapshotAttributes.DelayedPostSnapshot) != 0)
         {
            Host.WriteLine("Fast DoSnapshotSet finished.");
            return;
         }

         CheckSelectedWriterStatus();

         Host.WriteLine("Shadow copy set successfully created.");
      }

      private void PrepareForBackup()
      {
         Host.WriteLine("Preparing for backup ... ");

         m_backupComponents.PrepareForBackup();

         // Check selected writer status
         CheckSelectedWriterStatus();
      }

      internal void CheckSelectedWriterStatus()
      {
         if ((m_context & VssVolumeSnapshotAttributes.NoWriters) != 0)
            return;


         // Gather writer status to detect potential errors
         GatherWriterStatus();

         // Gets the number of writers in the gathered status info
         // (WARNING: GatherWriterStatus must be called before)
         foreach (VssWriterStatusInfo writer in m_backupComponents.WriterStatus)
         {
            if (!IsWriterSelected(writer.InstanceId))
               continue;

            switch (writer.State)
            {
               case VssWriterState.FailedAtIdentify:
               case VssWriterState.FailedAtPrepareBackup:
               case VssWriterState.FailedAtPrepareSnapshot:
               case VssWriterState.FailedAtFreeze:
               case VssWriterState.FailedAtThaw:
               case VssWriterState.FailedAtPostSnapshot:
               case VssWriterState.FailedAtBackupComplete:
               case VssWriterState.FailedAtPreRestore:
               case VssWriterState.FailedAtPostRestore:
               case VssWriterState.FailedAtBackupShutdown:
                  break;
               default:
                  continue;
            }

            // Print writer status
            Host.WriteError("Selected writer '{0}' is in failed state!", writer.Name);
            Host.WriteTable(new StringTable(
               new[] 
               {
                  "Status",
                  "Writer Failure Code",
                  "Writer ID",
                  "Instance ID",
               },
               new object[]
               {
                  writer.State,
                  writer.Failure,
                  writer.ClassId,
                  writer.InstanceId
               }));
            throw new CommandAbortedException();
         }
      }

      private bool IsWriterSelected(Guid guid)
      {
         return m_writers.Any(writer => writer.WriterMetadata.InstanceId == guid && !writer.IsExcluded);
      }

      private void AddToSnapshotSet(IEnumerable<string> volumeList)
      {
         // Preserve the list of volumes for script generation 
         m_latestVolumeList = new List<string>(volumeList);

         // Add volumes to the shadow set 
         foreach (string volume in volumeList)
         {
            Host.WriteLine("- Adding volume {0} [{1}] to the shadow set...", volume, Volume.GetDisplayNameForVolume(volume));
            m_latestSnapshotIdList.Add(m_backupComponents.AddToSnapshotSet(volume));
         }
      }

      private void SelectComponentsForBackup(IEnumerable<string> shadowSourceVolumes, IEnumerable<string> excludedWriterList, IEnumerable<string> includedWriterList)
      {
         // First, exclude all components that have data outside of the shadow set
         DiscoverDirectlyExcludedComponents(excludedWriterList, m_writers);

         // Then discover excluded components that have file groups outside the shadow set
         DiscoverNonShadowedExcludedComponents(shadowSourceVolumes);

         // Now, exclude all componenets that are have directly excluded descendents
         DiscoverAllExcludedComponents();

         // Next, exclude all writers that:
         // - either have a top-level nonselectable excluded component
         // - or do not have any included components (all its components are excluded)
         DiscoverExcludedWriters();

         // Now, discover the components that should be included (explicitly or implicitly)
         // These are the top components that do not have any excluded children
         DiscoverExplicitelyIncludedComponents();

         Host.WriteLine("Verifying explicitly specified writers/components...");

         foreach (string include in includedWriterList)
         {
            if (include.Contains(':'))
               VerifyExplicitlyIncludedComponent(include, m_writers);
            else
               VerifyExplicitlyIncludedWriter(include, m_writers);
         }

         // Finally, select the explicitly included components
         SelectExplicitelyIncludedComponents();
      }

      private void SelectExplicitelyIncludedComponents()
      {
         Host.WriteLine("Select explicitly included components ...");

         foreach (VssWriterDescriptor writer in m_writers.Where(w => !w.IsExcluded))
         {
            Host.WriteLine(" * Writer '{0}':", writer.WriterMetadata.WriterName);

            // Compute the roots of included components
            foreach (var component in writer.ComponentDescriptors.Where(c => c.IsExplicitlyIncluded))
            {
               Host.WriteLine("    - Add component {0}", component.FullPath);
               m_backupComponents.AddComponent(writer.WriterMetadata.InstanceId, writer.WriterMetadata.WriterId, component.ComponentType, component.LogicalPath, component.ComponentName);
            }
         }
      }

      private void VerifyExplicitlyIncludedComponent(string include, List<VssWriterDescriptor> writerList)
      {
         Host.WriteLine("- Verifying component \"{0}\"...", include);

         foreach (VssWriterDescriptor writer in writerList.Where(w => !w.IsExcluded))
         {
            // Find the associated component
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors.Where(c => !c.IsExcluded))
            {
               if (include.Equals(writer.WriterMetadata.WriterName + ":" + component.FullPath) ||
                   include.Equals(writer.WriterMetadata.WriterId.ToString("B") + ":" + component.FullPath, StringComparison.OrdinalIgnoreCase) ||
                   include.Equals(writer.WriterMetadata.InstanceId.ToString("B") + ":" + component.FullPath, StringComparison.OrdinalIgnoreCase))
               {
                  Host.WriteVerbose("- Found component '{0}' from writer '{1}.", component.FullPath, writer.WriterMetadata.WriterName);

                  // If we are during restore, we just found our component
                  if (m_duringRestore)
                  {
                     Host.WriteLine(" - The component \"{0}\" is selected.", include);
                     return;
                  }

                  // If not explicitly included, check to see if there is an explicitly included ancestor
                  bool isIncluded = component.IsExplicitlyIncluded;
                  if (!isIncluded)
                  {
                     isIncluded = writer.ComponentDescriptors.Any(ancestor => ancestor.IsAncestorOf(component) && ancestor.IsExplicitlyIncluded);
                  }

                  if (isIncluded)
                  {
                     Host.WriteLine(" - The component \"{0}\" is selected.", include);
                     return;
                  }
                  else
                  {
                     Host.WriteError("The component \"{0}\" was not included in the backup! Aborting backup...", include);
                     Host.WriteError("- Please reveiw the component/subcomponent definitions");
                     Host.WriteError("- Also, please verify list of volumes to be shadow copied.");
                     throw new CommandAbortedException();
                  }
               }
            }
         }

         Host.WriteError("The component \"{0}\" was not found in the writer components list! Aborting backup ...", include);
         Host.WriteError("- Please check the syntax of the component name.");
         throw new CommandAbortedException();
      }

      private void VerifyExplicitlyIncludedWriter(string writerName, List<VssWriterDescriptor> writerList)
      {
         Host.WriteLine("- Verifing that all components of writer \"{0}\" are included in backup...", writerName);

         foreach (VssWriterDescriptor writer in writerList.Where(w => !w.IsExcluded))
         {
            if (writerName.Equals(writer.WriterMetadata.WriterName) ||
               writerName.Equals(writer.WriterMetadata.WriterId.ToString("B"), StringComparison.OrdinalIgnoreCase) ||
               writerName.Equals(writer.WriterMetadata.InstanceId.ToString("B"), StringComparison.OrdinalIgnoreCase))
            {
               if (writer.IsExcluded)
               {
                  Host.WriteError("The writer \"{0}\" was not included in the backup! Aborting backup ...", writer.WriterMetadata.WriterName);
                  Host.WriteError("- Please reveiw the component/subcomponent definitions");
                  Host.WriteError("- Also, please verify list of volumes to be shadow copied.");
                  throw new CommandAbortedException();
               }

               if (writer.ComponentDescriptors.Any(component => component.IsExcluded))
               {
                  Host.WriteError("ERROR: The writer \"{0}\" has components not included in the backup! Aborting backup ...", writer.WriterMetadata.WriterName);
                  Host.WriteError("- Please reveiw the component/subcomponent definitions");
                  Host.WriteError("- Also, please verify list of volumes to be shadow copied.");
                  throw new CommandAbortedException();

               }

               Host.WriteLine(" - All components from writer \"{0}\" are selected.", writerName);
               return;
            }
         }

         Host.WriteError("ERROR: The writer \"{0}\" was not found! Aborting backup ...", writerName);
         Host.WriteError("- Please check the syntax of the writer name/id.");
         throw new CommandAbortedException();
      }

      private void DiscoverExplicitelyIncludedComponents()
      {
         Host.WriteLine("Discover explicitly included components...");

         // Enumerate all writers
         foreach (VssWriterDescriptor writer in m_writers.Where(w => w.IsExcluded == false))
         {
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors.Where(c => c.CanBeExplicitlyIncluded))
            {
               // Test if our component has a parent that is also included
               // If so this cannot be explicitely included since we have another ancestor that that must be (implictely or explicitely) included
               component.IsExplicitlyIncluded = !writer.ComponentDescriptors.Any(ancestor => ancestor.IsAncestorOf(component) && ancestor.CanBeExplicitlyIncluded);
            }
         }
      }

      private void DiscoverExcludedWriters()
      {
         Host.WriteLine("Discover excluded writers...");

         // Enumerate writers
         foreach (VssWriterDescriptor writer in m_writers.Where(w => w.IsExcluded == false))
         {
            // Discover if we have any:
            // - non-excluded selectable components 
            // - or non-excluded top-level non-selectable components
            // If we have none, then the whole writer must be excluded from the backup
            writer.IsExcluded = !writer.ComponentDescriptors.Any(comp => comp.CanBeExplicitlyIncluded);

            // No included components were found
            if (writer.IsExcluded)
            {
               Host.WriteLine("- The writer '{0} is now entierly excluded from the backup (it does not contain any components that can be potentially included in the backup).", writer.WriterMetadata.WriterName);
               continue;
            }

            // Now, discover if we have any top-level excluded non-selectable component 
            // If this is true, then the whole writer must be excluded from the backup
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors)
            {
               if (component.IsTopLevel && !component.IsSelectable && component.IsExcluded)
               {
                  Host.WriteLine("- The writer '{0}' is now entierly excluded from the backup (the top-level non-selectable component '{1}' is an excluded component.",
                     writer.WriterMetadata.WriterName, component.FullPath);
                  writer.IsExcluded = true;
                  break;
               }
            }
         }
      }

      private void DiscoverAllExcludedComponents()
      {
         Host.WriteLine("Discover all excluded components ...");

         // Discover components that should be excluded from the shadow set 
         // This means components that have at least one File Descriptor requiring 
         // volumes not in the shadow set. 
         foreach (VssWriterDescriptor writer in m_writers.Where(w => w.IsExcluded == false))
         {
            // Enumerate all components
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors)
            {
               // Check if this component has any excluded children
               // If yes, deselect it
               foreach (VssComponentDescriptor descendent in writer.ComponentDescriptors)
               {
                  if (component.IsAncestorOf(descendent) && descendent.IsExcluded)
                  {
                     Host.WriteLine("- Component '{0}' from writer '{1} is excluded from backup (it has an excluded descendent: '{2}').", component.FullPath, writer.WriterMetadata.WriterName, descendent.WriterName);
                     component.IsExcluded = true;
                     break;
                  }
               }
            }
         }
      }

      private void DiscoverNonShadowedExcludedComponents(IEnumerable<string> shadowSourceVolumes)
      {
         Host.WriteLine("Discover components that reside outside the shadow set...");

         // Discover components that should be excluded from the shadow set 
         // This means components that have at least one File Descriptor requiring 
         // volumes not in the shadow set. 
         foreach (VssWriterDescriptor writer in m_writers)
         {
            if (writer.IsExcluded)
               continue;

            // Check if the component is excluded
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors)
            {
               // Check to see if this component is explicitly excluded
               if (component.IsExcluded)
                  continue;

               // Try to find an affected volume outside the shadow set
               // If yes, exclude the component
               for (int i = 0; i < component.AffectedVolumes.Count; i++)
               {
                  if (Volume.ClusterIsPathOnSharedVolume(Host, component.AffectedVolumes[i]))
                  {
                     component.AffectedVolumes[i] = Volume.ClusterGetVolumeNameForVolumeMountPoint(Host, component.AffectedVolumes[i]);
                  }

                  if (!shadowSourceVolumes.Contains(component.AffectedVolumes[i]))
                  {
                     string localVolume;
                     try
                     {
                        localVolume = Volume.GetDisplayNameForVolume(component.AffectedVolumes[i]);
                     }
                     catch
                     {
                        localVolume = null;
                     }

                     if (localVolume != null)
                     {
                        Host.WriteLine("- Component '{0}' from writer '{1}' is excluded from backup (it requires {2} in the shadow set)",
                           component.FullPath, writer.WriterMetadata.WriterName, localVolume);
                     }
                     else
                     {
                        Host.WriteLine("- Component '{0}' from writer '{1}' is excluded from backup.", component.FullPath, writer.WriterMetadata.WriterName);
                     }
                     component.IsExcluded = true;
                     break;
                  }
               }
            }
         }




      }

      private void DiscoverDirectlyExcludedComponents(IEnumerable<string> excludedWriterAndComponentList, List<VssWriterDescriptor> writerList)
      {
         Host.WriteLine("Discover directly excluded components...");

         // Discover components that should be excluded from the shadow set 
         // This means components that have at least one File Descriptor requiring 
         // volumes not in the shadow set. 
         foreach (var writer in writerList)
         {
            // Check if the writer is excluded
            if (excludedWriterAndComponentList.Contains(writer.WriterMetadata.WriterName) ||
                excludedWriterAndComponentList.Contains(writer.WriterMetadata.WriterId.ToString("B"), StringComparer.OrdinalIgnoreCase) ||
                excludedWriterAndComponentList.Contains(writer.WriterMetadata.InstanceId.ToString("B"), StringComparer.OrdinalIgnoreCase))
            {
               writer.IsExcluded = true;
               continue;
            }

            // Check if the component is excluded
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors)
            {
               // Check to see if this component is explicitly excluded
               if (excludedWriterAndComponentList.Contains(writer.WriterMetadata.WriterName + ":" + component.FullPath) ||
                   excludedWriterAndComponentList.Contains(writer.WriterMetadata.WriterId + ":" + component.FullPath) ||
                  excludedWriterAndComponentList.Contains(writer.WriterMetadata.InstanceId + ":" + component.FullPath))
               {
                  Host.WriteLine("- Component '{0}' from writer '{1}' is explicitly excluded from backup.", component.FullPath, writer.WriterMetadata.WriterName);
                  component.IsExcluded = true;
                  continue;
               }
            }

            // If there are no included components, exclude the writer.
            if (!writer.ComponentDescriptors.Any(comp => comp.IsExcluded == false))
            {
               Host.WriteLine("- Excluding writer '{0}' since it has no selected components for restore.", writer.WriterMetadata.WriterName);
               writer.IsExcluded = true;
            }
         }
      }

      public void GatherWriterMetadata()
      {
         Host.WriteLine("Gathering writer metadata...");

         // Gathers writer metadata
         // WARNING: this call can be performed only once per IVssBackupComponents instance!

         using (IVssAsyncResult result = m_backupComponents.BeginGatherWriterMetadata(null, null))
         {
            Host.WriteLine("Waiting for asynchronous operation to complete...");
            result.AsyncWaitHandle.WaitOne();
         }

         Host.WriteLine("- Initialize writer metadata...");
         InitializeWriterMetadata();
      }

      internal void GatherWriterMetadataToScreen()
      {
         Host.WriteLine("Gathering writer metadata...");

         using (IVssAsyncResult result = m_backupComponents.BeginGatherWriterMetadata(null, null))
         {
            Host.WriteLine("Waiting for asynchronous operation to complete...");
            result.AsyncWaitHandle.WaitOne();
         }

         int c = 1;
         foreach (IVssExamineWriterMetadata writerMetadata in m_backupComponents.WriterMetadata)
         {
            string xml = writerMetadata.SaveAsXml();
            Host.WriteHeader("[Writer {0}]", c++);
            Host.WriteLine("{0}", xml);
         }

         Host.WriteHeader("[End of data]");
      }

      public void GatherWriterStatus()
      {
         Host.WriteLine("Gather writer status...");
         m_backupComponents.GatherWriterStatus();
      }

      public void ListWriterStatus()
      {
         Host.WriteLine("Listing writer status...");

         using (var i1 = Host.GetIndent())
         {
            IList<VssWriterStatusInfo> statusList = m_backupComponents.WriterStatus;
            Host.WriteLine("Number of writers that responded: {0}", statusList.Count);

            using (var i2 = Host.GetIndent())
            {
               foreach (VssWriterStatusInfo info in statusList)
               {
                  Host.WriteLine();
                  Host.WriteHeader("Writer \"{0}\"", info.Name);

                  StringTable table = new StringTable();
                  table.Add("Status:", info.State);
                  table.Add("Writer failure code:", info.Failure);
                  table.Add("Writer ID:", info.ClassId.ToString("B"));
                  table.Add("Instance ID:", info.InstanceId.ToString("B"));

                  Host.WriteTable(table, 3);
               }
            }
         }
      }

      private void InitializeWriterMetadata()
      {
         m_writers = new List<VssWriterDescriptor>(m_backupComponents.WriterMetadata.Select(wm => new VssWriterDescriptor(Host, wm)));         
      }
      #endregion

      #region Private Methods

      private void PrintSnapshotProperties(VssSnapshotProperties props)
      {
         StringTable table = new StringTable();

         Host.WriteLine();
         Host.WriteHeader("SNAPSHOT ID = {0:B}", props.SnapshotId);

         table.Add("Shadow Copy Set ID:", props.SnapshotSetId.ToString("B"));
         table.Add("Original Shadow Copy Count:", props.SnapshotsCount);
         table.Add("Original Volume Name:", props.OriginalVolumeName);
         table.Add("Creation Time:", props.CreationTimestamp);
         table.Add("Device Name:", props.SnapshotDeviceObject);
         table.Add("Originating Machine:", props.OriginatingMachine);
         table.Add("Service Machine:", props.ServiceMachine);

         if ((props.SnapshotAttributes & VssVolumeSnapshotAttributes.ExposedLocally) != 0)
            table.Add("Exposed Locally as:", props.ExposedName);
         else if ((props.SnapshotAttributes & VssVolumeSnapshotAttributes.ExposedRemotely) != 0)
         {
            table.Add("Exposed Remotely as:", props.ExposedName);
            if (!String.IsNullOrEmpty(props.ExposedPath))
               table.Add("Path Exposed:", props.ExposedPath);
         }
         else
         {
            table.Add("Is Exposed:", "No");
         }

         table.Add("Provider ID:", props.ProviderId.ToString("B"));

         StringBuilder attributes = new StringBuilder();
         foreach (VssVolumeSnapshotAttributes value in Enum.GetValues(typeof(VssVolumeSnapshotAttributes)))
         {
            if (value != VssVolumeSnapshotAttributes.ExposedLocally && value != VssVolumeSnapshotAttributes.ExposedRemotely
               && (props.SnapshotAttributes & value) == value)
            {
               attributes.AppendFormat("{0} ", value);
            }
         }
         table.Add("Snapshot Attributes:", attributes.ToString());

         Host.WriteTable(table, 2);

      }

      #endregion


      #region IDisposable Members

      public void Dispose()
      {
         if (m_backupComponents != null)
         {
            m_backupComponents.Dispose();
            m_backupComponents = null;
         }
      }

      #endregion

      internal void ListWriterMetadata(bool detailed)
      {
         Host.WriteLine("Listing writer metadata...");

         foreach (VssWriterDescriptor writer in m_writers)
         {
            Host.WriteLine();

            // Print writer identity information
            Host.WriteHeader("Writer \"{0}\"", writer.WriterMetadata.WriterName);
            Host.PushIndent();
            StringTable table = new StringTable();
            table.Add("Writer ID:", writer.WriterMetadata.WriterId.ToString("B"));
            table.Add("Instance ID:", writer.WriterMetadata.InstanceId.ToString("B"));
            if (writer.WriterMetadata.RestoreMethod == null)
            {
               table.Add("Restore Method:", "No restore method specified by this writer.");
            }
            else
            {
               table.Add("Supports restore events:", (writer.WriterMetadata.RestoreMethod.WriterRestore != VssWriterRestore.Never).ToString());
               table.Add("Writer restore conditions:", writer.WriterMetadata.RestoreMethod.Method);
               table.Add("Requires reboot after restore:", writer.WriterMetadata.RestoreMethod.RebootRequired);
            }
            if (!writer.WriterMetadata.ExcludeFiles.Any())
               table.Add("Excluded files:", "<none>");
            else
               table.Add("Excluded files:", "");

            Host.WriteTable(table);

            foreach (VssWMFileDescriptor file in writer.WriterMetadata.ExcludeFiles)
            {
               PrintFileDescription(file);
               Host.WriteLine();
            }

            foreach (VssComponentDescriptor component in writer.ComponentDescriptors)
            {
               PrintComponent(component, detailed);
               Host.WriteLine();
            }

            Host.PopIndent();
         }
      }

      private void PrintComponent(VssComponentDescriptor component, bool detailed)
      {
         Host.WriteHeader("Component \"{0}:{1}\"", component.WriterName, component.FullPath);
         Host.PushIndent();
         StringTable table = new StringTable();
         table.Add("Name:", component.ComponentName);
         table.Add("Logical Path:", component.LogicalPath);
         table.Add("Full Path:", component.FullPath);
         table.Add("Caption: ", component.Caption);
         table.Add("Type:", component.ComponentType);
         table.Add("Is Selectable:", component.IsSelectable);
         table.Add("Is Top Level:", component.IsTopLevel);
         table.Add("Notify on backup complete:", component.NotifyOnBackupComplete);
         Host.WriteTable(table);

         if (detailed)
         {
            Host.WriteLine("Components:");
            Host.PushIndent();
            foreach (VssWMFileDescriptor file in component.Files)
            {
               PrintFileDescription(file, "File");
               Host.WriteLine();
            }

            foreach (VssWMFileDescriptor file in component.DatabaseFiles)
            {
               PrintFileDescription(file, "Database");
               Host.WriteLine();
            }

            foreach (VssWMFileDescriptor file in component.DatabaseLogFiles)
            {
               PrintFileDescription(file, "Database Log File");
               Host.WriteLine();
            }

            Host.PopIndent();
         }

         Host.WriteLine("Paths affected by this component:");
         Host.PushIndent();
         foreach (string path in component.AffectedPaths)
            Host.WriteLine("{0}", path);
         Host.PopIndent();

         Host.WriteLine("Volumes affected by this component:");
         Host.PushIndent();
         foreach (string volume in component.AffectedVolumes)
         {
            string displayName;
            try
            {
               displayName = Volume.GetDisplayNameForVolume(volume);
            }
            catch
            {
               displayName = "Not valid for local machine";
            }
            Host.WriteLine("{0} [{1}]", volume, displayName);
         }
         Host.PopIndent();

         if (component.Dependencies.Any())
         {
            Host.WriteLine("Component Dependencies:");
            Host.PushIndent();
            foreach (var dependency in component.Dependencies)
               PrintDependency(dependency);
            Host.PopIndent();
         }

         Host.PopIndent();

      }

      private void PrintDependency(VssWMDependency dependency)
      {
         Host.WriteLine("Dependency to \"{0}:{1}\"", dependency.ComponentName, dependency.LogicalPath);
      }

      private void PrintFileDescription(VssWMFileDescriptor file, string type = null)
      {
         string alternateDisplayPath = "";
         if (!String.IsNullOrEmpty(file.AlternateLocation))
            alternateDisplayPath = String.Format(", Alternate Location={0}" + file.AlternateLocation);

         Host.PushIndent();
         StringTable table = new StringTable();
         if (type != null)
            table.Add("Type:", type);
         table.Add("Path:", file.Path);
         table.Add("Filespec:", file.FileSpecification);
         table.Add("Recursive:", file.IsRecursive);
         if (!String.IsNullOrEmpty(file.AlternateLocation))
            table.Add("Alternate Location:", file.AlternateLocation);

         Host.WriteTable(table);
         Host.PopIndent();
      }

      internal void GenerateSetvarScript(string fileName)
      {
         Host.WriteLine("Generating the SETVAR script ({0})... ", fileName);

         using (StreamWriter writer = new StreamWriter(fileName))
         {
            writer.WriteLine("@echo.");
            writer.WriteLine("@echo [This script is generated by AlphaShadow.exe for the shadow set {0:B}]", m_latestSnapshotSetId);
            writer.WriteLine("@echo.");
            writer.WriteLine();

            writer.WriteLine("SET SHADOW_SET_ID={0:B}", m_latestSnapshotSetId);

            // For each added volume add the AlphaShadow.exe exposure command
            for (int i = 0; i < m_latestSnapshotIdList.Count; i++)
            {
               Guid snapshotId = m_latestSnapshotIdList[i];
               writer.WriteLine("SET SHADOW_ID_{0}={1:B}", i, snapshotId);
               if ((m_context & VssVolumeSnapshotAttributes.Transportable) == 0)
               {
                  VssSnapshotProperties properties = m_backupComponents.GetSnapshotProperties(snapshotId);
                  writer.WriteLine("SET SHADOW_DEVICE_{0}={1}", i, properties.SnapshotDeviceObject);
               }
            }
         }
      }

      internal void BackupComplete(bool succeeded)
      {
         if (m_backupComponents.WriterComponents.Count == 0)
         {
            Host.WriteLine("- There were no writer components in this backup");
            return;
         }
         else if (succeeded)
         {
            Host.WriteLine("- Mark all writers as successfully backed up...");
         }
         else
         {
            Host.WriteLine("- Backup failed. Mark all writers as not successfully backed up...");
         }

         SetBackupSucceeded(succeeded);

         Host.WriteLine("Completing the backup (BackupComplete) ... ");

         m_backupComponents.BackupComplete();

         // Check selected writer status
         CheckSelectedWriterStatus();

      }

      private void SetBackupSucceeded(bool succeeded)
      {
         foreach (VssWriterDescriptor writer in m_writers)
         {
            foreach (VssComponentDescriptor component in writer.ComponentDescriptors.Where(c => c.IsExplicitlyIncluded))
            {
               m_backupComponents.SetBackupSucceeded(writer.WriterMetadata.InstanceId,
                  writer.WriterMetadata.WriterId,
                  component.ComponentType,
                  component.LogicalPath,
                  component.ComponentName,
                  succeeded);
            }
         }
      }

      internal void DeleteSnapshot(Guid snapshotId)
      {
         Host.WriteLine("- Deleting shadow copy {0:B}...", snapshotId);

         m_backupComponents.DeleteSnapshot(snapshotId, false);
      }

      internal void DeleteSnapshotSet(Guid snapshotSetId)
      {
         Host.WriteLine("- Deleting shadow copy set {0:B}...", snapshotSetId);

         int deletedSnapshots = m_backupComponents.DeleteSnapshotSet(snapshotSetId, false);

         Host.WriteLine("{0} snapshots were successfully deleted.", deletedSnapshots);
      }

      internal void DeleteAllSnapshots()
      {
         // Get a list of all shadow copies
         IList<VssSnapshotProperties> snapshots = m_backupComponents.QuerySnapshots().ToArray();

         if (snapshots.Count == 0)
         {
            Host.WriteLine("There were no shadow copies on the system.");
            return;
         }

         Host.PushIndent();
         try
         {
            foreach (VssSnapshotProperties snapshot in snapshots)
            {
               Host.WriteLine("- Deleting shadow copy {0:B} on {1} from provider {2} [{3}]...", snapshot.SnapshotId, snapshot.OriginalVolumeName, snapshot.ProviderId, snapshot.SnapshotAttributes);
               m_backupComponents.DeleteSnapshot(snapshot.SnapshotId, false);
            }
         }
         finally
         {
            Host.PopIndent();
         }
      }

      internal void DeleteOldestSnapshot(string volume)
      {
         string uniqueVolume = Volume.GetUniqueVolumeNameForPath(Host, volume, false);
         IList<VssSnapshotProperties> snapshots = m_backupComponents.QuerySnapshots().ToArray();

         if (snapshots.Count == 0)
         {
            Host.WriteLine("There are now shadow copies in the system.");
            return;
         }

         VssSnapshotProperties snapshot = snapshots
            .Where(snap => snap.OriginalVolumeName.Equals(uniqueVolume, StringComparison.OrdinalIgnoreCase))
            .OrderBy(snap => snap.CreationTimestamp).FirstOrDefault();

         if (snapshot == null)
         {
            Host.WriteLine("There are no shadow copies for the specified volume in the system.");
            return;
         }

         Host.WriteLine("- Deleting shadow copy {0:B} on {1} from provider {2} [{3}]...", snapshot.SnapshotId, snapshot.OriginalVolumeName, snapshot.ProviderId, snapshot.SnapshotAttributes);
         m_backupComponents.DeleteSnapshot(snapshot.SnapshotId, false);
      }

      internal void RevertToSnapshot(Guid snapshotId)
      {
         VssSnapshotProperties snapshot = m_backupComponents.GetSnapshotProperties(snapshotId);

         Host.WriteLine("Reverting to shadow copy {0:B} on {1} from provider {2:B} [{3}]...",
            snapshot.SnapshotId,
            snapshot.OriginalVolumeName,
            snapshot.ProviderId,
            snapshot.SnapshotAttributes);

         if (s_implementation.ShouldBlockRevert(snapshot.OriginalVolumeName))
         {
            Host.WriteWarning("Revert is disabled on the volume {0} because of writers.", snapshot.OriginalVolumeName);
            return;
         }

         m_backupComponents.RevertToSnapshot(snapshot.SnapshotId, true);

         IVssAsyncResult ar = m_backupComponents.BeginQueryRevertStatus(snapshot.OriginalVolumeName, null, null);
         m_backupComponents.EndQueryRevertStatus(ar);

         Host.WriteLine("The shadow copy has been successfully reverted.");
      }

      // Return the list of snapshot volume devices in this snapshot set
      private IList<string> GetSnapshotDevices(Guid snapshotSetID)
      {
         List<string> volumes = new List<string>();
         foreach (VssSnapshotProperties snapshot in m_backupComponents.QuerySnapshots().Where(snap => snap.SnapshotSetId == snapshotSetID))
         {
            // Get the snapshot device object name which is a volume guid name for persistent snapshot
            // and a device name for non persistent snapshot.
            // The volume guid name and the device name we obtained here might change after breaksnapshot
            // depending on if the disk signature is reverted, but those cached names should still work
            // as symbolic links, in which case they can not persist after reboot.
            Host.WriteLine("Will convert {0} to read-write...", snapshot.SnapshotDeviceObject);
            volumes.Add(snapshot.SnapshotDeviceObject);
         }

         return volumes;
      }


      internal void ExposeShapshotLocally(Guid snapshotId, string path)
      {
         Host.WriteLine("Exposing shadow copy {0:B} under the path '{1}'", snapshotId, path);

         VerifyExposeSnapshot(snapshotId);

         // Check if the path parameter is valid
         if (path.Length == 2 && path.EndsWith(":"))
         {
            Host.WriteLine("Checking if '{0}' is a valid drive letter...", path);

            try
            {
               Volume.QueryDosDevice(path);
               throw new ArgumentException(String.Format("The specified mount point [{0}] is a drive letter already in use.", path));
            }
            catch (Win32Exception ex)
            {
               if (ex.NativeErrorCode != 2)
                  throw;
            }
               
         }
         else
         {
            path = path.AppendBackslash();

            Host.WriteLine("Checking if '{0} is a valid empty directory...", path);

            if (!Directory.Exists(path))
               throw new ArgumentException(String.Format("The specified mount point [{0}] is not a valid directory.", path));

            if (Directory.GetFileSystemEntries(path).Any())
               throw new ArgumentException(String.Format("The specified mount point [{0}] is not an empty directory.", path));

         }
         // Expose the snapshot locally 
         m_backupComponents.ExposeSnapshot(snapshotId, null, VssVolumeSnapshotAttributes.ExposedLocally, path);

         Host.WriteLine("Shadow copy exposed as '{0}'", path);
      }

      private void VerifyExposeSnapshot(Guid snapshotId)
      {
         // Make sure that the expose operation is valid for this snapshot.
         // Get the shadow copy properties
         VssSnapshotProperties props;
         try
         {
            props = m_backupComponents.GetSnapshotProperties(snapshotId);
         }
         catch (VssObjectNotFoundException)
         {
            throw new ArgumentException("There is no snapshot with the given id.");
         }

         // Client Accessible snapshots are not exposable
         if ((props.SnapshotAttributes & VssVolumeSnapshotAttributes.ClientAccessible) != 0)
            throw new ArgumentException("The snapshot id identifies a client accessible snapshot which cannot be exposed.");

         if (!String.IsNullOrEmpty(props.ExposedName) || !String.IsNullOrEmpty(props.ExposedPath))
            throw new ArgumentException("Client-accessible (SFSF) snapshots cannot be exposed.");
      }

      internal void ExposeSnapshotRemotely(Guid snapshotId, string shareName, string pathFromRoot)
      {
         pathFromRoot = String.IsNullOrEmpty(pathFromRoot) ? null : pathFromRoot;

         Host.WriteLine("Exposing shadow copy {0:B} under the share '{1}' (path from root: '{2}')", snapshotId, shareName, pathFromRoot ?? "");

         VerifyExposeSnapshot(snapshotId);

         // Note: a true reqester should also check here if 
         // - the remote share name is valid (i.e. unused)
         // - the path from root is valid
         m_backupComponents.ExposeSnapshot(snapshotId, pathFromRoot, VssVolumeSnapshotAttributes.ExposedRemotely, shareName);
      }

      internal void ImportSnapshotSet()
      {
         Host.WriteLine("Importing the transportable shadow copy...");

         m_backupComponents.ImportSnapshots();

         Host.WriteLine("Shadow copy set successfully imported.");
      }

      internal void InitializeWriterComponentsForRestore()
      {
         Host.WriteLine("Initializing writer components for restore...");

         m_writerComponentsForRestore = new List<VssWriterDescriptor>();

         bool found = false;
         // Enumerate the list of writers in the metadata  
         foreach (IVssWriterComponents components in m_backupComponents.WriterComponents)
         {
            // Try to discover the name based on an existing writer (identical ID and instance ID).
            // Otherwise, ignore it!
            foreach (VssWriterDescriptor writer in m_writers)
            {
               // Do not check for instance id.
               if (components.WriterId != writer.WriterMetadata.WriterId)
                  continue;

               Host.WriteLine("Writer {0} is present in the Backup Components document and on the system. Considering for restore...", writer.WriterMetadata.WriterName);

               // Adding components
               writer.InitializeComponentsForRestore(components);

               m_writerComponentsForRestore.Add(writer);

               found = true;
            }

            if (!found)
            {
               Host.WriteLine("Writer with ID {0:B} is not present in the system. Ignoring...", components.WriterId);
            }
         }
      }

      internal void SelectComponentsForRestore(IList<string> excludedWriterAndComponentList, IList<string> includedWriterAndComponentList)
      {
         // First, exclude all components that have data outside of the shadow set
         DiscoverDirectlyExcludedComponents(excludedWriterAndComponentList, m_writerComponentsForRestore);

         // Exclude all writers that do not support restore events
         ExcludeWritersWithNoRestoreEvents();

         Host.WriteLine("Verifying explicitly specified writers/components...");

         foreach (string inc in includedWriterAndComponentList)
         {
            if (inc.Contains(':'))
               VerifyExplicitlyIncludedComponent(inc, m_writerComponentsForRestore);
            else
               VerifyExplicitlyIncludedWriter(inc, m_writerComponentsForRestore);
         }

         // Finally, select the explicitly included components
         SelectNonexcludedComponentsForRestore();
      }

      private void SelectNonexcludedComponentsForRestore()
      {
         Host.WriteLine("Select components for restore...");

         Host.PushIndent();
         try
         {
            foreach (VssWriterDescriptor writer in m_writerComponentsForRestore.Where(w => !w.IsExcluded))
            {
               Host.WriteLine("* Writer '{0}':", writer.WriterMetadata.WriterName);

               Host.PushIndent();
               try
               {
                  // Compute the roots of included components
                  foreach (var component in writer.ComponentDescriptors.Where(c => !c.IsExcluded))
                  {
                     Host.WriteLine("- Select component {0}", component.FullPath);
                     m_backupComponents.SetSelectedForRestore(writer.WriterMetadata.WriterId, component.ComponentType, component.LogicalPath, component.ComponentName, true);
                  }
               }
               finally
               {
                  Host.PopIndent();
               }
            }
         }
         finally
         {
            Host.PopIndent();
         }
      }

      private void ExcludeWritersWithNoRestoreEvents()
      {
         Host.WriteLine("Exclude writers that do not support restore events...");


         foreach (var writer in m_writerComponentsForRestore.Where(w => !w.IsExcluded))
         {
            if (writer.WriterMetadata.RestoreMethod.WriterRestore == VssWriterRestore.Never)
            {
               Host.WriteLine("Excluding writer '{0}' since it does not support restore events.", writer.WriterMetadata.WriterName);
               writer.IsExcluded = true;
            }
         }
      }

      internal void PreRestore()
      {
         Host.WriteLine("Sending the PreRestore event...");
         m_backupComponents.PreRestore();
      }

      internal void SetFileRestoreStatus(bool successfullyRestored)
      {
         Host.WriteLine("Set restore status for all components for restore...");

         // All-or-nothing policy
         //
         // WARNING: this might be insufficient since we cannot distinguish
         // between a partial failed restore and a completely failed restore!
         // A true requester should be able to make this difference (see the documentation for more details)
         VssFileRestoreStatus restoreStatus = successfullyRestored ? VssFileRestoreStatus.All : VssFileRestoreStatus.None;

         try
         {
            Host.PushIndent();

            foreach (VssWriterDescriptor writer in m_writerComponentsForRestore.Where(w => !w.IsExcluded))
            {

               Host.WriteLine("* Writer '{0}':", writer.WriterMetadata.WriterName);

               try
               {
                  Host.PushIndent();
                  foreach (VssComponentDescriptor component in writer.ComponentDescriptors.Where(c => !c.IsExcluded))
                  {
                     Host.WriteLine("- Select component {0}", component.FullPath);
                     m_backupComponents.SetFileRestoreStatus(writer.WriterMetadata.WriterId, component.ComponentType, component.LogicalPath, component.ComponentName, restoreStatus);
                  }
               }
               finally
               {
                  Host.PopIndent();
               }
            }
         }
         finally
         {
            Host.PopIndent();
         }
      }

      internal void PostRestore()
      {
         Host.WriteLine("Sending the PostRestore event...");
         m_backupComponents.PostRestore();
      }
   }
}
