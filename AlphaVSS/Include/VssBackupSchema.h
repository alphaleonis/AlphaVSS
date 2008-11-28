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

#if NTDDI_VERSION < NTDDI_WS03
// Dummy values when enumeration is not supported
#define VSS_BS_UNDEFINED							 0x01
#define VSS_BS_DIFFERENTIAL							 0x02
#define VSS_BS_INCREMENTAL							 0x04
#define VSS_BS_EXCLUSIVE_INCREMENTAL_DIFFERENTIAL	 0x08
#define VSS_BS_LOG									 0x10
#define VSS_BS_COPY									 0x20
#define VSS_BS_TIMESTAMPED							 0x40
#define VSS_BS_LAST_MODIFY							 0x80
#define VSS_BS_LSN									 0x100
#define VSS_BS_WRITER_SUPPORTS_NEW_TARGET			 0x200
#define VSS_BS_WRITER_SUPPORTS_RESTORE_WITH_MOVE	 0x400
#endif

#if NTDDI_VERSION < NTDDI_LONGHORN
#define VSS_BS_INDEPENDENT_SYSTEM_STATE				 0x800
#define VSS_BS_ROLLFORWARD_RESTORE					 0x1000
#define VSS_BS_RESTORE_RENAME						 0x2000
#define VSS_BS_AUTHORITATIVE_RESTORE				 0x4000
#define VSS_BS_WRITER_SUPPORTS_PARALLEL_RESTORES	 0x8000
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>The <see cref="VssBackupSchema"/> enumeration is used by a writer to indicate the types of backup operations it can participate in. 
	/// The supported kinds of backup are expressed as a bit mask (or bitwise OR) of <see cref="VssBackupSchema"/> values.</summary>
	/// <remarks><b>Windows XP:</b> This enumeration is not available until Windows Server 2003 or later.</remarks>
	[Flags]
	public enum class VssBackupSchema
	{
		/// <summary>
		/// The writer supports a simple full backup and restoration of entire files (as defined by a <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> value of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Full"/>). 
		/// This backup scheme can be used as the basis of an incremental or differential backup. This is the default value.
		/// </summary>
		Undefined = VSS_BS_UNDEFINED,
		/// <summary>
		/// The writer supports differential backups (corresponding to the <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> value <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Differential"/>). 
		/// Files created or changed since the last full backup are saved. Files are not marked as having been backed up. This setting does not preclude mixing of incremental and differential backups.
		/// </summary>
		Differential = VSS_BS_DIFFERENTIAL,
		/// <summary>
		/// The writer supports incremental backups (corresponding to the <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> value <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Incremental"/>). Files created or changed since the last full or incremental backup are saved. Files are marked as having been backed up. This setting does not preclude mixing of incremental and differential backups.
		/// </summary>
		Incremental = VSS_BS_INCREMENTAL,
		/// <summary>
		/// The writer supports both differential and incremental backup schemas, but only exclusively: for example, you cannot follow a differential backup with an incremental one. 
		/// A writer cannot support this schema if it does not support both incremental and differential schemas (<see cref="Differential" /> | <see cref="Incremental"/>).
		/// </summary>
		ExclusiveIncrementalDifferential = VSS_BS_EXCLUSIVE_INCREMENTAL_DIFFERENTIAL,
		/// <summary>
		/// The writer supports backups that involve only the log files it manages (corresponding to a <see dref="T:Alphaleonis.Win32.Vss.VssBackupType"/> value of <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Log"/>). 
		/// </summary>
		Log = VSS_BS_LOG,
		/// <summary>
		/// Similar to the default backup schema (<see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Undefined"/>), the writer supports copy backup operations 
		/// (corresponding to <see dref="F:Alphaleonis.Win32.Vss.VssBackupType.Copy"/>) where file access information (such as information as to when a file was 
		/// last backed up) will not be updated either in the writer's own state information or in the file system information. This type of 
		/// backup cannot be used as the basis of an incremental or differential backup.
		/// </summary>
		Copy = VSS_BS_COPY,
		Timestamped = VSS_BS_TIMESTAMPED,
		LastModify = VSS_BS_LAST_MODIFY,
		/// <summary>
		/// Reserved for system use.
		/// </summary>
		Lsn = VSS_BS_LSN,
		WriterSupportsNewTarget = VSS_BS_WRITER_SUPPORTS_NEW_TARGET,
		WriterSupportsRestoreWithMove = VSS_BS_WRITER_SUPPORTS_RESTORE_WITH_MOVE,
		IndependentSystemState = VSS_BS_INDEPENDENT_SYSTEM_STATE,
		RollforwardRestore = VSS_BS_ROLLFORWARD_RESTORE,
		RestoreRename = VSS_BS_RESTORE_RENAME,
		AuthorativeRestore = VSS_BS_AUTHORITATIVE_RESTORE,
		WriterSupportsParallelRestores = VSS_BS_WRITER_SUPPORTS_PARALLEL_RESTORES
	};
}
} }

