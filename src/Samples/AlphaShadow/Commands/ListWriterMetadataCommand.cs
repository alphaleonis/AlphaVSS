
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaShadow.Commands
{
   class ListWriterMetadataCommand : ContextCommand
   {
      public OptionSpec OptDetailed = new OptionSpec("detailed", OptionType.ValueProhibited, "Displays detailed information about components.", false);
      public OptionSpec OptXml = new OptionSpec("xml", OptionType.ValueProhibited, "Displays data as XML", false);

      public ListWriterMetadataCommand()
         : base("lwm", "List writer metadata")
      {
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return base.CommandSpecificOptions.Concat(new[] { OptDetailed, OptXml });
         }
      }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();
         if (HasOption(OptDetailed) && HasOption(OptXml))
            throw new ArgumentException(String.Format("At most one of the options {0} and {1} must be specified.", OptDetailed, OptXml));
      }

      public override void Run()
      {
         UpdateFinalContext();
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(Context);
            if (HasOption(OptXml))
            {
               client.GatherWriterMetadataToScreen();
            }
            else
            {
               client.GatherWriterMetadata();
               client.ListWriterMetadata(HasOption(OptDetailed));
            }
         }
      }

       public override async Task RunAsync(CancellationToken cancellationToken)
       {
           UpdateFinalContext();
           using (VssClient client = new VssClient(Host))
           {
               client.Initialize(Context);
               if (HasOption(OptXml))
               {
                   await client.GatherWriterMetadataToScreenAsync(cancellationToken);
               }
               else
               {
                   await client.GatherWriterMetadataAsync(cancellationToken);
                   client.ListWriterMetadata(HasOption(OptDetailed));
               }
           }
       }

    }
}
