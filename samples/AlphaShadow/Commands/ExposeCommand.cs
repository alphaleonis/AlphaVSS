
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   class ExposeCommand : AlphaShadowCommand
   {
      private readonly OptionSpec OptSnapshotID = new OptionSpec("sid", OptionTypes.SingleValueRequired, "Specifies the ID of the shadow copy to expose.", true, "SnapshotID");      
      private readonly OptionSpec OptMountPoint = new OptionSpec("mountPoint", OptionTypes.SingleValueRequired, "Specifies the path to the directory to mount the exposed shadow in. Cannot be used with -share.", false, "path");
      private readonly OptionSpec OptDir = new OptionSpec("childDir", OptionTypes.SingleValueRequired, "Specifies the child directory of the snapshot to expose. Only valid together with -share.", false, "path");
      private readonly OptionSpec OptShare = new OptionSpec("share", OptionTypes.SingleValueRequired, "Specifies the share to expose the shadow copy as. Cannot be used with -mountPoint.", false, "share");

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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      public override async Task RunAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
