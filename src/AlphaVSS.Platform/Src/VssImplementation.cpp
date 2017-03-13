

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