using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaShadow.Commands
{
    class ListWriterStatusCommand : ContextCommand
    {
        public ListWriterStatusCommand()
            : base("lws", "List writer status")
        {
        }

        public override void Run()
        {
            UpdateFinalContext();
            using (VssClient client = new VssClient(Host))
            {
                client.Initialize(Context);
                client.GatherWriterMetadata();
                client.GatherWriterStatus();
                client.ListWriterStatus();
            }
        }

        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            UpdateFinalContext();
            using (VssClient client = new VssClient(Host))
            {
                client.Initialize(Context);
                await client.GatherWriterMetadataAsync(cancellationToken);
                await client.GatherWriterStatusAsync(cancellationToken);
                client.ListWriterStatus();
            }
        }
    }
}