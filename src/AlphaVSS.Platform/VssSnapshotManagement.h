
#pragma once

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
      DEFINE_EX_INTERFACE_ACCESSOR(IVssSnapshotMgmt2, m_snapshotMgmt)
   };

}
}}
