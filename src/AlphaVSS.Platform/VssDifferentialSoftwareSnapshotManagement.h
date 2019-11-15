
#pragma once

#include <vss.h>

#include <VsMgmt.h>
#include "Macros.h"

using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   public ref class VssDifferentialSoftwareSnapshotManagement : IVssDifferentialSoftwareSnapshotManagement, MarshalByRefObject
   {
   public:
      ~VssDifferentialSoftwareSnapshotManagement();
      !VssDifferentialSoftwareSnapshotManagement();

      virtual void AddDiffArea(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace);
      virtual void ChangeDiffAreaMaximumSize(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace);
      virtual IList<VssDiffAreaProperties^>^ QueryDiffAreasForSnapshot(Guid snapshotId);
      virtual IList<VssDiffAreaProperties^>^ QueryDiffAreasForVolume(String^ volumeName);
      virtual IList<VssDiffAreaProperties^>^ QueryDiffAreasOnVolume(String^ volumeName);
      virtual IList<VssDiffVolumeProperties^>^ QueryVolumesSupportedForDiffAreas(String^ originalVolumeName);

      //
      // From IVssDifferentialSoftwareSnapshotMgmt2
      //
      virtual void ChangeDiffAreaMaximumSize(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace, bool isVolatile);

      //
      // From IVssDifferentialSoftwareSnapshotMgmt3
      //
      virtual void ClearVolumeProtectFault(String^ volumeName);
      virtual void DeleteUnusedDiffAreas(String^ diffAreaVolumeName);
      virtual VssVolumeProtectionInfo^ GetVolumeProtectionLevel(String^ volumeName);
      virtual void SetVolumeProtectionLevel(String^ volumeName, VssProtectionLevel protectionLevel);

   internal:
      VssDifferentialSoftwareSnapshotManagement(::IVssDifferentialSoftwareSnapshotMgmt *pMgmt);
   private:
      ::IVssDifferentialSoftwareSnapshotMgmt *m_mgmt;
      DEFINE_EX_INTERFACE_ACCESSOR(IVssDifferentialSoftwareSnapshotMgmt2, m_mgmt);
      DEFINE_EX_INTERFACE_ACCESSOR(IVssDifferentialSoftwareSnapshotMgmt3, m_mgmt);

   };

}
}}