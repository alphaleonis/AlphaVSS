
using System;
using System.Collections.Generic;
using Alphaleonis.Win32.Vss;

namespace AlphaShadow.Commands
{
   abstract class ContextCommand : AlphaShadowCommand
   {
      public ContextCommand(string name, string description)
         : base(name, description)
      {
         Context = (VssVolumeSnapshotAttributes)VssSnapshotContext.Backup;
      }

      protected VssVolumeSnapshotAttributes Context { get; set; }

      public override IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            return ContextOptions.All;
         }
      }

      protected bool Persistent { get; private set; }
      protected bool NoWriters { get; private set; }

      protected override void ProcessOptions()
      {
         // Set default context 
         Context = (VssVolumeSnapshotAttributes)VssSnapshotContext.Backup;
         base.ProcessOptions();

         Persistent = HasOption(ContextOptions.OptPersistent);
         if (Persistent)
            Host.WriteLine("(Option: Persistent shadow copy)");

         NoWriters = HasOption(ContextOptions.OptNoWriters);
         if (NoWriters)
            Host.WriteLine("(Option: No-writers)");

         if (HasOption(ContextOptions.OptNoAutoRecovery))
         {
            Host.WriteLine("(Option: No auto recovery)");
            Context |= VssVolumeSnapshotAttributes.NoAutoRecovery;
         }

         if (HasOption(ContextOptions.OptTxFRecovered))
         {
            Host.WriteLine("(Option: TxF Recovery)");
            Context |= VssVolumeSnapshotAttributes.TxFRecovery;
         }

         if (HasOption(ContextOptions.OptDifferential))
         {
            Host.WriteLine("(Option: Creating differential HW shadow copies)");
            Context |= VssVolumeSnapshotAttributes.Differential;
         }

         if (HasOption(ContextOptions.OptPlex))
         {
            Host.WriteLine("(Option: Creating plex HW shadow copies)");
            Context |= VssVolumeSnapshotAttributes.Plex;
         }

         if (HasOption(ContextOptions.OptSharedFolders))
         {
            Host.WriteLine("(Option: Creating shadow copies for Shared Folders - Client Accessible)");
            Context |= VssVolumeSnapshotAttributes.ClientAccessible;
         }

      }

      protected void UpdateFinalContext()
      {
         if (Persistent)
         {
            if (!NoWriters)
               Context |= (VssVolumeSnapshotAttributes)VssSnapshotContext.AppRollback;
            else
               Context |= (VssVolumeSnapshotAttributes)VssSnapshotContext.NasRollback;
         }
         else
         {
            if (!NoWriters)
               Context |= (VssVolumeSnapshotAttributes)VssSnapshotContext.Backup;
            else
               Context |= (VssVolumeSnapshotAttributes)VssSnapshotContext.FileShareBackup;
         }
         Console.WriteLine("Final context: {0}", (int)Context);
      }

   }
}
