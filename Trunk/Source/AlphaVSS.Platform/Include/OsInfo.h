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

namespace Alphaleonis { namespace Win32 { namespace Vss {

	enum class OsArchitecture
	{
		X86 = PROCESSOR_ARCHITECTURE_INTEL,
		X64 = PROCESSOR_ARCHITECTURE_AMD64,
		IA64 = PROCESSOR_ARCHITECTURE_IA64,
		Unknown = PROCESSOR_ARCHITECTURE_UNKNOWN
	};

	[FlagsAttribute]
	enum class OsVersion
	{
		NOSP = 0x00,
		SP1 = 0x1,
		SP2 = 0x2,
		SP3 = 0x4,
		SP4 = 0x8,

		WinXP =		0x00001000,
		WinXPSP1 =	0x00001001,
		WinXPSP2 =	0x00001002,
		WinXPSP3 =	0x00001004,
		WinXPSP4 =	0x00001008,
			
		Win2003 =		0x00002000,
		Win2003SP1 =	0x00002001,
		Win2003SP2 =	0x00002002,
		Win2003SP3 =	0x00002004,
		Win2003SP4 =	0x00002008,

		WinVista =		0x00004000,
		WinVistaSP1 =	0x00004001,
		WinVistaSP2 =	0x00004002,
		WinVistaSP3 =	0x00004004,
		WinVistaSP4 =	0x00004008,

		Win2008 =		0x00008000,
		Win2008SP1 =	0x00008001,
		Win2008SP2 =	0x00008002,
		Win2008SP3 =	0x00008004,
		Win2008SP4 =	0x00008008,

		Unknown = 0x0
	};

	ref class OsInfo 
	{
	public:
		
		static OsInfo();

		static void RequireSpecific(... array<OsVersion> ^args);
		static void RequireAtLeast(OsVersion version);
		static void RequireAtLeastInFamily(... array<OsVersion> ^args);

		static void ThrowUnsupportedOsException()
		{
			throw gcnew NotSupportedException(L"This operation is not supported on the current operating system.");
		}

		static property OsArchitecture Architecture
		{
			OsArchitecture get();
		}

		static property OsVersion Family
		{
			OsVersion get();
		}

		static property OsVersion ServicePack 
		{
			OsVersion get();
		}

		static property OsVersion Version
		{
			OsVersion get();
		}

	private:
		OsInfo() {}
		static OsVersion GetFamily(OsVersion osVersion);
		static OsVersion GetSp(OsVersion osVersion);
		static OsArchitecture mArchitecture;
		static OsVersion mOperatingSystem;
	};
}}
}