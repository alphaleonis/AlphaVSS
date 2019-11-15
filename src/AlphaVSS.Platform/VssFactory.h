
#pragma once

namespace Alphaleonis {
   namespace Win32 {
      namespace Vss
      {
         /// <summary>
         ///		Represents a platform specific implementation of the global methods in the VSS API.
         /// </summary>
         /// <remarks>
         ///		An instance of this class can either be created directly, or by using the factory methods
         ///		in <see cref="VssUtils"/> to obtain an instance of the <see cref="IVssImplementation"/> 
         ///     interface for the current platform.
         /// </remarks>
         public ref class VssFactory : IVssFactory, MarshalByRefObject
         {
         public:
            VssFactory();
            virtual IVssBackupComponents^ CreateVssBackupComponents();
            virtual IVssExamineWriterMetadata^ CreateVssExamineWriterMetadata(String^ xml);
            virtual IVssSnapshotManagement^ CreateVssSnapshotManagement();
            virtual IVssInfoProvider^ GetInfoProvider();
         };
      }
   }
}