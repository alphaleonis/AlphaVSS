

namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssWriterState"/> enumeration indicates the current state of the writer.</summary>
   /// <remarks>A requester determines the state of a writer through <see cref="IVssBackupComponents.WriterStatus"/>.</remarks>
   public enum VssWriterState
   {
      /// <summary><para>The writer's state is not known.</para><para>This indicates an error on the part of the writer.</para></summary>
      Unknown = 0,
      /// <summary>The writer has completed processing current shadow copy events and is ready to proceed, or <c>CVssWriter::OnPrepareSnapshot</c> has not yet been called.</summary>
      Stable = 1,
      /// <summary>The writer is waiting for the freeze state.</summary>
      WaitingForFreeze = 2,
      /// <summary>The writer is waiting for the thaw state.</summary>
      WaitingForThaw = 3,
      /// <summary>The writer is waiting for the <c>PostSnapshot</c> state.</summary>
      WaitingForPostSnapshot = 4,
      /// <summary>The writer is waiting for the requester to finish its backup operation.</summary>
      WaitingForBackupComplete = 5,
      /// <summary>The writer vetoed the shadow copy creation process at the writer identification state.</summary>
      FailedAtIdentify = 6,
      /// <summary>The writer vetoed the shadow copy creation process during the backup preparation state.</summary>
      FailedAtPrepareBackup = 7,
      /// <summary>The writer vetoed the shadow copy creation process during the <c>PrepareForSnapshot</c> state.</summary>
      FailedAtPrepareSnapshot = 8,
      /// <summary>The writer vetoed the shadow copy creation process during the freeze state.</summary>
      FailedAtFreeze = 9,
      /// <summary>The writer vetoed the shadow copy creation process during the thaw state.</summary>
      FailedAtThaw = 10,
      /// <summary>The writer vetoed the shadow copy creation process during the <c>PostSnapshot</c> state.</summary>
      FailedAtPostSnapshot = 11,
      /// <summary>The shadow copy has been created and the writer failed during the <c>BackupComplete</c> state. 
      /// A writer should save information about this failure to the error log.</summary>
      FailedAtBackupComplete = 12,
      /// <summary>The writer failed during the <c>PreRestore</c> state.</summary>
      FailedAtPreRestore = 13,
      /// <summary>The writer failed during the <c>PostRestore</c> state.</summary>
      FailedAtPostRestore = 14,
      /// <summary>The writer failed during the shutdown of the backup application.</summary>
      FailedAtBackupShutdown = 15,
   }
}
