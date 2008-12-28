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
#include "VssComponent.h"
#include "VssListAdapter.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	The <see cref="VssWriterComponents"/> class contains methods used to obtain and modify component information 
	/// 	(in the form of <see cref="VssComponent"/> objects) associated with a given writer but stored in a 
	/// 	requester's Backup Components Document.
	/// </summary>
	public ref class VssWriterComponents : IDisposable
	{
	public:
		/// <summary>Releases any resources aquired by this instance</summary>
		~VssWriterComponents();
		/// <summary>Releases any resources aquired by this instance</summary>
		!VssWriterComponents();

		/// <summary>
		/// 	A read-only collection of <see cref="VssComponent"/> instances to the a given writer's 
		/// 	components explicitly stored in the Backup Components Document. 
		/// </summary>
		/// <value>A read-only collection of <see cref="VssComponent"/> instances to the a given writer's 
		/// 	components explicitly stored in the Backup Components Document. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note>
		/// </value>
		property IVssListAdapter<VssComponent^>^ Components { IVssListAdapter<VssComponent^>^ get(); }

		/// <summary>Identifier of the writer instance responsible for the components.</summary>
		property Guid InstanceId { Guid get(); }

		/// <summary>Identifier of the writer class responsible for the components.</summary>
		property Guid WriterId { Guid get(); }

	internal:
		static VssWriterComponents^ Adopt(IVssWriterComponentsExt *vssWriterComponents);
	private:
		VssWriterComponents(IVssWriterComponentsExt *vssWriterComponents);
		IVssWriterComponentsExt *mVssWriterComponents;

		ref class ComponentList sealed : VssListAdapter<VssComponent^>
		{
		public:
			ComponentList(VssWriterComponents^ component);

			property int Count { virtual int get() override; }
			property VssComponent^ default[int] { virtual VssComponent^ get(int index) override; }
		private:
			VssWriterComponents^ mWriterComponents;
		};

		ComponentList^ mComponents;
		Guid mWriterId;
		Guid mInstanceId;
	};
}
} }