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
#include "VssError.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>Enumeration representing the status of a writer.</summary>
	public enum class VssWriterFailure
	{
		/// <summary>Indication of a successful operation.</summary>
		Ok = S_OK,

		/// <summary>The shadow copy contains only a subset of the volumes needed by the writer to correctly back up the application component.</summary>
		InconsistenSnapshot = VSS_E_WRITERERROR_INCONSISTENTSNAPSHOT,

		/// <summary>The writer ran out of memory or other system resources. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		OutOfResources = VSS_E_WRITERERROR_OUTOFRESOURCES,

		/// <summary>The writer operation failed because of a time-out between the Freeze and Thaw events. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		Timeout = VSS_E_WRITERERROR_TIMEOUT,

		/// <summary>The writer failed due to an error that would likely not occur if the entire backup, restore, or shadow copy creation process was restarted. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</summary>
		Retryable = VSS_E_WRITERERROR_RETRYABLE,

		/// <summary>The writer operation failed because of an error that might recur if another shadow copy is created.</summary>
		NonRetryable = VSS_E_WRITERERROR_NONRETRYABLE,

		/// <summary>The writer is not responding.</summary>
		NotResponding = VSS_E_WRITER_NOT_RESPONDING,

		/// <summary>
		/// 	<para>
		/// 		The writer status is not available for one or more writers. A writer may have reached the maximum number of available backup 
		/// 		and restore sessions.
		/// 	</para>
		/// 	<para>
		/// 		<b>Windows Vista, Windows Server 2003, and Windows XP:</b> This value is not supported.
		/// 	</para>
		/// </summary>
		StatusNotAvailable = VSS_E_WRITER_STATUS_NOT_AVAILABLE
	};
}
} }

