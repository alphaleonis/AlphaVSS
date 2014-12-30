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
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class ExposeCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionType.SingleValueRequired, "Specifies the ID of the shadow copy to expose.", true, "SnapshotID");      
      private readonly OptionSpec OptMountPoint = new OptionSpec("mountPoint", OptionType.SingleValueRequired, "Specifies the path to the directory to mount the exposed shadow in. Cannot be used with -share.", false, "path");
      private readonly OptionSpec OptDir = new OptionSpec("childDir", OptionType.SingleValueRequired, "Specifies the child directory of the snapshot to expose. Only valid together with -share.", false, "path");
      private readonly OptionSpec OptShare = new OptionSpec("share", OptionType.SingleValueRequired, "Specifies the share to expose the shadow copy as. Cannot be used with -mountPoint.", false, "share");

      public ExposeCommand()
         : base("expose", "Exposes a shadow copy locally or as a share.")
      {         
      }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return new OptionSpec[] { OptSnapshotID, OptMountPoint, OptDir, OptShare };
         }
      }

      public Guid SnapshotId { get; set; }

      protected override void ProcessOptions()
      {
         base.ProcessOptions();
         SnapshotId = GetOptionValue<Guid>(OptSnapshotID);

         if (!HasValue(OptMountPoint) && !HasValue(OptShare) || HasValue(OptMountPoint) && HasValue(OptShare))
            throw new ArgumentException(String.Format("Exactly one of the options {0} or {1} must be specified.", OptMountPoint, OptShare));

         if (HasValue(OptDir) && !HasValue(OptShare))
            throw new ArgumentException(String.Format("Option {0} can only be specified together with {1}.", OptDir, OptShare));
      }

      public override void Run()
      {
         using (VssClient client = new VssClient(Host))
         {
            client.Initialize(VssSnapshotContext.All);

            if (HasValue(OptMountPoint))
               client.ExposeShapshotLocally(SnapshotId, GetOptionValue<string>(OptMountPoint));
            else
               client.ExposeSnapshotRemotely(SnapshotId, GetOptionValue<string>(OptShare), HasOption(OptDir) ? GetOptionValue<string>(OptDir) : null);
         }
      }
   }
}
