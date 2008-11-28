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

#include <StdAfx.h>
#include "VssProviderProp.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssProviderProp^ VssProviderProp::Adopt(VSS_PROVIDER_PROP *pProp)
	{
		try
		{
			return gcnew VssProviderProp(pProp);
		}
		finally
		{
			::CoTaskMemFree(pProp->m_pwszProviderName);
			::CoTaskMemFree(pProp->m_pwszProviderVersion);
		}
	}

	VssProviderProp::VssProviderProp(VSS_PROVIDER_PROP *pProp)
		:	mProviderId(ToGuid(pProp->m_ProviderId)),
			mProviderName(gcnew String(pProp->m_pwszProviderName)),
			mProviderType((VssProviderType)pProp->m_eProviderType),
			mProviderVersion(gcnew String(pProp->m_pwszProviderVersion)),
			mClassId(ToGuid(pProp->m_ClassId))
	{
	}

	VssObjectType VssProviderProp::Type::get()
	{
		return VssObjectType::Provider;
	}

	Guid VssProviderProp::ProviderId::get()
	{
		return mProviderId;
	}

	String^ VssProviderProp::ProviderName::get()
	{
		return mProviderName;
	}

	VssProviderType VssProviderProp::ProviderType::get()
	{
		return mProviderType;
	}

	String^ VssProviderProp::ProviderVersion::get()
	{
		return mProviderVersion;
	}

	Guid VssProviderProp::ProviderVersionId::get()
	{
		return mProviderVersionId;
	}

	Guid VssProviderProp::ClassId::get()
	{
		return mClassId;
	}

}
} }