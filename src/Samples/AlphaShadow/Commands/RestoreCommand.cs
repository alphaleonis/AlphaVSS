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
using System.IO;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class RestoreCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptFile = new OptionSpec("", OptionType.SingleValueRequired, "The previously generated Backup Components document to base the restore on.", true, "file.xml");
      public readonly OptionSpec OptExecCommand = CommonOptions.OptExecCommand.WithHelpText("The command to execute between PreRestore and PostRestore.");
      private readonly OptionSpec OptSimulated = new OptionSpec("simulate", OptionType.ValueProhibited, "Specified to just perform a restore simulation.", false);

      public RestoreCommand()
         : base("restore", "Restore based on a previously-generated Backup Components document")
      {
         
      }
      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return new[] { OptFile, 
                            OptExecCommand, 
                            CommonOptions.OptExecCommandArgs, 
                            CommonOptions.OptVerifyWriterIncluded, 
                            CommonOptions.OptExcludeWriter,
                            OptSimulated
            };
         }
      }

      public string FileName { get; set; }
      public HashSet<string> ExcludedWriters { get; private set; }
      public HashSet<string> IncludedWriters { get; private set; }
      public bool Simulate { get; set; }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();
         ExcludedWriters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         IncludedWriters = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
         FileName = RemainingArguments.Single();
         if (!File.Exists(FileName))
            throw new ArgumentException(String.Format("The specified file '{0}' does not exist.", FileName));

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

         Simulate = HasOption(OptSimulated);
      }


      public override void Run()
      {
         Host.WriteLine("Performing a{0} restore", Simulate ? " simulated" : "");

         string xmlDoc = File.ReadAllText(FileName);
         Host.WriteVerbose("XML Document:\n{0}", xmlDoc);

         using (VssClient client = new VssClient(Host))
         {
            // Initialize the VSS client
            client.Initialize(VssSnapshotContext.All, xmlDoc, true);

            // Gather writer metadata
            client.GatherWriterMetadata();

            // Gather writer status
            client.GatherWriterStatus();

            // List writer status
            client.ListWriterStatus();

            // Initialize the list of writers and components for restore
            client.InitializeWriterComponentsForRestore();

            // Select required components for restore
            client.SelectComponentsForRestore(ExcludedWriters.ToList(), IncludedWriters.ToList());

            if (Simulate)
            {
               Host.WriteLine("Restore simulation done.");
               return;
            }

            // Issue a PreRestore event to the writers
            client.PreRestore();

            // Execute the optional custom command between PreRestore and PostRestore
            try
            {
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
               // Notify writers about a failed restore
               client.SetFileRestoreStatus(false);

               // Issue a PostRestore event to the writers
               client.PostRestore();

               throw;
            }
                        
            // Notify writers about a succesful restore
            client.SetFileRestoreStatus(true);

            // Issue a PostRestore event to the writers
            client.PostRestore();

            // Check selected writer status
            client.CheckSelectedWriterStatus();

            Host.WriteLine("Restore done.");

         }
      }
   }
}
