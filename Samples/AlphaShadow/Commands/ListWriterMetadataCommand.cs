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

   }
}
