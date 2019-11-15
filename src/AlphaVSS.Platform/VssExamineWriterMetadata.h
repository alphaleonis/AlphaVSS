
#pragma once

#include <vss.h>
#include "VssWMComponent.h"
#include "Macros.h"

using namespace System::Text;
using namespace System::Collections::Generic;
using namespace System::Security::Permissions;


namespace Alphaleonis { namespace Win32 { namespace Vss
{
   private ref class VssExamineWriterMetadata : IDisposable, IVssExamineWriterMetadata, MarshalByRefObject
   {
   public:
      ~VssExamineWriterMetadata();
      !VssExamineWriterMetadata();

      virtual bool LoadFromXml(String^ xml);
      virtual String^ SaveAsXml();
      property VssBackupSchema BackupSchema { virtual VssBackupSchema get(); }

      property IList<VssWMFileDescriptor^>^ AlternateLocationMappings { virtual IList<VssWMFileDescriptor^>^ get(); }

      property VssWMRestoreMethod^ RestoreMethod { virtual VssWMRestoreMethod^ get(); }
      property IList<IVssWMComponent^>^ Components { virtual IList<IVssWMComponent^>^ get(); }

      property IList<VssWMFileDescriptor^>^ ExcludeFiles { virtual IList<VssWMFileDescriptor^>^ get(); }

      property Guid InstanceId { virtual Guid get(); }

      property Guid WriterId { virtual Guid get(); }

      property String^ WriterName { virtual String^ get(); }

      property VssUsageType Usage { virtual VssUsageType get(); }

      property VssSourceType Source { virtual VssSourceType get(); }
      property String^ InstanceName { virtual String^ get(); }
      property System::Version^ Version { virtual System::Version^ get(); }
      property IList<VssWMFileDescriptor^>^ ExcludeFromSnapshotFiles { virtual IList<VssWMFileDescriptor^>^ get(); }
   internal:
      [SecurityPermission(SecurityAction::LinkDemand)]
      static IVssExamineWriterMetadata^ Adopt(::IVssExamineWriterMetadata *ewm);
   private:
      VssExamineWriterMetadata(::IVssExamineWriterMetadata *examineWriterMetadata);
      ::IVssExamineWriterMetadata *mExamineWriterMetadata;

      DEFINE_EX_INTERFACE_ACCESSOR(IVssExamineWriterMetadataEx, mExamineWriterMetadata);
      DEFINE_EX_INTERFACE_ACCESSOR(IVssExamineWriterMetadataEx2, mExamineWriterMetadata);

      void Initialize();

      Guid m_instanceId;
      Guid m_writerId;
      String^ m_writerName;
      String^ m_instanceName;
      VssUsageType m_usage;
      VssSourceType m_source;
      IList<VssWMFileDescriptor^> ^m_excludeFiles;
      IList<IVssWMComponent^> ^m_components;
      IList<VssWMFileDescriptor^>^ m_excludeFilesFromSnapshot;
      VssWMRestoreMethod^ m_restoreMethod;
      IList<VssWMFileDescriptor^>^ m_alternateLocationMappings;
      System::Version^ m_version;
   };
}
} }