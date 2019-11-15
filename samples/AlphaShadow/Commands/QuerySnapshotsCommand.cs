
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   public class QueryCommand : AlphaShadowCommand
   {
      private OptionSpec OptSnapshotID = new OptionSpec("sid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy for which to display information.", false, "SnapshotID");
      private OptionSpec OptSnapshotSetID = new OptionSpec("ssid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy set for which to list shadow copies.", false, "SnapshotSetID");

      public QueryCommand()
         : base("query", "Lists information about shadow copies in the system.")
      {         
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            yield return OptSnapshotID;
            yield return OptSnapshotSetID;
         }
      }

      public Guid? SnapshotID { get; private set; }
      public Guid? SnapdhotSetID { get; private set; }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();
         if (HasOption(OptSnapshotID) && HasOption(OptSnapshotSetID))
            throw new ArgumentException(String.Format("At most one of the options {0} and {1} must be specified.", OptSnapshotID, OptSnapshotSetID));

         if (HasOption(OptSnapshotID))
            SnapshotID = GetOptionValue<Guid>(OptSnapshotID);

         if (HasOption(OptSnapshotSetID))
            SnapdhotSetID = GetOptionValue<Guid>(OptSnapshotSetID);
      }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      public override async Task RunAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
      {
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All, null, false);

            if (SnapshotID.HasValue)
               client.GetSnapshotProperties(SnapshotID.Value);
            else
               client.QuerySnapshotSet(SnapdhotSetID.HasValue ? SnapdhotSetID.Value : Guid.Empty);
         }
      }
   }
}
