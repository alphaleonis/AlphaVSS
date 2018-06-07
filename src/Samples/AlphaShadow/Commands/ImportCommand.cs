
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class ImportCommand : AlphaShadowCommand
   {
      public readonly OptionSpec OptFile = new OptionSpec("", OptionType.SingleValueRequired, "The backup components document specifying the shadow copy to import.", true, "file.xml");
      public readonly OptionSpec OptExecCommand = CommonOptions.OptExecCommand.WithHelpText("The command to execute after importing the shadow copy.");

      public ImportCommand()
         : base("import", "Imports a transportable shadow copy.")
      {
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return new[] { OptFile, OptExecCommand, CommonOptions.OptExecCommandArgs };
         }
      }

      protected override void ProcessOptions()
      {
         XmlDocFile = RemainingArguments.Single();
         if (!File.Exists(XmlDocFile))
            throw new ArgumentException(String.Format("The specified file '{0}' does not exist.", XmlDocFile));

      }

      public string XmlDocFile { get; set; }


      public override void Run()
      {
         Host.WriteLine("Importing shadow copy set from file '{0}'", XmlDocFile);

         string xmlDoc = File.ReadAllText(XmlDocFile);

         Host.WriteVerbose("XML document:\n{0}", xmlDoc);

         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All, xmlDoc);
            client.ImportSnapshotSet();

            if (HasValue(OptExecCommand))
            {
               string arguments = String.Empty;
               if (HasOption(CommonOptions.OptExecCommandArgs))
               {
                  arguments = GetOptionValue<string>(CommonOptions.OptExecCommandArgs);
               }
               Host.ExecCommand(GetOptionValue<string>(OptExecCommand), arguments);
            }
         }
       }


       public override async Task RunAsync(CancellationToken cancellationToken)
       {
           Host.WriteLine("Importing shadow copy set from file '{0}'", XmlDocFile);

           string xmlDoc = File.ReadAllText(XmlDocFile);

           Host.WriteVerbose("XML document:\n{0}", xmlDoc);

           using (VssClient client = new VssClient(Host))
           {
               client.Initialize(VssSnapshotContext.All, xmlDoc);
               await client.ImportSnapshotSetAsync(cancellationToken);

               if (HasValue(OptExecCommand))
               {
                   string arguments = String.Empty;
                   if (HasOption(CommonOptions.OptExecCommandArgs))
                   {
                       arguments = GetOptionValue<string>(CommonOptions.OptExecCommandArgs);
                   }
                   Host.ExecCommand(GetOptionValue<string>(OptExecCommand), arguments);
               }
           }
       }
    }
}
