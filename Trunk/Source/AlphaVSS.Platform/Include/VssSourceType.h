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

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>The <see cref="VssSourceType"/> enumeration specifies the type of data that a writer manages.</summary>
	public enum class VssSourceType
	{
		/// <summary><para>The source of the data is not known.</para><para>This indicates a writer error, and the requester should report it.</para></summary>
		Undefined = VSS_ST_UNDEFINED,
		
		/// <summary>The source of the data is a database that supports transactions, such as Microsoft SQL Server.</summary>
		TransactedDB = VSS_ST_TRANSACTEDDB,
		
		/// <summary>The source of the data is a database that does not support transactions.</summary>
		NonTransactedDB = VSS_ST_NONTRANSACTEDDB,

		/// <summary>
		///     <para>Unclassified source type—data will be in a file group.</para>
		///		<para>This is the default source type.</para>
		/// </summary>
		Other = VSS_ST_OTHER

	};
}
} }