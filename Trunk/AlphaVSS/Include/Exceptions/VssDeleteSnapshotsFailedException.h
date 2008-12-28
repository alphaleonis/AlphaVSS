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

using namespace System;

#include "Exceptions/VssException.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	/// <summary>
	///		Exception thrown to indicate that the requested deletion of snapshots did not complete successfully.
	/// </summary>	
	/// <remarks>
	///    To get further information about the cause of the error, check the inner exception which is populated with the 
	///    original exception that caused the deletion to fail.
	/// </remarks>
	[Serializable]
	public ref class VssDeleteSnapshotsFailedException : VssException
	{
	public:
		/// <summary>Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class with 
		/// with information describing the error.</summary>
		VssDeleteSnapshotsFailedException(int deletedSnapshotsCount, Guid %nonDeletedSnapshotId, Exception ^innerException);

		/// <summary>
		///    Gets the number of snapshots that were successfully deleted before the operation failed.
		/// </summary>
		property int DeletedSnapshotsCount { int get(); }

		/// <summary>
		///    Gets the id of the snapshot that could not be deleted and caused the abortion of the delete operation.
		/// </summary>
		property Guid NonDeletedSnapshotId { Guid get(); }
	protected:		
		/// <summary>Initializes a new instance of the <see cref="VssDeleteSnapshotsFailedException"/> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination.</param>
		VssDeleteSnapshotsFailedException(System::Runtime::Serialization::SerializationInfo ^ info, System::Runtime::Serialization::StreamingContext context);
	private:		    
		int mDeletedSnapshotsCount;
		Guid mNonDeletedSnapshotId;
	};
}
} }
