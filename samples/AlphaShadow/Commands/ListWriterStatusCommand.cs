
using System;
using System.Threading.Tasks;

namespace AlphaShadow.Commands
{
   class ListWriterStatusCommand : ContextCommand
   {
      public ListWriterStatusCommand()
         : base("lws", "List writer status")
      {
      }

      public override async Task RunAsync()
      {
         UpdateFinalContext();
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(Context);
            await client.GatherWriterMetadataAsync().ConfigureAwait(false);
            await client.GatherWriterStatusAsync().ConfigureAwait(false);
            client.ListWriterStatus();
         }
      }

   }
}