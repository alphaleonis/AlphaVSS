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
   public class QueryCommand : AlphaShadowCommand
   {
      private OptionSpec OptSnapshotID = new OptionSpec("sid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy for which to display information.", false, "SnapshotID");
      private OptionSpec OptSnapshotSetID = new OptionSpec("ssid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy set for which to list shadow copies.", false, "SnapshotSetID");

      public QueryCommand()
         : base("query", "Lists information about shadow copies in the system.")
      {         
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            yield return OptSnapshotID;
            yield return OptSnapshotSetID;
         }
      }

      public Guid? SnapshotID { get; private set; }
      public Guid? SnapdhotSetID { get; private set; }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();
         if (HasOption(OptSnapshotID) && HasOption(OptSnapshotSetID))
            throw new ArgumentException(String.Format("At most one of the options {0} and {1} must be specified.", OptSnapshotID, OptSnapshotSetID));

         if (HasOption(OptSnapshotID))
            SnapshotID = GetOptionValue<Guid>(OptSnapshotID);

         if (HasOption(OptSnapshotSetID))
            SnapdhotSetID = GetOptionValue<Guid>(OptSnapshotSetID);
      }

      public override void Run()
      {
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All, null, false);

            if (SnapshotID.HasValue)
               client.GetSnapshotProperties(SnapshotID.Value);
            else
               client.QuerySnapshotSet(SnapdhotSetID.HasValue ? SnapdhotSetID.Value : Guid.Empty);
         }
      }
   }
}
