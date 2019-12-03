#pragma once

namespace Alphaleonis {
   namespace Win32 {
      namespace Vss
      {
         public ref class VssInformationProvider : IVssInfoProvider, MarshalByRefObject
         {
         public:
            // Inherited via IVssInformationProvider
            virtual bool IsVolumeSnapshotted(System::String^ volumeName);
            virtual VssSnapshotCompatibility GetSnapshotCompatibility(String^ volumeName);
            virtual bool ShouldBlockRevert(System::String^ volumeName);
         };
      }
   }
}