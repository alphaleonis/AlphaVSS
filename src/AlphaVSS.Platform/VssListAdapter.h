
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
