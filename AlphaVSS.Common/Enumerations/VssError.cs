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
namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// 	<para>
   /// 		The <see cref="VssError"/> enumeration represents error- and success codes that may be
   /// 		returned by some Vss methods.
   /// 	</para>
   /// </summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
   public enum VssError : uint
   {
      /// <summary>Indication of a successful operation.</summary>
      Success = 0x00,

      /// <summary>
      /// The asynchronous operation was cancelled.
      /// </summary>
      AsyncCanceled = 0x0004230B,

      /// <summary>
      /// The asynchronous operation was completed successfully.
      /// </summary>
      AsyncFinished = 0x0004230A,

      /// <summary>
      /// The asynchronous operation is still running.
      /// </summary>
      AsyncPending = 0x00042309,

      /// <summary>
      /// Unexpected error. The error code is logged in the error log file.
      /// </summary>
      Unexpected = 0x8000ffff,

      /// <summary>
      /// A method call was made when the object was in an incorrect state
      /// for that method.
      /// </summary>
      BadState = 0x80042301,

      /// <summary>
      /// The provider has already been registered.
      /// </summary>
      ProviderAlreadyRegistered = 0x80042303,

      /// <summary>
      /// The specified identifier does not correspond to a registered provider.
      /// </summary>
      ProviderNotRegistered = 0x80042304,

      /// <summary>
      /// The provider was unable to perform the request at this time. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.
      /// </summary>
      ProviderVeto = 0x80042306,

      /// <summary>
      /// The shadow copy provider is currently in use and cannot be unregistered.
      /// </summary>
      ProviderInUse = 0x80042307,

      /// <summary>
      /// The specified object was not found.
      /// </summary>
      ObjectNotFound = 0x80042308,

      /// <summary>
      /// No VSS provider indicates that it supports the specified volume. 
      /// </summary>
      VolumeNotSupported = 0x8004230C,

      /// <summary>
      /// The volume is not supported by the specified provider.
      /// </summary>
      VolumeNotSupportedByProvider = 0x8004230E,

      /// <summary>
      /// The object already exists.
      /// </summary>
      ObjectAlreadyExists = 0x8004230D,

      /// <summary>
      /// The provider returned an unexpected error code. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.
      /// </summary>
      UnexpectedProviderError = 0x8004230F,

      /// <summary>
      /// The given XML document is invalid.  It is either incorrectly-formed XML or it does not match the schema.
      /// </summary>
      InvalidXmlDocument = 0x80042311,

      /// <summary>
      /// The maximum number of volumes has been added to the shadow copy set. The specified volume was not added to the shadow copy set. 
      /// </summary>
      MaximumNumberOfVolumesReached = 0x80042312,

      /// <summary>
      /// VSS encountered problems while sending events to writers.
      /// </summary>
      UnexpectedWriterError = 0x80042315,

      /// <summary>
      /// Another shadow copy creation is already in progress. Please wait a few moments and try again.
      /// </summary>        
      SnapshotSetInProgress = 0x80042316,

      /// <summary>
      /// The volume has been added to the maximum number of shadow copy sets. The specified volume was not added to the shadow copy set.
      /// </summary>
      MaximumNumberOfSnapshotsReached = 0x80042317,

      /// <summary>
      /// An error was detected in the Volume Shadow Copy Service (VSS). The problem occurred while trying to contact VSS writers.
      /// Please verify that the Event System service and the VSS service are running and check for associated errors in the event logs.
      /// </summary>
      WriterInfrastructureError = 0x80042318,

      /// <summary>
      /// A writer did not respond to a GatherWriterStatus call.  The writer may either have terminated
      /// or it may be stuck.  Check the system and application event logs for more information.
      /// </summary>
      WriterNotResponding = 0x80042319,

      /// <summary>
      /// The writer has already sucessfully called the Subscribe function.  It cannot call
      /// subscribe multiple times.
      /// </summary>
      WriterAlreadySubscribed = 0x8004231A,

      /// <summary>
      /// The shadow copy provider does not support the specified shadow copy type.
      /// </summary>
      UnsupportedContext = 0x8004231B,

      /// <summary>
      /// The specified shadow copy storage association is in use and so can't be deleted.
      /// </summary>
      VolumeInUse = 0x8004231D,

      /// <summary>
      /// Maximum number of shadow copy storage associations already reached.
      /// </summary>
      MaximumDiffareaAssociationsReached = 0x8004231E,

      /// <summary>
      /// The system or provider has insufficient storage space. If possible delete any old or unnecessary persistent shadow copies and try again.
      /// </summary>
      InsufficientStorage = 0x8004231F,

      /// <summary>
      /// No shadow copies were successfully imported.
      /// </summary>
      NoSnapshotsImported = 0x80042320,

      /// <summary>
      /// Some shadow copies were not succesfully imported.
      /// </summary>
      SomeSnapshotsNotImportedSuccess = 0x00042321,

      /// <summary>
      /// Some shadow copies were not succesfully imported.
      /// </summary>
      SomeSnapshotsNotImportedError = 0x80042321,

      /// <summary>
      /// The maximum number of remote machines for this operation has been reached.
      /// </summary>
      MaximumNumberOfRemoteMachinesReached = 0x80042322,

      /// <summary>
      /// The remote server is unavailable.
      /// </summary>
      RemoteServerUnavailable = 0x80042323,

      /// <summary>
      /// The remote server is running a version of the Volume Shadow Copy Service that does not
      /// support remote shadow-copy creation.
      /// </summary>
      RemoteServerUnsupported = 0x80042324,

      /// <summary>
      /// A revert is currently in progress for the specified volume.  Another revert
      /// cannot be initiated until the current revert completes.
      /// </summary>
      RevertInProgress = 0x80042325,

      /// <summary>
      /// The volume being reverted was lost during revert.
      /// </summary>
      RevertVolumeLost = 0x80042326,

      /// <summary>
      ///		<para>The provider encountered an error that requires the user to restart the computer.</para>
      ///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
      /// </summary>
      RebootRequired = 0x80042327,

      /// <summary>
      ///     <para>The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).</para>
      ///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
      /// </summary>
      TransactionFreezeTimeout = 0x80042328,

      /// <summary>
      ///     <para>The system was unable to thaw the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).</para>
      ///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
      /// </summary>
      TransactionThawTimeout = 0x80042329,

      /// <summary>
      /// The shadow copy contains only a subset of the volumes needed by the writer to correctly back 
      /// up the application component.
      /// </summary>
      WriterErrorInconsistentSnapshot = 0x800423F0,

      /// <summary>
      /// The writer ran out of memory or other system resources. The recommended way to handle this error code is 
      /// to wait ten minutes and then repeat the operation, up to three times.
      /// </summary>
      WriterOutOfResources = 0x800423F1,

      /// <summary>
      /// The writer operation failed because of a time-out between the Freeze and Thaw events. 
      /// The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.
      /// </summary>
      WriterTimeout = 0x800423F2,

      /// <summary>
      /// The writer failed due to an error that would likely not occur if the entire backup, restore, or shadow copy creation 
      /// process was restarted. The recommended way to handle this error code is to wait ten minutes and then repeat 
      /// the operation, up to three times.
      /// </summary>
      WriterErrorRetryable = 0x800423F3,

      /// <summary>
      /// The writer experienced a non-transient error.  If the backup process is retried,
      /// the error is likely to reoccur.        
      /// </summary>
      WriterErrorNonRetryable = 0x800423F4,

      /// <summary>
      /// The writer experienced an error while trying to recover the shadow-copy volume.
      /// </summary>
      WriterErrorRecoveryFailed = 0x800423F5,

      /// <summary>
      /// The shadow copy set break operation failed because the disk/partition identities could not be reverted. 
      /// The target identity already exists on the machine or cluster and must be masked before this operation can succeed.
      /// </summary>
      BreakRevertIdFailed = 0x800423F6,

      /// <summary>
      /// This version of the hardware provider does not support this operation.
      /// </summary>
      LegacyProvider = 0x800423F7,

      /// <summary>
      /// At least one of the providers in this Shadow Copy Set failed the break operation for a snapshot.
      /// </summary>
      BreakFailFromProvider = 0x800423F8,

      /// <summary>
      /// There are too few disks on this computer or one or more of the disks is too small. 
      /// Add or change disks so they match the disks in the backup, and try the restore again.
      /// </summary>
      AsrDiskAssignmentFailed = 0x80042401,

      /// <summary>
      /// Windows cannot create a disk on this computer needed to restore from the backup. 
      /// Make sure the disks are properly connected, or add or change disks, and try the restore again.
      /// </summary>
      AsrDiskRecreationFailed = 0x80042402,

      /// <summary>
      /// The computer needs to be restarted to finish preparing a hard disk for restore. To continue, restart your computer and run the restore again.
      /// </summary>
      AsrNoArcPath = 0x80042403,

      /// <summary>
      /// The backup failed due to a missing disk for a dynamic volume. Please ensure the disk is online and retry the backup.
      /// </summary>
      AsrMissingDynamicDisk = 0x80042404,

      /// <summary>
      /// Automated System Recovery failed the shadow copy, because a selected critical volume is located on a cluster shared disk. 
      /// This is an unsupported configuration.
      /// </summary>
      AsrSharedCriticalDiskError = 0x80042405,

      /// <summary>
      /// A data disk is currently set as active in BIOS. Set some other disk as active or use the DiskPart utility to clean the 
      /// data disk, and then retry the restore operation.
      /// </summary>
      AsrDatadiskRdisk0 = 0x80042406,

      /// <summary>
      /// The disk that is set as active in BIOS is too small to recover the original system disk. 
      /// Replace the disk with a larger one and retry the restore operation.
      /// </summary>
      AsrRdisk0TooSmall = 0x80042407,

      /// <summary>
      /// There is not enough disk space on the system to perform the restore operation. 
      /// Add another disk or replace one of the existing disks and retry the restore operation.
      /// </summary>
      AsrCriticalDisksTooSmall = 0x80042408,

      /// <summary>
      /// 	<para>
      /// 		The writer status is not available for one or more writers. A writer may have reached the maximum number of available backup 
      /// 		and restore sessions.
      /// 	</para>
      /// 	<para>
      /// 		<b>Windows Vista, Windows Server 2003, and Windows XP:</b> This value is not supported.
      /// 	</para>
      /// </summary>
      WriterStatusNotAvailable = 0x80042409,



      /// <summary>
      /// The system was unable to flush I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times. 
      /// </summary>
      FlushWritesTimeout = 0x80042313,

      /// <summary>
      /// The system was unable to hold I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.
      /// </summary>
      HoldWritesTimeout = 0x80042314,
   }
}
