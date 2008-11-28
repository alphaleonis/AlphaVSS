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
#include "StdAfx.h"


#include "VssEnumObjectEnumerator.h"
#include "VssSnapshotProp.h"
#include "VssProviderProp.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	
	VssEnumObjectEnumerator^ VssEnumObjectEnumerator::Adopt(IVssEnumObject *pEnumObject)
	{
		try
		{
			return gcnew VssEnumObjectEnumerator(pEnumObject);
		}
		catch (...)
		{
			pEnumObject->Release();
			throw;
		}
	}

	
	VssEnumObjectEnumerator::VssEnumObjectEnumerator(IVssEnumObject *pEnumObject)
		: mEnumObject(pEnumObject), mCurrent(nullptr)
	{
	}

	
	VssEnumObjectEnumerator::~VssEnumObjectEnumerator()
	{
		this->!VssEnumObjectEnumerator();
	}

	
	VssEnumObjectEnumerator::!VssEnumObjectEnumerator()
	{
		if (mEnumObject != 0)
		{
			mEnumObject->Release();
			mEnumObject = 0;
		}
	}

	
	bool VssEnumObjectEnumerator::MoveNext()
	{
		VSS_OBJECT_PROP rgelt;
		ULONG celtFetched = 0;

		if (mCurrent != nullptr)
		{
			delete mCurrent;
			mCurrent = nullptr;
		}

		CheckCom(mEnumObject->Next(1, &rgelt, &celtFetched));
		
		if (celtFetched == 0)
			return false;

		switch (rgelt.Type)
		{		
		case VSS_OBJECT_SNAPSHOT:
			mCurrent = VssSnapshotProp::Adopt(&rgelt.Obj.Snap);
			break;
		case VSS_OBJECT_PROVIDER:
			mCurrent = VssProviderProp::Adopt(&rgelt.Obj.Prov);
			break;
		default:
			throw gcnew VssException(String::Format(System::Globalization::CultureInfo::InvariantCulture, "Unsupported object for enumeration ({0})", (VssObjectType)rgelt.Type));
		};

		return true;
	}

	
	void VssEnumObjectEnumerator::Reset()
	{
		CheckCom(mEnumObject->Reset());
	}


	
	Object^ VssEnumObjectEnumerator::CurrentObject::get()
	{
		return mCurrent;
	}

	
	IVssObjectProp^ VssEnumObjectEnumerator::Current::get()
	{
		return mCurrent;
	}

}
} }
