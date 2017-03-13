
#pragma once

#include <vss.h>
#include "VssComponent.h"
#include "VssListAdapter.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	private ref class VssWriterComponents : IDisposable, IVssWriterComponents, MarshalByRefObject
	{
	public:
		~VssWriterComponents();
		!VssWriterComponents();

		property IList<IVssComponent^>^ Components { virtual IList<IVssComponent^>^ get(); }
		property Guid InstanceId { virtual Guid get(); }
		property Guid WriterId { virtual Guid get(); }

	internal:
		static VssWriterComponents^ Adopt(IVssWriterComponentsExt *vssWriterComponents);
	private:
		VssWriterComponents(IVssWriterComponentsExt *vssWriterComponents);
		IVssWriterComponentsExt *mVssWriterComponents;

		ref class ComponentList sealed : VssListAdapter<IVssComponent^>
		{
		public:
			ComponentList(VssWriterComponents^ component);

			property int Count { virtual int get() override; }
			property IVssComponent^ default[int] { virtual IVssComponent^ get(int index) override; }
		private:
			VssWriterComponents^ mWriterComponents;
		};

		ComponentList^ m_components;
		Guid m_writerId;
		Guid m_instanceId;
	};
}
} }