/* Copyright (c) 2008-2011 Peter Palotas
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

      property IList<VssWMFileDescription^>^ AlternateLocationMappings { virtual IList<VssWMFileDescription^>^ get(); }

      property VssWMRestoreMethod^ RestoreMethod { virtual VssWMRestoreMethod^ get(); }
      property IList<IVssWMComponent^>^ Components { virtual IList<IVssWMComponent^>^ get(); }

      property IList<VssWMFileDescription^>^ ExcludeFiles { virtual IList<VssWMFileDescription^>^ get(); }

      property Guid InstanceId { virtual Guid get(); }

      property Guid WriterId { virtual Guid get(); }

      property String^ WriterName { virtual String^ get(); }

      property VssUsageType Usage { virtual VssUsageType get(); }

      property VssSourceType Source { virtual VssSourceType get(); }
      property String^ InstanceName { virtual String^ get(); }
      property System::Version^ Version { virtual System::Version^ get(); }
      property IList<VssWMFileDescription^>^ ExcludeFromSnapshotFiles { virtual IList<VssWMFileDescription^>^ get(); }
   internal:
      [SecurityPermission(SecurityAction::LinkDemand)]
      static IVssExamineWriterMetadata^ Adopt(::IVssExamineWriterMetadata *ewm);
   private:
      VssExamineWriterMetadata(::IVssExamineWriterMetadata *examineWriterMetadata);
      ::IVssExamineWriterMetadata *mExamineWriterMetadata;

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      DEFINE_EX_INTERFACE_ACCESSOR(IVssExamineWriterMetadataEx, mExamineWriterMetadata);
#endif

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      DEFINE_EX_INTERFACE_ACCESSOR(IVssExamineWriterMetadataEx2, mExamineWriterMetadata);
#endif

      void Initialize();

      Guid m_instanceId;
      Guid m_writerId;
      String^ m_writerName;
      String^ m_instanceName;
      VssUsageType m_usage;
      VssSourceType m_source;
      IList<VssWMFileDescription^> ^m_excludeFiles;
      IList<IVssWMComponent^> ^m_components;
      IList<VssWMFileDescription^>^ m_excludeFilesFromSnapshot;
      VssWMRestoreMethod^ m_restoreMethod;
      IList<VssWMFileDescription^>^ m_alternateLocationMappings;
      System::Version^ m_version;
   };
}
} }