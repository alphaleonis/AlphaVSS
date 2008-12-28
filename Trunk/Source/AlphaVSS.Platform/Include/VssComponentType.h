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
	/// <summary>
	/// The <see cref="VssComponentType"/> enumeration is used by both the requester and the writer to specify the type of component being used 
	/// with a shadow copy backup operation.
	/// </summary>
	public enum class VssComponentType
	{
		/// <summary><para>Undefined component type.</para>
		/// <para>This value indicates an application error.</para>
		/// </summary>
		Undefined = VSS_CT_UNDEFINED,

		/// <summary>Database component.</summary>
		Database = VSS_CT_DATABASE,

		/// <summary>File group component. This is any component other than a database.</summary>
		FileGroup = VSS_CT_FILEGROUP 
	};
}
} }