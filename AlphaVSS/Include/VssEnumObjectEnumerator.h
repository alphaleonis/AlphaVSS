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

#include "IVssObjectProp.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	///		Supports a simple iteration over a generic collection of <see cref="IVssObjectProp"/> instances.
	///	</summary>
	/// <remarks>This class in conjunction with <see dref="T:Alphaleonis.Win32.Vss.VssEnumObjectEnumerable"/> provides
	/// a wrapper around the <see href="http://msdn.microsoft.com/en-us/library/aa384132(VS.85).aspx">IVssEnumObject Interface</see>
	/// from the VSS API.</remarks>
	ref class VssEnumObjectEnumerator : IDisposable, System::Collections::Generic::IEnumerator<IVssObjectProp^>
	{
	public:
		~VssEnumObjectEnumerator();
		!VssEnumObjectEnumerator();

		/// <summary>Advances the enumerator to the next element of the collection.</summary>
		/// <returns><c>true</c> if the enumerator was successfully advanced to the next element; <c>false</c> if the enumerator 
		/// has passed the end of the collection.</returns>
		virtual bool MoveNext();

		/// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
		virtual void Reset();

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current element in the collection</returns>
		property Object^ CurrentObject
		{
			virtual Object^ get() sealed = System::Collections::IEnumerator::Current::get;
		}

		/// <summary>Gets the current element in the collection.</summary>
		/// <returns>The current element in the collection</returns>
		property IVssObjectProp^ Current
		{
			virtual IVssObjectProp^ get();
		}
	internal:
		static VssEnumObjectEnumerator^ Adopt(IVssEnumObject *pEnumObject);
	private:
		VssEnumObjectEnumerator(IVssEnumObject *pEnumObject);
		IVssEnumObject *mEnumObject;
		IVssObjectProp^ mCurrent;
	};
}
} }