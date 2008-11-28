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
#define VSS_WS_FAILED_AT_BACKUPSHUTDOWN VSS_WS_COUNT
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>The <see cref="VssWriterState"/> enumeration indicates the current state of the writer.</summary>
	/// <remarks>A requester determines the state of a writer through <see dref="P:Alphaleonis.Win32.Vss.VssBackupComponents.WriterStatus"/>.</remarks>
	public enum class VssWriterState
	{
		/// <summary><para>The writer's state is not known.</para><para>This indicates an error on the part of the writer.</para></summary>
		Unknown = VSS_WS_UNKNOWN,
		/// <summary>The writer has completed processing current shadow copy events and is ready to proceed, or <c>CVssWriter::OnPrepareSnapshot</c> has not yet been called.</summary>
		Stable = VSS_WS_STABLE,
		/// <summary>The writer is waiting for the freeze state.</summary>
		WaitingForFreeze = VSS_WS_WAITING_FOR_FREEZE,
		/// <summary>The writer is waiting for the thaw state.</summary>
		WaitingForThaw = VSS_WS_WAITING_FOR_THAW,
		/// <summary>The writer is waiting for the <c>PostSnapshot</c> state.</summary>
		WaitingForPostSnapshot = VSS_WS_WAITING_FOR_POST_SNAPSHOT,
		/// <summary>The writer is waiting for the requester to finish its backup operation.</summary>
		WaitingForBackupComplete = VSS_WS_WAITING_FOR_BACKUP_COMPLETE,
		/// <summary>The writer vetoed the shadow copy creation process at the writer identification state.</summary>
		FailedAtIdentify = VSS_WS_FAILED_AT_IDENTIFY,
		/// <summary>The writer vetoed the shadow copy creation process during the backup preparation state.</summary>
		FailedAtPrepareBackup = VSS_WS_FAILED_AT_PREPARE_BACKUP,
		/// <summary>The writer vetoed the shadow copy creation process during the <c>PrepareForSnapshot</c> state.</summary>
		FailedAtPrepareSnapshot = VSS_WS_FAILED_AT_PREPARE_SNAPSHOT,
		/// <summary>The writer vetoed the shadow copy creation process during the freeze state.</summary>
		FailedAtFreeze = VSS_WS_FAILED_AT_FREEZE,
		/// <summary>The writer vetoed the shadow copy creation process during the thaw state.</summary>
		FailedAtThaw = VSS_WS_FAILED_AT_THAW,
		/// <summary>The writer vetoed the shadow copy creation process during the <c>PostSnapshot</c> state.</summary>
		FailedAtPostSnapshot = VSS_WS_FAILED_AT_POST_SNAPSHOT,
		/// <summary>The shadow copy has been created and the writer failed during the <c>BackupComplete</c> state. 
		/// A writer should save information about this failure to the error log.</summary>
		FailedAtBackupComplete = VSS_WS_FAILED_AT_BACKUP_COMPLETE,
		/// <summary>The writer failed during the <c>PreRestore</c> state.</summary>
		FailedAtPreRestore = VSS_WS_FAILED_AT_PRE_RESTORE,
		/// <summary>The writer failed during the <c>PostRestore</c> state.</summary>
		FailedAtPostRestore = VSS_WS_FAILED_AT_POST_RESTORE,
		/// <summary>The writer failed during the shutdown of the backup application.</summary>
		FailedAtBackupShutdown = VSS_WS_FAILED_AT_BACKUPSHUTDOWN,
		/// <summary>Reserved value.</summary>
		Count = VSS_WS_COUNT

	};
}
} }