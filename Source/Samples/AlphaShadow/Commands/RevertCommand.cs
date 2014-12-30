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
