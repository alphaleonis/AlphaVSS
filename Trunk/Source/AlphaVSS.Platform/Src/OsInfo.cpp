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
#include "stdafx.h"
#include "OSInfo.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	static OSInfo::OSInfo()
	{
		SYSTEM_INFO si;
		OSVERSIONINFOEX osvi;

		ZeroMemory(&si, sizeof(SYSTEM_INFO));
		ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));

		osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);

		if (!GetVersionEx((OSVERSIONINFO *)&osvi))
		{
			mArchitecture = OSArchitecture::Unknown;
			return;
		}

		GetSystemInfo(&si);

		mArchitecture = (OSArchitecture)si.wProcessorArchitecture;
		
		if (osvi.dwMajorVersion == 6) // Vista or 2008
		{
			if (osvi.wProductType == VER_NT_WORKSTATION)
				mOperatingSystem = OSVersions::WinVista;
			else
				mOperatingSystem = OSVersions::Win2008;
		}
		else if (osvi.dwMajorVersion == 5) // Win2k, Win2k3 or WinXp
		{
			if (osvi.dwMinorVersion == 1) // WinXP
				mOperatingSystem = OSVersions::WinXP;
			else if (osvi.dwMinorVersion == 2)
			{
				if (osvi.wProductType == VER_NT_WORKSTATION)
					mOperatingSystem = OSVersions::WinXP;
				else
					mOperatingSystem = OSVersions::Win2003;
			}
			else
			{
				mOperatingSystem = OSVersions::None;
			}
		}
		else
		{
			mOperatingSystem = OSVersions::None;
		}

		if (osvi.wServicePackMajor > 0)
		{
			int sp = osvi.wServicePackMajor > 4 ? 4 : osvi.wServicePackMajor;

			if (mOperatingSystem != OSVersions::None)
				mOperatingSystem = mOperatingSystem | ((OSVersions)sp);
		}
	}



	void OSInfo::RequireSpecific(... array<OSVersions> ^args)
	{
		if (args->Length == 0)
			return;

		for each (OSVersions version in args)
		{
			if (version == mOperatingSystem)
				return;
		}
		throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	void OSInfo::RequireAtLeast(OSVersions version)
	{
		if (mOperatingSystem < version)
			throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	bool OSInfo::IsAtLeast(OSVersions version)
	{
		return mOperatingSystem >= version;
	}

	void OSInfo::RequireAtLeastInFamily(... array<OSVersions> ^args)
	{
		if (args->Length == 0)
			return;

		for each (OSVersions required in args)
		{
			if (Family == GetFamily(required) && ServicePack >= GetSp(required))
			{
				return;
			}
		}
		throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	OSArchitecture OSInfo::Architecture::get()
	{
		return mArchitecture; 
	}

	OSVersions OSInfo::Family::get()
	{
		return GetFamily(mOperatingSystem); 
	}

	OSVersions OSInfo::ServicePack::get()
	{
		return GetSp(mOperatingSystem); 
	}

	OSVersions OSInfo::Version::get()
	{
		return mOperatingSystem;
	}

	OSVersions OSInfo::GetFamily(OSVersions osVersion)
	{
		return (OSVersions)((int)osVersion & ~(0xFFF));
	}

	OSVersions OSInfo::GetSp(OSVersions osVersion)
	{
		return (OSVersions)((int)osVersion & 0xFFF);
	}


}
}}