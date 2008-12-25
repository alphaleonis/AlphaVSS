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
#include "OsInfo.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	static OsInfo::OsInfo()
	{
		SYSTEM_INFO si;
		OSVERSIONINFOEX osvi;

		ZeroMemory(&si, sizeof(SYSTEM_INFO));
		ZeroMemory(&osvi, sizeof(OSVERSIONINFOEX));

		osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);

		if (!GetVersionEx((OSVERSIONINFO *)&osvi))
		{
			mArchitecture = OsArchitecture::Unknown;
			return;
		}

		GetSystemInfo(&si);

		mArchitecture = (OsArchitecture)si.wProcessorArchitecture;
		
		if (osvi.dwMajorVersion == 6) // Vista or 2008
		{
			if (osvi.wProductType == VER_NT_WORKSTATION)
				mOperatingSystem = OsVersion::WinVista;
			else
				mOperatingSystem = OsVersion::Win2008;
		}
		else if (osvi.dwMajorVersion == 5) // Win2k, Win2k3 or WinXp
		{
			if (osvi.dwMinorVersion == 1) // WinXP
				mOperatingSystem = OsVersion::WinXP;
			else if (osvi.dwMinorVersion == 2)
			{
				if (osvi.wProductType == VER_NT_WORKSTATION)
					mOperatingSystem = OsVersion::WinXP;
				else
					mOperatingSystem = OsVersion::Win2003;
			}
			else
			{
				mOperatingSystem = OsVersion::Unknown;
			}
		}
		else
		{
			mOperatingSystem = OsVersion::Unknown;
		}

		if (osvi.wServicePackMajor > 0)
		{
			int sp = osvi.wServicePackMajor > 4 ? 4 : osvi.wServicePackMajor;

			if (mOperatingSystem != OsVersion::Unknown)
				mOperatingSystem = mOperatingSystem | ((OsVersion)sp);
		}
	}



	void OsInfo::RequireSpecific(... array<OsVersion> ^args)
	{
		if (args->Length == 0)
			return;

		for each (OsVersion version in args)
		{
			if (version == mOperatingSystem)
				return;
		}
		throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	void OsInfo::RequireAtLeast(OsVersion version)
	{
		if (mOperatingSystem < version)
			throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	void OsInfo::RequireAtLeastInFamily(... array<OsVersion> ^args)
	{
		if (args->Length == 0)
			return;

		for each (OsVersion required in args)
		{
			if (Family == GetFamily(required) && ServicePack >= GetSp(required))
			{
				return;
			}
		}
		throw gcnew NotSupportedException(L"Operation not supported on this operating system.");
	}

	OsArchitecture OsInfo::Architecture::get()
	{
		return mArchitecture; 
	}

	OsVersion OsInfo::Family::get()
	{
		return GetFamily(mOperatingSystem); 
	}

	OsVersion OsInfo::ServicePack::get()
	{
		return GetSp(mOperatingSystem); 
	}

	OsVersion OsInfo::Version::get()
	{
		return mOperatingSystem;
	}

	OsVersion OsInfo::GetFamily(OsVersion osVersion)
	{
		return (OsVersion)((int)osVersion & ~(0xFFF));
	}

	OsVersion OsInfo::GetSp(OsVersion osVersion)
	{
		return (OsVersion)((int)osVersion & 0xFFF);
	}


}
}}