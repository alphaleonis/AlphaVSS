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

#include "VssWMDependency.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
	VssWMDependency^ VssWMDependency::Adopt(IVssWMDependency *dependency)
	{
		try
		{ 
			return gcnew VssWMDependency(dependency);
		}
		finally
		{
			dependency->Release();
		}
	}

	VssWMDependency::VssWMDependency(IVssWMDependency *dependency)		
	{
		VSS_ID id;
		CheckCom(dependency->GetWriterId(&id));
		mWriterId = ToGuid(id);

		AutoBStr logicalPath;
		CheckCom(dependency->GetLogicalPath(&logicalPath));
		mLogicalPath = logicalPath;
		
		AutoBStr componentName;
		CheckCom(dependency->GetComponentName(&componentName));
		mComponentName = componentName;
	}
#endif

	VssWMDependency::~VssWMDependency()
	{
	}

	Guid VssWMDependency::WriterId::get()
	{
		return mWriterId;
	}

	String^ VssWMDependency::LogicalPath::get()
	{
		return mLogicalPath;
	}

	String^ VssWMDependency::ComponentName::get()
	{
		return mComponentName;
	}

}
} }

