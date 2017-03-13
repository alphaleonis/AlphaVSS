
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class RevertCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy to revert to.", true, "SnapshotID");

      public RevertCommand()
         : base("revert", "Revert a volume to the specified shadow copy.")
      {
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return new[] { OptSnapshotID };
         }
      }

      public override void Run()
      {

         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All);
            client.RevertToSnapshot(GetOptionValue<Guid>(OptSnapshotID));
         }
      }
   }
}
