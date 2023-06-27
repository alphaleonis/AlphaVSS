
using System;
namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// 	<para>
   /// 		The <see cref="VssFileSpecificationBackupType"/> enumeration is used by writers to indicate their support of certain backup 
   /// 		operations—such as incremental or differential backup—on the basis of file sets (a specified file or files).
   /// 	</para>
   /// 	<para>
   /// 		File sets stored in the Writer Metadata Document are tagged with a bit mask (or bitwise OR) of <see cref="VssFileSpecificationBackupType"/>
   /// 		values indicating the following:
   /// 		<list type="bullet">
   /// 			<item>
   /// 				<description>
   /// 					Whether the writer and the requester have to 
   /// 					evaluate a given file set for participation in the specified type of backup operations
   /// 				</description>
   /// 			</item>
   /// 			<item>
   /// 				<description>
   /// 					Whether backing up the specified file will require a shadow copy
   /// 				</description>
   /// 			</item>
   /// 		</list>
   /// 	</para>
   /// </summary>
   /// <remarks>
   /// 	For more information see the MSDN documentation on 
   /// 	<see href="http://msdn.microsoft.com/en-us/library/aa384951(VS.85).aspx">VSS_FILE_SPEC_BACKUP_TYPE Enumeration</see>
   /// </remarks>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue"), Flags]
   public enum VssFileSpecificationBackupType
   {
      /// <summary>
      /// Used on operating systems where this enumeration is not supported, i.e. Windows XP.
      /// </summary>
      Unknown = 0x0,

      /// <summary>
      /// 	<para>
      /// 	    A file set tagged with this value must be involved in all types of backup operations.
      /// 	</para>
      /// 	<para>
      /// 	    A writer tags a file set with this value to indicate to the requester that it expects a copy of the 
      /// 		current version of the file set to be available following the restore of any backup operation 
      /// 		with a <see cref="VssBackupType"/> of 
      /// 		<see cref="VssBackupType.Full"/>.
      /// 	</para>
      /// </summary>
      FullBackupRequired = 0x00000001,

      /// <summary>
      /// 	A writer tags a file set with this value to indicate to the requester that it 
      /// 	expects a copy of the current version of the file set to be available following 
      /// 	the restore of any backup operation with a <see cref="VssBackupType"/> of 
      /// 		<see cref="VssBackupType.Differential"/>.
      /// </summary>
      DifferentialBackupRequired = 0x00000002,

      /// <summary>
      /// 	A writer tags a file set with this value to indicate to the requester that it 
      /// 	expects a copy of the current version of the file set to be available following the 
      /// 	restore of any backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Incremental"/>.
      /// </summary>
      IncrementalBackupRequired = 0x00000004,

      /// <summary>
      /// 	A writer tags a file set with this value to indicate to the requester that it 
      /// 	expects a copy of the current version of the file set to be available following the 
      /// 	restore of any backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Log"/>.
      /// </summary>
      LogBackupRequired = 0x00000008,

      /// <summary>
      /// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
      /// 	(and never from the original volume) when participating in a backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Full"/>.
      /// </summary>
      FullSnapshotRequired = 0x00000100,

      /// <summary>
      /// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
      /// 	(and never from the original volume) when participating in a backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Differential"/>.
      /// </summary>
      DifferentialSnapshotRequired = 0x00000200,

      /// <summary>
      /// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
      /// 	(and never from the original volume) when participating in a backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Incremental"/>.
      /// </summary>
      IncrementalSnapshotRequired = 0x00000400,

      /// <summary>
      /// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
      /// 	(and never from the original volume) when participating in a backup operation with a 
      /// 	<see cref="VssBackupType"/> of <see cref="VssBackupType.Log"/>.
      /// </summary>
      LogSnapshotRequired = 0x00000800,

      /// <summary>
      /// 	A writer tags a file set with this value to indicate to the requester that they expect the file to be created during the snapshot sequence.
      /// </summary>
      CreatedDuringBackup = 0x00010000,

      /// <summary>
      /// 	The default file backup specification type. A file set tagged with this value must always participate in backup and restore operations.
      /// </summary>
      AllBackupRequired = 0x0000000F,

      /// <summary>
      /// 	The shadow copy requirement for backup. A file set tagged with this value must always be backed up 
      /// 	from a shadow copy of a volume (and never from the original volume) when participating in a backup operation.
      /// </summary>
      AllSnapshotRequired = 0x00000F00
   };
}
