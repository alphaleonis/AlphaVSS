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

#include "Exceptions/VssDeleteSnapshotsFailedException.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{		
	VssDeleteSnapshotsFailedException::VssDeleteSnapshotsFailedException(int deletedSnapshotsCount, Guid %nonDeletedSnapshotId, Exception ^innerException)
		: VssException(
		dynamic_cast<VssException^>(innerException) != nullptr ? safe_cast<VssException^>(innerException)->ErrorCode : 0,
		L"Failed to delete all specified snapshots")
	{
		mDeletedSnapshotsCount = deletedSnapshotsCount;
		mNonDeletedSnapshotId = nonDeletedSnapshotId;
	}


	VssDeleteSnapshotsFailedException::VssDeleteSnapshotsFailedException(System::Runtime::Serialization::SerializationInfo ^ info, System::Runtime::Serialization::StreamingContext context)
		: VssException(info, context)
	{
	}

	int VssDeleteSnapshotsFailedException::DeletedSnapshotsCount::get()
	{
		return mDeletedSnapshotsCount;
	}

	Guid VssDeleteSnapshotsFailedException::NonDeletedSnapshotId::get()
	{
		return mNonDeletedSnapshotId;
	}

}
} }
