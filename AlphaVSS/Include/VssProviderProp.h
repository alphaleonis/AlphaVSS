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
#include "VssObjectType.h"
#include "IVssObjectProp.h"
#include "VssProviderType.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>The <see cref="VssProviderProp"/> structure specifies shadow copy provider properties.</summary>
	public ref class VssProviderProp :  IVssObjectProp
	{
	public:
		/// <summary>Indicates the type of this property object as <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.Provider"/>.</summary>
		/// <value>The <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.Provider"/> value of the <see cref="VssObjectType"/> enumeration.</value>
		virtual property VssObjectType Type { VssObjectType get(); }

		/// <summary>Identifies the provider who supports shadow copies of this class.</summary>
		property Guid ProviderId { Guid get(); }

		/// <summary>The provider name.</summary>
		property String^ ProviderName { String^ get(); }

		/// <summary>The provider type. See <see cref="VssProviderType"/> for more information.</summary>
		property VssProviderType ProviderType { VssProviderType get(); }

		/// <summary>The provider version in readable format.</summary>
		property String^ ProviderVersion { String^ get(); }

		/// <summary>A <see cref="Guid"/> uniquely identifying the version of a provider.</summary>
		property Guid ProviderVersionId { Guid get(); }

		/// <summary>Class identifier of the component registered in the local machine's COM catalog.</summary>
		property Guid ClassId { Guid get(); }
	internal:
		static VssProviderProp^ Adopt(VSS_PROVIDER_PROP *pProp);
	private:
		VssProviderProp(VSS_PROVIDER_PROP *pProp);

		Guid mProviderId;
		String^ mProviderName;
		VssProviderType mProviderType;
		String^ mProviderVersion;
		Guid mProviderVersionId;
		Guid mClassId;
	};
}
} }