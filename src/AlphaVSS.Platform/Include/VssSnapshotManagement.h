
#pragma once

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003

#include <VsMgmt.h>

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   public ref class VssSnapshotManagement : IVssSnapshotManagement, MarshalByRefObject
   {
   public:
      ~VssSnapshotManagement();
      !VssSnapshotManagement();

      virtual IVssDifferentialSoftwareSnapshotManagement^ GetDifferentialSoftwareSnapshotManagementInterface();
      virtual Int64 GetMinDiffAreaSize();
   internal:
      VssSnapshotManagement();
   private:
      ::IVssSnapshotMgmt *m_snapshotMgmt;

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      DEFINE_EX_INTERFACE_ACCESSOR(IVssSnapshotMgmt2, m_snapshotMgmt)
#endif
   };

}
}}

#endif
