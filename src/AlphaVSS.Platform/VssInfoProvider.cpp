

#include "pch.h"

#include "VssInfoProvider.h"
#include "VssBackupComponents.h"
#include "VssSnapshotManagement.h"

namespace Alphaleonis {
	namespace Win32 {
		namespace Vss
		{			
			bool VssInformationProvider::IsVolumeSnapshotted(String^ volumeName)
			{
				LONG lSnapshotCapability = 0;
				BOOL bSnapshotsPresent = 0;
				CheckCom(::IsVolumeSnapshotted((VSS_PWSZ)((const wchar_t*)NoNullAutoMStr(volumeName)), &bSnapshotsPresent, &lSnapshotCapability));
				return bSnapshotsPresent != 0;
			}

			VssSnapshotCompatibility VssInformationProvider::GetSnapshotCompatibility(String^ volumeName)
			{
				LONG lSnapshotCapability = 0;
				BOOL bSnapshotsPresent = 0;
				CheckCom(::IsVolumeSnapshotted((VSS_PWSZ)((const wchar_t*)NoNullAutoMStr(volumeName)), &bSnapshotsPresent, &lSnapshotCapability));
				if (!bSnapshotsPresent)
					throw gcnew InvalidOperationException("No snapshot exists for the specified volume");
				return (VssSnapshotCompatibility)lSnapshotCapability;
			}

			bool VssInformationProvider::ShouldBlockRevert(String^ volumeName)
			{
				// According to MSDN this method is supported also on Windows 2003, however it is not 
				// present in the header-files or library files, except for the library files for ws03 
				// in the vshadow sample directory in the VSSSDK72. Requiring WS08 here.
				bool bBlock = 0;
				CheckCom(::ShouldBlockRevert(NoNullAutoMStr(volumeName), &bBlock));
				return bBlock != 0;
			}
		}
	}
}