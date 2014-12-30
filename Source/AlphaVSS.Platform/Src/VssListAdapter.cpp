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
#include "StdAfx.h"
#include "VssListAdapter.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	generic<typename T>
	void VssListAdapter<T>::Add(T item)
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");
	}

	generic<typename T>
	void VssListAdapter<T>::Clear()
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");
	}

	generic<typename T>
	bool VssListAdapter<T>::Contains(T item)
	{
		for (int i = 0; i < Count; i++)
			if (this[i]->Equals(item))
				return true;
		return false;
	}

	generic<typename T>
	void VssListAdapter<T>::CopyTo(array<T>^ arr, int arrayIndex)
	{
		if (arr == nullptr)
			throw gcnew ArgumentNullException(L"arr");

		if (arrayIndex < 0)
			throw gcnew ArgumentOutOfRangeException("arrayIndex");

		if (arr->Rank != 1)
			throw gcnew ArgumentException("array must be one-dimensional", "arr");

		if (arrayIndex + Count > arr->Length)
			throw gcnew ArgumentException("invalid arrayIndex");

		for (int i = 0; i < Count; i++)
			arr[i + arrayIndex] = this[i];
	}

	generic<typename T>
	System::Collections::Generic::IEnumerator<T>^ VssListAdapter<T>::GetEnumerator()
	{
		return gcnew Enumerator(this);
	}

	generic<typename T>
	System::Collections::IEnumerator^ VssListAdapter<T>::GetEnumeratorNG()
	{
		return gcnew Enumerator(this);
	}

	generic<typename T>
	int VssListAdapter<T>::IndexOf(T item)
	{
		for (int i = 0; i < Count; i++)
			if (this[i]->Equals(item))
				return i;
		return -1;
	}

	generic<typename T>
	void VssListAdapter<T>::Insert(int index, T item)
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");
	}

	generic<typename T>
	bool VssListAdapter<T>::Remove(T item)
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");
	}
	
	generic<typename T>
	void VssListAdapter<T>::RemoveAt(int index)
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");
	}

	generic<typename T>
	bool VssListAdapter<T>::IsReadOnly::get()
	{
		return true;
	}		

	generic<typename T>
	void VssListAdapter<T>::default::set(int index, T value)
	{
		throw gcnew NotSupportedException(L"Cannot modify read-only list");		
	}		

	generic<typename T>
	VssListAdapter<T>::Enumerator::Enumerator(VssListAdapter<T>^ list)
		: m_list(list), m_index(-1)
	{
	}

	generic<typename T>
	VssListAdapter<T>::Enumerator::~Enumerator()
	{
	}
	
	generic<typename T>
	VssListAdapter<T>::Enumerator::!Enumerator()
	{
	}

	generic<typename T>
	bool VssListAdapter<T>::Enumerator::MoveNext()
	{
		int count = m_list->Count;
		if (++m_index >= count)
		{
			m_index = count;
			return false;
		}
		return true;
	}

	generic<typename T>
	void VssListAdapter<T>::Enumerator::Reset()
	{
		m_index = -1;
	}

	generic<typename T>
	Object^ VssListAdapter<T>::Enumerator::CurrentObject::get()
	{
		return m_list[m_index];
	}

	generic<typename T>
	T VssListAdapter<T>::Enumerator::Current::get()
	{
		return m_list[m_index];
	}


} } }