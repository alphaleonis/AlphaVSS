
#pragma once

#include "Config.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   /// <summary>
   ///		Represents a platform specific implementation of the global methods in the VSS API.
   /// </summary>
   /// <remarks>
   ///		An instance of this class can either be created directly, or by using the factory methods
   ///		in <see cref="VssUtils"/> to obtain an instance of the <see cref="IVssImplementation"/> 
   ///     interface for the current platform.
   /// </remarks>
   public ref class VssImplementation : IVssImplementation, MarshalByRefObject 
   {
   public:
      VssImplementation();
      virtual IVssBackupComponents^ CreateVssBackupComponents();
      virtual bool IsVolumeSnapshotted(String^ volumeName);
      virtual VssSnapshotCompatibility GetSnapshotCompatibility(String^ volumeName);
      virtual bool ShouldBlockRevert(String^ volumeName);
      [System::Security::Permissions::SecurityPermission(System::Security::Permissions::SecurityAction::LinkDemand)]
      virtual IVssExamineWriterMetadata^ CreateVssExamineWriterMetadata(String^ xml);
      virtual IVssSnapshotManagement^ GetSnapshotManagementInterface();
   };

}
}}