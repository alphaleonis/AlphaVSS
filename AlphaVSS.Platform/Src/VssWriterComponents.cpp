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

#include "VssWriterComponents.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssWriterComponents^ VssWriterComponents::Adopt(IVssWriterComponentsExt *vssWriterComponents)
	{
		try
		{
			return gcnew VssWriterComponents(vssWriterComponents);
		}
		catch (...)
		{
			vssWriterComponents->Release();
			throw;
		}
	}

	VssWriterComponents::VssWriterComponents(IVssWriterComponentsExt *vssWriterComponents)
		: mVssWriterComponents(vssWriterComponents), m_components(nullptr)
	{
		m_components = gcnew ComponentList(this);
		VSS_ID iid, wid;
		CheckCom(vssWriterComponents->GetWriterInfo(&iid, &wid));
		m_instanceId = ToGuid(iid);
		m_writerId = ToGuid(wid);
	}

	VssWriterComponents::~VssWriterComponents()
	{
		this->!VssWriterComponents();
	}

	VssWriterComponents::!VssWriterComponents()
	{
		if (mVssWriterComponents != 0)
		{
			mVssWriterComponents->Release();
			mVssWriterComponents = 0;
		}
	}

	Guid VssWriterComponents::InstanceId::get()
	{
		return m_instanceId;
	}

	Guid VssWriterComponents::WriterId::get()
	{
		return m_writerId;
	}

	VssWriterComponents::ComponentList::ComponentList(VssWriterComponents^ writerComponents)
		: mWriterComponents(writerComponents)
	{
	}

	int VssWriterComponents::ComponentList::Count::get()
	{
		if (mWriterComponents->mVssWriterComponents == 0)
			throw gcnew ObjectDisposedException("Instance of IVssListAdapter must not be used after the object from which it was obtained has been disposed.");

		UINT cComponents;
		CheckCom(mWriterComponents->mVssWriterComponents->GetComponentCount(&cComponents));
		return cComponents;
	}

	IVssComponent^ VssWriterComponents::ComponentList::default::get(int index)
	{
		if (mWriterComponents->mVssWriterComponents == 0)
			throw gcnew ObjectDisposedException("Instance of IVssListAdapter must not be used after the object from which it was obtained has been disposed.");

		::IVssComponent *component;
		CheckCom(mWriterComponents->mVssWriterComponents->GetComponent(index, &component));
		return VssComponent::Adopt(component);
	}

	IList<IVssComponent^>^ VssWriterComponents::Components::get()
	{
		return m_components;
	}


}
} }