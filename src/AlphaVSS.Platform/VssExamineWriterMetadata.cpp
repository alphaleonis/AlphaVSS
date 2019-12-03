
#include "pch.h"

#include "VssExamineWriterMetadata.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   IVssExamineWriterMetadata^ VssExamineWriterMetadata::Adopt(::IVssExamineWriterMetadata *ewm)
   {
      try
      {
         return gcnew VssExamineWriterMetadata(ewm);
      }
      catch (...)
      {
         ewm->Release();
         throw;
      }
   }

   VssExamineWriterMetadata::VssExamineWriterMetadata(::IVssExamineWriterMetadata *examineWriterMetadata)
      : mExamineWriterMetadata(examineWriterMetadata)
   {
      Initialize();
   }


   void VssExamineWriterMetadata::Initialize()
   {
      VSS_ID idInstance, idWriter;
      AutoBStr bsWriterName, bsInstanceName;
      VSS_USAGE_TYPE usage;
      VSS_SOURCE_TYPE source;

      bool hasExIdentity = false;

      IVssExamineWriterMetadataEx *ex = GetIVssExamineWriterMetadataEx();
      if (ex != 0)
      {
         CheckCom(ex->GetIdentityEx(&idInstance, &idWriter, &bsWriterName, &bsInstanceName, &usage, &source));			
         hasExIdentity = true;
      }

      if (!hasExIdentity)
         CheckCom(mExamineWriterMetadata->GetIdentity(&idInstance, &idWriter, &bsWriterName, &usage, &source));

      m_instanceId = ToGuid(idInstance);
      m_writerId = ToGuid(idWriter);
      m_writerName = bsWriterName;
      m_instanceName = bsInstanceName;
      m_usage = (VssUsageType)usage;
      m_source = (VssSourceType)source;

      m_excludeFiles = nullptr;
      m_components = nullptr;
      m_restoreMethod = nullptr;
      m_alternateLocationMappings = nullptr;
      m_version = nullptr;
      m_excludeFilesFromSnapshot = nullptr;
   }

   VssExamineWriterMetadata::~VssExamineWriterMetadata()
   {
      this->!VssExamineWriterMetadata();
   }

   VssExamineWriterMetadata::!VssExamineWriterMetadata()
   {
      if (mExamineWriterMetadata != 0)
      {
         mExamineWriterMetadata->Release();
         mExamineWriterMetadata = 0;
      }

      if (m_IVssExamineWriterMetadataEx != 0)
      {
         m_IVssExamineWriterMetadataEx->Release();
         m_IVssExamineWriterMetadataEx = 0;
      }

      if (m_IVssExamineWriterMetadataEx2 != 0)
      {
         m_IVssExamineWriterMetadataEx2->Release();
         m_IVssExamineWriterMetadataEx2 = 0;
      }
   }

   bool VssExamineWriterMetadata::LoadFromXml(String^ xml)
   {
      HRESULT hr = mExamineWriterMetadata->LoadFromXML(NoNullAutoMBStr(xml));
      if (FAILED(hr))
         ThrowException(hr);

      if (hr == S_FALSE)
         return false;

      // Since cached data may have been modified, we reset everything.
      Initialize();

      return true;
   }

   String^ VssExamineWriterMetadata::SaveAsXml()
   {
      AutoBStr xml;
      CheckCom(mExamineWriterMetadata->SaveAsXML(&xml));
      return xml;
   }

   Guid VssExamineWriterMetadata::InstanceId::get()
   {
      return m_instanceId;
   }

   Guid VssExamineWriterMetadata::WriterId::get()
   {
      return m_writerId;
   }

   String^ VssExamineWriterMetadata::WriterName::get()
   {
      return m_writerName;
   }

   VssUsageType VssExamineWriterMetadata::Usage::get()
   {
      return m_usage;
   }

   VssSourceType VssExamineWriterMetadata::Source::get()
   {
      return m_source;
   }

   IList<VssWMFileDescriptor^>^ VssExamineWriterMetadata::ExcludeFiles::get()
   {
      if (m_excludeFiles != nullptr)
         return m_excludeFiles;

      UINT cIncludeFiles, cExcludeFiles, cComponents;
      CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

      IList<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(cExcludeFiles);
      for (UINT i = 0; i < cExcludeFiles; i++)
      {
         IVssWMFiledesc *filedesc;
         CheckCom(mExamineWriterMetadata->GetExcludeFile(i, &filedesc));
         list->Add(CreateVssWMFileDescriptor(filedesc));
      }
      m_excludeFiles = list;
      return m_excludeFiles;
   }

   IList<IVssWMComponent^>^ VssExamineWriterMetadata::Components::get()
   {
      if (m_components != nullptr)
         return m_components;

      UINT cIncludeFiles, cExcludeFiles, cComponents;
      CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

      IList<IVssWMComponent^>^ list = gcnew List<IVssWMComponent^>(cComponents);
      for (UINT i = 0; i < cComponents; i++)
      {
         ::IVssWMComponent *component;
         CheckCom(mExamineWriterMetadata->GetComponent(i, &component));
         list->Add(VssWMComponent::Adopt(component));
      }
      m_components = list;
      return m_components;
   }

   VssWMRestoreMethod^ VssExamineWriterMetadata::RestoreMethod::get()
   {
      if (m_restoreMethod != nullptr)
         return m_restoreMethod;

      VSS_RESTOREMETHOD_ENUM eMethod;
      AutoBStr bstrService, bstrUserProcedure;
      VSS_WRITERRESTORE_ENUM eWriterRestore;
      bool bRebootRequired;
      UINT iMappings;
      HRESULT result = mExamineWriterMetadata->GetRestoreMethod(&eMethod, &bstrService, &bstrUserProcedure, &eWriterRestore, &bRebootRequired, &iMappings);

      if (FAILED(result))
         ThrowException(result);

      if (result == S_FALSE)
         return nullptr;

      m_restoreMethod = gcnew VssWMRestoreMethod((VssRestoreMethod)eMethod, bstrService, bstrUserProcedure, (VssWriterRestore)eWriterRestore, bRebootRequired, iMappings);
      return m_restoreMethod;

   }

   IList<VssWMFileDescriptor^>^ VssExamineWriterMetadata::AlternateLocationMappings::get()
   {
      if (m_alternateLocationMappings != nullptr)
         return m_alternateLocationMappings;

      // Return an empty list if no restore method is available
      if (this->RestoreMethod == nullptr)
         return (gcnew List<VssWMFileDescriptor^>(0))->AsReadOnly();

      IList<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(this->RestoreMethod->MappingCount);

      for (int i = 0; i < this->RestoreMethod->MappingCount; i++)
      {
         IVssWMFiledesc *filedesc;
         CheckCom(mExamineWriterMetadata->GetAlternateLocationMapping(i, &filedesc));
         list->Add(CreateVssWMFileDescriptor(filedesc));
      }
      m_alternateLocationMappings = list;
      return m_alternateLocationMappings;
   }

   VssBackupSchema VssExamineWriterMetadata::BackupSchema::get()
   {
      DWORD schema;
      CheckCom(mExamineWriterMetadata->GetBackupSchema(&schema));
      return (VssBackupSchema)schema;
   }

   String^ VssExamineWriterMetadata::InstanceName::get()
   {
      return m_instanceName;
   }

   Version^ VssExamineWriterMetadata::Version::get()
   {
      if (m_version == nullptr)
      {
         DWORD dwMajorVersion = 0;
         DWORD dwMinorVersion = 0;
         CheckCom(RequireIVssExamineWriterMetadataEx2()->GetVersion(&dwMajorVersion, &dwMinorVersion));
      }
      return m_version;
   }

   IList<VssWMFileDescriptor^>^ VssExamineWriterMetadata::ExcludeFromSnapshotFiles::get()
   {
      if (m_excludeFilesFromSnapshot != nullptr)
         return m_excludeFilesFromSnapshot;

      UINT cExcludedFromSnapshot;

      CheckCom(RequireIVssExamineWriterMetadataEx2()->GetExcludeFromSnapshotCount(&cExcludedFromSnapshot));

      IList<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(cExcludedFromSnapshot);
      for (UINT i = 0; i < cExcludedFromSnapshot; i++)
      {
         IVssWMFiledesc *filedesc;
         CheckCom(RequireIVssExamineWriterMetadataEx2()->GetExcludeFromSnapshotFile(i, &filedesc));
         list->Add(CreateVssWMFileDescriptor(filedesc));
      }
      m_excludeFiles = list;

      return m_excludeFiles;
   }

}
} }

