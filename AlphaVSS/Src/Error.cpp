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
#include "StdAfx.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	void ThrowException(HRESULT errorCode, const wchar_t *message)
	{
		String^ msg;
		if (message == 0)
			msg = String::Empty;
		else
			msg = gcnew String(message);

		switch (errorCode)
		{
		case E_ACCESSDENIED:
			throw gcnew UnauthorizedAccessException();
		case E_INVALIDARG:
			throw gcnew ArgumentException();
		case E_OUTOFMEMORY:
			throw gcnew OutOfMemoryException();
		case E_NOTIMPL:
			throw gcnew NotImplementedException();
		case E_UNEXPECTED:
			throw gcnew SystemException(L"Unexpected system error.");
		case VSS_E_INVALID_XML_DOCUMENT:
			throw gcnew VssInvalidXmlDocumentException();
		case VSS_E_BAD_STATE:
			throw gcnew VssBadStateException(msg);
		case VSS_E_OBJECT_NOT_FOUND:
			throw gcnew VssObjectNotFoundException();
		case VSS_E_PROVIDER_VETO:
			throw gcnew VssProviderVetoException();
		case VSS_E_UNEXPECTED_PROVIDER_ERROR:
			throw gcnew VssUnexpectedProviderError();
		case VSS_E_MAXIMUM_NUMBER_OF_VOLUMES_REACHED:
			throw gcnew VssMaximumNumberOfVolumesReachedException();
		case VSS_E_MAXIMUM_NUMBER_OF_SNAPSHOTS_REACHED:
			throw gcnew VssMaximumNumberOfSnapshotsReachedException();
		case VSS_E_PROVIDER_NOT_REGISTERED:
			throw gcnew VssProviderNotRegisteredException();
		case VSS_E_VOLUME_NOT_SUPPORTED:
			throw gcnew VssVolumeNotSupportedException();
		case VSS_E_VOLUME_NOT_SUPPORTED_BY_PROVIDER:
			throw gcnew VssVolumeNotSupportedByProviderException();
		case VSS_E_UNEXPECTED_WRITER_ERROR:
			throw gcnew VssUnexpectedWriterError();
		case VSS_E_INSUFFICIENT_STORAGE:
			throw gcnew VssInsufficientStorageException();
		case VSS_E_FLUSH_WRITES_TIMEOUT:
			throw gcnew VssFlushWritesTimeoutException();
		case VSS_E_HOLD_WRITES_TIMEOUT:
			throw gcnew VssHoldWritesTimeoutException();
		case VSS_E_OBJECT_ALREADY_EXISTS:
			throw gcnew VssObjectAlreadyExistsException();
#if NTDDI_VERSION >= NTDDI_WS03
// Skip these if not supported
		case VSS_E_REBOOT_REQUIRED:
			throw gcnew VssRebootRequiredException();
		case VSS_E_REVERT_IN_PROGRESS:
			throw gcnew VssRevertInProgressException();
#if NTDDI_VERSION >= NTDDI_LONGHORN
		case VSS_E_TRANSACTION_FREEZE_TIMEOUT:
			throw gcnew VssTransactionFreezeTimeoutException();
		case VSS_E_TRANSACTION_THAW_TIMEOUT:
			throw gcnew VssTransactionThawTimeoutException();
#endif
#endif
		case VSS_E_UNSUPPORTED_CONTEXT:
			throw gcnew VssUnsupportedContextException();
		case VSS_E_VOLUME_IN_USE:
			throw gcnew VssVolumeInUseException();
		case VSS_E_SNAPSHOT_SET_IN_PROGRESS:
			throw gcnew VssSnapshotSetInProgressException();
		default:
			System::Runtime::InteropServices::Marshal::ThrowExceptionForHR(errorCode);
		}
	}
}	
} }
