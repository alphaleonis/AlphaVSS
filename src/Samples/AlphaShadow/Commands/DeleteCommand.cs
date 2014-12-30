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
using Alphaleonis.Win32.Vss;
using System.Collections.Generic;
using System.Linq;

namespace AlphaShadow.Commands
{
   class DeleteCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptAll = new OptionSpec("all", OptionType.ValueProhibited, "Delete all shadow copies.", false);
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy to delete.", false, "SnapshotID");
      private readonly OptionSpec OptSnapshotSetID = new OptionSpec("ssid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy set to delete.", false, "SnapshotSetID");
      private readonly OptionSpec OptOldest = new OptionSpec("oldest", OptionType.SingleValueRequired, "Delete the oldest snapshot of the specified volume.", false, "volume");

      public Guid? SnapshotID { get; private set; }
      public Guid? SnapshotSetID { get; private set; }

      public DeleteCommand()
         : base("delete", "Deletes one or more shadow copies on the system.")
      {         
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return new OptionSpec[] { OptAll, OptSnapshotID, OptSnapshotSetID, OptOldest };
         }
      }
      protected override void ProcessOptions()
      {
         base.ProcessOptions();

         OptionSpec[] options = new[] { OptAll, OptSnapshotID, OptSnapshotSetID, OptOldest };
         int optionCount = options.Count(opt => HasOption(opt));
         if (optionCount != 1)
            throw new ArgumentException(String.Format("One and only one of the options {0} must be specified.", String.Join(", ", options.Select(opt => opt.ToString()).ToArray())));

         if (HasOption(OptSnapshotID))
            SnapshotID = GetOptionValue<Guid>(OptSnapshotID);

         if (HasOption(OptSnapshotSetID))
            SnapshotSetID = GetOptionValue<Guid>(OptSnapshotSetID);
      }

      public override void Run()
      {
         using (VssClient client = new VssClient(Host))
         {
            if (HasOption(OptAll))
            {
               Host.WriteWarning("This will delete all shadow copies on the system.");
               if (!Host.ShouldContinue())
                  return;
            }

            client.Initialize(VssSnapshotContext.All);
            if (SnapshotID.HasValue)
            {
               Host.WriteLine("Delete the snapshot with id {0:B}", SnapshotID.Value);
               client.DeleteSnapshot(SnapshotID.Value);
            }
            else if (SnapshotSetID.HasValue)
            {
               Host.WriteLine("Delete the snapshot set with id {0:B}", SnapshotSetID.Value);
               client.DeleteSnapshotSet(SnapshotSetID.Value);
            }
            else if (HasOption(OptAll))
            {               
               client.DeleteAllSnapshots();
            }
            else
            {
               string volume = GetOptionValue<string>(OptOldest);
               Host.WriteLine("Delete the oldest snapshot for volume {0}", volume);
               client.DeleteOldestSnapshot(volume);
            }
         }
      }
   }
}
