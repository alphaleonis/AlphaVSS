
using System;
using Alphaleonis.Win32.Vss;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlphaShadow.Commands
{
   class DeleteCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptAll = new OptionSpec("all", OptionTypes.ValueProhibited, "Delete all shadow copies.", false);
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy to delete.", false, "SnapshotID");
      private readonly OptionSpec OptSnapshotSetID = new OptionSpec("ssid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy set to delete.", false, "SnapshotSetID");
      private readonly OptionSpec OptOldest = new OptionSpec("oldest", OptionTypes.SingleValueRequired, "Delete the oldest snapshot of the specified volume.", false, "volume");

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      public override async Task RunAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
