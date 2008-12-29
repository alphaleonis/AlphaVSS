/* Copyright (c) 2008 Peter Palotas
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
#pragma once

#include <vss.h>

#if ALPHAVSS_TARGET < ALPHAVSS_TARGET_WIN2003
// Dummy values when not defined by the vss headers
#define VSS_FSBT_FULL_BACKUP_REQUIRED			1
#define VSS_FSBT_DIFFERENTIAL_BACKUP_REQUIRED	2
#define VSS_FSBT_INCREMENTAL_BACKUP_REQUIRED	4
#define VSS_FSBT_LOG_BACKUP_REQUIRED			8
#define VSS_FSBT_FULL_SNAPSHOT_REQUIRED			0x10
#define VSS_FSBT_DIFFERENTIAL_SNAPSHOT_REQUIRED	0x20
#define VSS_FSBT_INCREMENTAL_SNAPSHOT_REQUIRED	0x40
#define VSS_FSBT_LOG_SNAPSHOT_REQUIRED			0x80
#define VSS_FSBT_ALL_BACKUP_REQUIRED			0x100
#define VSS_FSBT_ALL_SNAPSHOT_REQUIRED			0x200
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
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
	CA_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")
	[System::Flags]
	public enum class VssFileSpecificationBackupType
	{
		/// <summary>
		/// 	<para>
		/// 	    A file set tagged with this value must be involved in all types of backup operations.
		/// 	</para>
		/// 	<para>
		/// 	    A writer tags a file set with this value to indicate to the requester that it expects a copy of the 
		/// 		current version of the file set to be available following the restore of any backup operation 
		/// 		with a <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of 
		/// 		<see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Full"/>.
		/// 	</para>
		/// </summary>
		FullBackupRequired = VSS_FSBT_FULL_BACKUP_REQUIRED,

		/// <summary>
		/// 	A writer tags a file set with this value to indicate to the requester that it 
		/// 	expects a copy of the current version of the file set to be available following 
		/// 	the restore of any backup operation with a <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of 
		/// 		<see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Differential"/>.
		/// </summary>
		DifferentialBackupRequired = VSS_FSBT_DIFFERENTIAL_BACKUP_REQUIRED,

		/// <summary>
		/// 	A writer tags a file set with this value to indicate to the requester that it 
		/// 	expects a copy of the current version of the file set to be available following the 
		/// 	restore of any backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Incremental"/>.
		/// </summary>
		IncrementalBackupRequired = VSS_FSBT_INCREMENTAL_BACKUP_REQUIRED,

		/// <summary>
		/// 	A writer tags a file set with this value to indicate to the requester that it 
		/// 	expects a copy of the current version of the file set to be available following the 
		/// 	restore of any backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Log"/>.
		/// </summary>
		LogBackupRequired = VSS_FSBT_LOG_BACKUP_REQUIRED,

		/// <summary>
		/// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
		/// 	(and never from the original volume) when participating in a backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Full"/>.
		/// </summary>
		FullSnapshotRequired = VSS_FSBT_FULL_SNAPSHOT_REQUIRED,

		/// <summary>
		/// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
		/// 	(and never from the original volume) when participating in a backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Differential"/>.
		/// </summary>
		DifferentialSnapshotRequired = VSS_FSBT_DIFFERENTIAL_SNAPSHOT_REQUIRED,

		/// <summary>
		/// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
		/// 	(and never from the original volume) when participating in a backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Incremental"/>.
		/// </summary>
		IncrementalSnapshotRequired = VSS_FSBT_INCREMENTAL_SNAPSHOT_REQUIRED,

		/// <summary>
		/// 	A file set tagged with this value must be backed up from a shadow copy of a volume 
		/// 	(and never from the original volume) when participating in a backup operation with a 
		/// 	<see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Log"/>.
		/// </summary>
		LogSnapshotRequired = VSS_FSBT_LOG_SNAPSHOT_REQUIRED,

		/// <summary>
		/// 	The default file backup specification type. A file set tagged with this value must always participate in backup and restore operations.
		/// </summary>
		AllBackupRequired = VSS_FSBT_ALL_BACKUP_REQUIRED,

		/// <summary>
		/// 	The shadow copy requirement for backup. A file set tagged with this value must always be backed up 
		/// 	from a shadow copy of a volume (and never from the original volume) when participating in a backup operation.
		/// </summary>
		AllSnapshotRequired = VSS_FSBT_ALL_SNAPSHOT_REQUIRED
	};
}
} }

