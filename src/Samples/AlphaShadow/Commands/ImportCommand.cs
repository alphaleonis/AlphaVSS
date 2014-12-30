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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
   }
}
