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
using Alphaleonis.Win32.Filesystem;
using Alphaleonis.Win32.Vss;
using System.Diagnostics;

namespace AlphaShadow.Commands
{
   class CreateCommand : ContextCommand
   {
      private static readonly OptionSpec OptExecCommand = CommonOptions.OptExecCommand.WithHelpText("Custom command executed after shadow creation.");

      private static OptionSpec[] s_options = new OptionSpec[] 
      {
         new OptionSpec("", OptionType.MultipleValuesRequired, "Specifies the volumes on which to create shadow copies.", true, "VolumeNames..."),
         CommonOptions.OptTransportable,
         CommonOptions.OptNonTransportableDoc,
         CommonOptions.OptVerifyWriterIncluded,
         CommonOptions.OptExcludeWriter,
         CommonOptions.OptSetVarScript,
         OptExecCommand,
         CommonOptions.OptExecCommandArgs
      };      

      public CreateCommand()
         : base("create", "Creates a shadow set on the specified volume(s).")
      {         
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return s_options.Concat(base.CommandSpecificOptions);
         }
      }

      public IList<string> VolumeList { get; private set; }
      public HashSet<string> ExcludedWriters { get; private set; }
      public HashSet<string> IncludedWriters { get; private set; }
      public string BackupComponentsDoc { get; private set; }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();

         VolumeList = new List<string>();
         ExcludedWriters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         IncludedWriters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         if (HasOption(CommonOptions.OptTransportable))
         {
            if (HasOption(CommonOptions.OptNonTransportableDoc))
               throw new ArgumentException(String.Format("At most one of the options /{0} and /{1} must be specified.", CommonOptions.OptNonTransportableDoc, CommonOptions.OptTransportable));

            BackupComponentsDoc = GetOptionValue<string>(CommonOptions.OptTransportable);
            Host.WriteLine("(Option: Transportable shadow set. Saving xml to file '{0}')", BackupComponentsDoc);
            Context |= VssVolumeSnapshotAttributes.Transportable;
         }

         if (HasOption(CommonOptions.OptNonTransportableDoc))
         {
            BackupComponentsDoc = GetOptionValue<string>(CommonOptions.OptNonTransportableDoc);
            Host.WriteLine("(Option: Non-Transportable shadow set. Saving xml to file '{0}')", BackupComponentsDoc);
         }

         if (HasOption(CommonOptions.OptExcludeWriter))
         {
            foreach (string writerName in GetOptionValues<string>(CommonOptions.OptExcludeWriter))
            {
               if (ExcludedWriters.Add(writerName))
               {
                  Host.WriteLine("(Option: Excluding writer/component '{0}')", writerName);
               }
            }
         }

         if (HasOption(CommonOptions.OptVerifyWriterIncluded))
         {
            foreach (string writerName in GetOptionValues<string>(CommonOptions.OptVerifyWriterIncluded))
            {
               if (ExcludedWriters.Contains(writerName))
                  throw new ArgumentException(String.Format("A writer cannot be both included and excluded: '{0}'.", writerName));

               if (IncludedWriters.Add(writerName))
               {
                  Host.WriteLine("(Option: Verifying inclusion of writer/component '{0}')", writerName);
               }
            }
         }

         foreach (string arg in RemainingArguments)
         {
            if (!Volume.IsVolume(Host, arg))
               throw new ArgumentException(String.Format("{0} is not a valid volume.", arg));

            string volumeName = Volume.GetUniqueVolumeNameForPath(Host, arg, true);
            VolumeList.Add(volumeName);
         }
      }


      public override void Run()
      {
         Host.WriteVerbose("- Attempting to create a shadow copy set for the volumes: {0}", String.Join(",", VolumeList.ToArray()));

         UpdateFinalContext();
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(Context);

            // Create the shadow copy set
            client.CreateSnapshot(VolumeList, BackupComponentsDoc, ExcludedWriters, IncludedWriters);

            // Execute BackupComplete, except in fast snapshot creation
            if ((Context & VssVolumeSnapshotAttributes.DelayedPostSnapshot) == 0)
            {

               try
               {
                  if (HasOption(CommonOptions.OptSetVarScript))
                  {
                     client.GenerateSetvarScript(GetOptionValue<string>(CommonOptions.OptSetVarScript));
                  }

                  if (HasOption(OptExecCommand))
                  {
                     string arguments = String.Empty;
                     if (HasOption(CommonOptions.OptExecCommandArgs))
                     {
                        arguments = GetOptionValue<string>(CommonOptions.OptExecCommandArgs);
                     }
                     Host.ExecCommand(GetOptionValue<string>(OptExecCommand), arguments);
                  }
               }
               catch (Exception)
               {
                  // Mark backup failure and exit
                  if ((Context & VssVolumeSnapshotAttributes.NoWriters) == 0)
                     client.BackupComplete(false);

                  throw;
               }

               // Complete the backup
               // Note that this will notify writers that the backup is succesful! 
               // (which means eventually log truncation)
               if ((Context & VssVolumeSnapshotAttributes.NoWriters) == 0)
                  client.BackupComplete(true);
               
            }

            Host.WriteLine("Snapshot creation done.");
         }         
      }

      
   }
}
