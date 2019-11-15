
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class RevertCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy to revert to.", true, "SnapshotID");

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

      public override async Task RunAsync()
      {

         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All);
            await client.RevertToSnapshotAsync(GetOptionValue<Guid>(OptSnapshotID)).ConfigureAwait(false);
         }
      }
   }
}
