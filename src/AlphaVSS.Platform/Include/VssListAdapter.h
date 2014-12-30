/* Copyright (c) 2008-2012 Peter Palotas
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

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	generic<typename T> 
	private ref class VssListAdapter abstract : System::Collections::Generic::IList<T>, MarshalByRefObject
	{
	public:
		virtual void Add(T item) sealed;
		virtual void Clear();
		virtual bool Contains(T item);
		virtual void CopyTo(array<T>^ arr, int arrayIndex);
		virtual System::Collections::IEnumerator^ GetEnumeratorNG() = System::Collections::IList::GetEnumerator;
		virtual System::Collections::Generic::IEnumerator<T>^ GetEnumerator();
		virtual int IndexOf(T item);
		virtual void Insert(int index, T item);
		virtual bool Remove(T item);
		virtual void RemoveAt(int index);

		property int Count 
		{ 
			virtual int get() abstract; 
		}
		
		property bool IsReadOnly 
		{ 
			virtual bool get(); 
		}
		
		property T default[int] 
		{
			virtual T get (int index) abstract;
			virtual void set (int index, T value);
		};

	protected:
		ref class Enumerator sealed : System::Collections::Generic::IEnumerator<T>
		{
		public:
			Enumerator(VssListAdapter<T>^ list);
			~Enumerator();
			!Enumerator();

			virtual bool MoveNext();
			virtual void Reset();

			property Object^ CurrentObject
			{
				virtual Object^ get() = System::Collections::IEnumerator::Current::get;
			}

			property T Current 
			{
				virtual T get(); 
			}
		private:
			VssListAdapter<T>^ m_list;
			int m_index;
		};
	};
} } }
