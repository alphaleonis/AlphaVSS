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
#define VSS_E_REBOOT_REQUIRED E_UNEXPECTED
#endif

#if NTDDI_VERSION < NTDDI_LONGHORN
#define VSS_E_WRITER_STATUS_NOT_AVAILABLE E_UNEXPECTED
#define VSS_E_TRANSACTION_FREEZE_TIMEOUT E_UNEXPECTED
#define VSS_E_TRANSACTION_THAW_TIMEOUT E_UNEXPECTED
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	<para>
	/// 		The <see cref="VssError"/> enumeration represents error- and success codes that may be
	/// 		returned by some Vss methods.
	/// 	</para>
	/// </summary>
	public enum class VssError
	{
		/// <summary>Indication of a successful operation.</summary>
		Success = S_OK,
		
		/// <summary>The asynchronous operation was canceled.</summary>
		AsyncCancelled = VSS_S_ASYNC_CANCELLED,
		/// <summary>The asynchronous operation was completed successfully.</summary>
		AsyncFinished = VSS_S_ASYNC_FINISHED,
		/// <summary>The asynchronous operation is still running.</summary>
		AsyncPending = VSS_S_ASYNC_PENDING,

		/// <summary>Unexpected error. The error code is logged in the error log file.</summary>
		Unexpected = E_UNEXPECTED,

		/// <summary>The requested object does not exist.</summary>
		ObjectNotFound = VSS_E_OBJECT_NOT_FOUND,

		/// <summary>The provider was unable to perform the request at this time. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</summary>
		ProviderVeto = VSS_E_PROVIDER_VETO,
		
		// See GetWriterStatus
		/// <summary>The shadow copy contains only a subset of the volumes needed by the writer to correctly back up the application component.</summary>
		InconsistenSnapshot = VSS_E_WRITERERROR_INCONSISTENTSNAPSHOT,

		/// <summary>The writer ran out of memory or other system resources. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		WriterOutOfResources = VSS_E_WRITERERROR_OUTOFRESOURCES,

		/// <summary>The writer operation failed because of a time-out between the Freeze and Thaw events. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		WriterTimeout = VSS_E_WRITERERROR_TIMEOUT,

		/// <summary>The writer failed due to an error that would likely not occur if the entire backup, restore, or shadow copy creation process was restarted. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		WriterErrorRetryable = VSS_E_WRITERERROR_RETRYABLE,

		/// <summary>The writer operation failed because of an error that might recur if another shadow copy is created.</summary>
		WriterErrorNonRetryable = VSS_E_WRITERERROR_NONRETRYABLE,

		/// <summary>The writer is not responding.</summary>
		WriterNotResponding = VSS_E_WRITER_NOT_RESPONDING,

		/// <summary>
		/// 	<para>
		/// 		The writer status is not available for one or more writers. A writer may have reached the maximum number of available backup 
		/// 		and restore sessions.
		/// 	</para>
		/// 	<para>
		/// 		<b>Windows Vista, Windows Server 2003, and Windows XP:</b> This value is not supported.
		/// 	</para>
		/// </summary>
		WriterStatusNotAvailable = VSS_E_WRITER_STATUS_NOT_AVAILABLE,

		/// <summary>The provider returned an unexpected error code. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</summary>
		UnexpectedProviderError = VSS_E_UNEXPECTED_PROVIDER_ERROR,

		// See DoSnapshotSet
		/// <summary>The system or provider has insufficient storage space. If possible delete any old or unnecessary persistent shadow copies and try again.</summary>
		InsufficientStorage = VSS_E_INSUFFICIENT_STORAGE,

		/// <summary>The system was unable to flush I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times. </summary>
		FlushWritesTimeout = VSS_E_FLUSH_WRITES_TIMEOUT,

		/// <summary>The system was unable to hold I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times. </summary>
		HoldWritesTimeout = VSS_E_HOLD_WRITES_TIMEOUT,

		/// <summary>
		///		<para>The provider encountered an error that requires the user to restart the computer.</para>
		///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
		/// </summary>
		RebootRequired = VSS_E_REBOOT_REQUIRED,

		/// <summary>
		///     <para>The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).</para>
		///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
		/// </summary>
		TransactionFreezeTimeout = VSS_E_TRANSACTION_FREEZE_TIMEOUT,

		/// <summary>
		///     <para>The system was unable to thaw the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).</para>
		///		<para><b>Windows Server 2003 and Windows XP:</b>This value is not supported until Windows Vista.</para>
		/// </summary>
		TransactionThawTimeout = VSS_E_TRANSACTION_THAW_TIMEOUT,

		/// <summary>The volume has been added to the maximum number of shadow copy sets. The specified volume was not added to the shadow copy set.</summary>
		MaximumNumberOfSnapshotsReached = VSS_E_MAXIMUM_NUMBER_OF_SNAPSHOTS_REACHED,

		/// <summary>The maximum number of volumes has been added to the shadow copy set. The specified volume was not added to the shadow copy set. </summary>
		MaximumNumberOfVolumesReached = VSS_E_MAXIMUM_NUMBER_OF_VOLUMES_REACHED,

		/// <summary>The specified identifier does not correspond to a registered provider.</summary>
		ProviderNotRegistered = VSS_E_PROVIDER_NOT_REGISTERED,

		/// <summary>No VSS provider indicates that it supports the specified volume. </summary>
		VolumeNotSupported = VSS_E_VOLUME_NOT_SUPPORTED,

		/// <summary>The volume is not supported by the specified provider.</summary>
		VolumeNotSupportedByProvider = VSS_E_VOLUME_NOT_SUPPORTED_BY_PROVIDER,



	};
}
} }