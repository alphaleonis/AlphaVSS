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

#include "stdafx.h"

#include "VssImplementation.h"
#include "VssBackupComponents.h"
#include "VssSnapshotManagement.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssImplementation::VssImplementation()
	{
	}

	IVssBackupComponents^ VssImplementation::CreateVssBackupComponents()
	{
		return gcnew VssBackupComponents();
	}

	bool VssImplementation::IsVolumeSnapshotted(String ^ volumeName)
	{
		LONG lSnapshotCapability = 0;
		BOOL bSnapshotsPresent = 0;
		CheckCom(::IsVolumeSnapshotted((VSS_PWSZ)((const wchar_t *)NoNullAutoMStr(volumeName)), &bSnapshotsPresent, &lSnapshotCapability));	
		return bSnapshotsPresent != 0;
	}

	VssSnapshotCompatibility VssImplementation::GetSnapshotCompatibility(String^ volumeName)
	{
		LONG lSnapshotCapability = 0;
		BOOL bSnapshotsPresent = 0;
		CheckCom(::IsVolumeSnapshotted((VSS_PWSZ)((const wchar_t *)NoNullAutoMStr(volumeName)), &bSnapshotsPresent, &lSnapshotCapability));
		if (!bSnapshotsPresent)
			throw gcnew InvalidOperationException("No snapshot exists for the specified volume");
		return (VssSnapshotCompatibility)lSnapshotCapability;
	}

	bool VssImplementation::ShouldBlockRevert(String ^ volumeName)
	{
		// According to MSDN this method is supported also on Windows 2003, however it is not 
		// present in the header-files or library files, except for the library files for ws03 
		// in the vshadow sample directory in the VSSSDK72. Requiring WS08 here.
#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINVISTAORLATER
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2008);
		bool bBlock = 0;
		CheckCom(::ShouldBlockRevert(NoNullAutoMStr(volumeName), &bBlock));
		return bBlock != 0;
#else
		throw gcnew NotSupportedException(L"This method requires Windows Server 2008.");
#endif
	}

	IVssExamineWriterMetadata^ VssImplementation::CreateVssExamineWriterMetadata(String^ xml)
	{
		::IVssExamineWriterMetadata *pMetadata;
		CheckCom(::CreateVssExamineWriterMetadata(NoNullAutoMBStr(xml), &pMetadata));
		return VssExamineWriterMetadata::Adopt(pMetadata);

	}

	IVssSnapshotManagement^ VssImplementation::GetSnapshotManagementInterface()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		return gcnew VssSnapshotManagement();
#else
		UnsupportedOs();
#endif
	}

}
}}