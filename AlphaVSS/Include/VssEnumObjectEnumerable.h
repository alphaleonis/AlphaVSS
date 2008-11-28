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

#include "VssEnumObjectEnumerator.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	<para>
	/// 		The <see cref="VssEnumObjectEnumerable" /> provides the <c>IEnumerable</c> interface to 
	/// 		the <see href="http://msdn.microsoft.com/en-us/library/aa384132(VS.85).aspx">IVssEnumObject Interface</see> from
	/// 		the original VSS API.
	/// 	</para>
	/// 	<para>
	/// 		It provides an enumeration over <see cref="IVssObjectProp"/> instances, as returned by 
	/// 		<see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.Query" />.
	/// 	</para>
	/// </summary>
	ref class VssEnumObjectEnumerable : IDisposable, System::Collections::Generic::IEnumerable<IVssObjectProp^>
	{
	public:
		~VssEnumObjectEnumerable();
		!VssEnumObjectEnumerable();

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="System::Collections::IEnumerator"/> object that can be used to iterate through the collection.</returns>
		virtual System::Collections::IEnumerator^ GetEnumeratorNG() sealed = System::Collections::IEnumerable::GetEnumerator;

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>An <see cref="System::Collections::Generic::IEnumerator"/> object that can be used to iterate through the collection.</returns>
		virtual System::Collections::Generic::IEnumerator<IVssObjectProp^>^ GetEnumerator();
	internal:
		static VssEnumObjectEnumerable^ Adopt(IVssEnumObject *pEnumObject);
	private:
		VssEnumObjectEnumerable(VssEnumObjectEnumerator ^enumerator);
		VssEnumObjectEnumerator ^mEnumerator;
	};
}
} }