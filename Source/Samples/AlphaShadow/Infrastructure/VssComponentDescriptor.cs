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
using System.Collections.ObjectModel;

namespace AlphaShadow
{
   enum VssFileDescriptorType
   {
      Undefined,
      ExcludedFile,
      File,
      Database,
      DatabaseLog
   }

   class VssComponentDescriptor 
   {
      private List<string> m_affectedPaths;
      private List<string> m_affectedVolumes ;

      /// <summary>
      /// Initializes a new instance of the VssComponentDescriptor class.
      /// </summary>
      public VssComponentDescriptor(IUIHost host, string writerName, IVssComponent component)
      {
         if (host == null)
            throw new ArgumentNullException("host", "host is null.");
         
         if (component == null)
            throw new ArgumentNullException("component", "component is null.");

         WriterName = writerName;

         ComponentName = component.ComponentName;
         ComponentType = component.ComponentType;
         LogicalPath = component.LogicalPath;

         if (!LogicalPath.EndsWith("\\"))
            FullPath = LogicalPath + ComponentName;
         else
            FullPath = LogicalPath + "\\" + ComponentName;

         if (!FullPath.StartsWith("\\"))
            FullPath = "\\" + FullPath;
      }

      public VssComponentDescriptor(IUIHost host, string writerName, IVssWMComponent component)
      {
         if (component == null)
            throw new ArgumentNullException("component", "component is null.");
         
         WriterName = writerName;

         FullPath = ExtensionMethods.AppendBackslash(component.LogicalPath) + component.ComponentName;
         if (!FullPath.StartsWith("\\"))
            FullPath = "\\" + FullPath;

         HashSet<string> affectedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         HashSet<string> affectedVolumes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

         foreach (VssWMFileDescriptor file in component.Files)
         {
            affectedPaths.Add(file.GetExpandedPath());
            affectedVolumes.Add(file.GetAffectedVolume(host));
         }

         foreach (VssWMFileDescriptor file in component.DatabaseFiles)
         {
            affectedPaths.Add(file.GetExpandedPath());
            affectedVolumes.Add(file.GetAffectedVolume(host));
         }

         foreach (VssWMFileDescriptor file in component.DatabaseLogFiles)
         {
            affectedPaths.Add(file.GetExpandedPath());
            affectedVolumes.Add(file.GetAffectedVolume(host));
         }

         m_affectedPaths = new List<string>(affectedPaths.OrderBy(path => path, StringComparer.OrdinalIgnoreCase));
         m_affectedVolumes = new List<string>(affectedVolumes.OrderBy(path => path, StringComparer.OrdinalIgnoreCase));
         Caption = component.Caption;
         ComponentName = component.ComponentName;
         LogicalPath = component.LogicalPath;
         RestoreMetadata = component.RestoreMetadata;
         IsSelectable = component.Selectable;
         ComponentType = component.Type;
         NotifyOnBackupComplete = component.NotifyOnBackupComplete;
         
         Files = new List<VssWMFileDescriptor>(component.Files);
         DatabaseFiles = new List<VssWMFileDescriptor>(component.DatabaseFiles);
         DatabaseLogFiles = new List<VssWMFileDescriptor>(component.DatabaseLogFiles);
         Dependencies = new List<VssWMDependency>(component.Dependencies);
      }      

      public bool IsTopLevel { get; set; }

      public string FullPath { get; private set; }     

      public string WriterName { get; set; }

      public bool IsAncestorOf(VssComponentDescriptor descendent)
      {
         // The child must have a longer full path
         if (descendent.FullPath.Length <= FullPath.Length)
            return false;

         string fullPathWithBackslash = FullPath.AppendBackslash();

         return descendent.FullPath.AppendBackslash().StartsWith(FullPath.AppendBackslash(), StringComparison.OrdinalIgnoreCase);
      }

      public bool IsExcluded { get; set; }

      public bool IsExplicitlyIncluded { get; set; }

      public IList<string> AffectedPaths
      {
         get
         {
            return new ReadOnlyCollection<string>(m_affectedPaths);
         }
      }

      public IList<string> AffectedVolumes
      {
         get
         {
            return new ReadOnlyCollection<string>(m_affectedVolumes);
         }
      }


      public bool CanBeExplicitlyIncluded
      {
         get
         {
            if (IsExcluded)
               return false;

            if (IsSelectable)
               return true;

            if (IsTopLevel)
               return true;

            return false;
         }
      }

      public string Caption { get; set; }

      public string ComponentName { get; set; }

      public string LogicalPath { get; set; }

      public bool RestoreMetadata { get; set; }

      public bool IsSelectable { get; set; }

      public VssComponentType ComponentType { get; set; }

      public bool NotifyOnBackupComplete { get; set; }

      public List<VssWMFileDescriptor> Files { get; set; }

      public List<VssWMFileDescriptor> DatabaseLogFiles { get; set; }

      public List<VssWMFileDescriptor> DatabaseFiles { get; set; }

      public List<VssWMDependency> Dependencies { get; set; }
   }
}
