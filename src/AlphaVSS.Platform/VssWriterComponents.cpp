
#include "pch.h"

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