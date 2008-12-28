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
	///		Exception indicating that the creation of a shadow copy is in progress, and only one shadow copy creation 
	///		operation can be in progress at one time. Either wait to try again or return with a failure error code.
	/// </summary>
	[Serializable]
	public ref class VssSnapshotSetInProgressException sealed : VssException 
	{
	public:
		/// <summary>Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with a system-supplied message that describes the error.</summary>
		VssSnapshotSetInProgressException();
		/// <summary>Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with a specified message that describes the error.</summary>
		/// <param name="message">The message that describes the exception.</param>
		VssSnapshotSetInProgressException(String ^ message);
		/// <summary>Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with a specified message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The message that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not <see langword="null"/>, the current exception is raised in a catch block that handles the inner exception.</param>
		VssSnapshotSetInProgressException(String ^ message, Exception ^ innerException);
	protected:		
		/// <summary>Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with serialized data.</summary>
		/// <param name="info">The <see cref="System::Runtime::Serialization::SerializationInfo"/> that holds the serialized object data. </param>
		/// <param name="context">The <see cref="System::Runtime::Serialization::StreamingContext"/> that contains contextual information about the source or destination.</param>
		VssSnapshotSetInProgressException(System::Runtime::Serialization::SerializationInfo ^ info, System::Runtime::Serialization::StreamingContext context);
	};
}
} }
