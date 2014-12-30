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