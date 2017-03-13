
using System;

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

   }
}