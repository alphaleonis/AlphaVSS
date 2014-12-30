/* Copyright (c) 2008-2012 Peter Palotas
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
#include "StdAfx.h"

#include "VssComponent.h"


namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssComponent^ VssComponent::Adopt(::IVssComponent *vssWriterComponents)
   {
      try
      {
         return gcnew VssComponent(vssWriterComponents);
      }
      catch (...)
      {
         vssWriterComponents->Release();
         throw;
      }
   }

   VssComponent::VssComponent(::IVssComponent *vssComponent)
      : m_vssComponent(vssComponent), 
      m_alternateLocationMappings(nullptr),
      m_directedTargets(nullptr),
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      m_differencedFiles(nullptr),
#endif
      m_restoreSubcomponents(nullptr),
      m_partialFiles(nullptr),
      m_newTargets(nullptr)
   {
      m_alternateLocationMappings = gcnew AlternateLocationMappingList(this);
      m_directedTargets = gcnew DirectedTargetList(this);
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      m_differencedFiles = gcnew DifferencedFileList(this);
#endif
      m_restoreSubcomponents = gcnew RestoreSubcomponentList(this);
      m_partialFiles = gcnew PartialFileList(this);
      m_newTargets = gcnew NewTargetList(this);
   }

   VssComponent::~VssComponent()
   {
      this->!VssComponent();
   }

   VssComponent::!VssComponent()
   {
      if (m_vssComponent != 0)
      {
         m_vssComponent->Release();
         m_vssComponent = 0;
      }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      if (m_IVssComponentEx != 0)
      {
         m_IVssComponentEx->Release();
         m_IVssComponentEx = 0;
      }
#endif
   }

   // TODO: Why is this #if 0'ed???
#if 0
   void VssComponent::AddDifferencedFilesByLastModifyTime(String ^ path, String ^ fileSpec, bool recursive, DateTime lastModifyTime)
   {
      CheckCom(m_vssComponent->AddDifferencedFilesByLastModifyTime(NoNullAutoMStr(path), 
         NoNullAutoMStr(fileSpec), recursive,ToFileTime(lastModifyTime)));
   }

   void VssComponent::AddDifferencedFilesByLastModifyTime(VssDifferencedFileInfo^ differencedFile)
   {
      AddDifferencedFilesByLastModifyTime(differencedFile->Path, differencedFile->FileSpec, differencedFile->IsRecursive, differencedFile->LastModifyTime);
   }

   void VssComponent::AddDirectedTarget(String ^ sourcePath, String^ sourceFileName, String^ sourceRangeList, String^ destinationPath, String^ destinationFileName, String^ destinationRangeList)
   {
      CheckCom(m_vssComponent->AddDirectedTarget(
         NoNullAutoMStr(sourcePath), NoNullAutoMStr(sourceFileName), NoNullAutoMStr(sourceRangeList),
         NoNullAutoMStr(destinationPath), NoNullAutoMStr(destinationFileName), NoNullAutoMStr(destinationRangeList)));
   }

   void VssComponent::AddPartialFile(String^ path, String^ filename, String^ ranges, String^ metaData)
   {
      CheckCom(m_vssComponent->AddPartialFile(NoNullAutoMStr(path), NoNullAutoMStr(filename), NoNullAutoMStr(ranges), AutoMStr(metaData)));
   }

   void VssComponent::AddDirectedTarget(VssDirectedTargetInfo ^directedTarget)
   {
      AddDirectedTarget(directedTarget->SourcePath, directedTarget->SourceFileName, directedTarget->SourceRangeList, 
         directedTarget->DestinationPath, directedTarget->DestinationFileName, directedTarget->DestinationRangeList);
   }

   void VssComponent::AddParitalFile(VssPartialFileInfo^ partialFile)
   {
      AddPartialFile(partialFile->Path, partialFile->FileName, partialFile->Range, partialFile->Metadata);
   }

   String^ VssComponent::BackupMetadata::get()
   {
      AutoBStr bstrBackupMetadata;
      CheckCom(m_vssComponent->GetBackupMetadata(&bstrBackupMetadata));
      return bstrBackupMetadata;
   }

   String^ VssComponent::RestoreMetadata::get()
   {
      AutoBStr s;
      CheckCom(m_vssComponent->GetRestoreMetadata(&s));
      return s;
   }

      void VssComponent::SetBackupMetadata(String^ metadata)
   {
      CheckCom(m_vssComponent->SetBackupMetadata(NoNullAutoMStr(metadata)));
   }

   void VssComponent::SetBackupStamp(String^ stamp)
   {
      CheckCom(m_vssComponent->SetBackupStamp(NoNullAutoMStr(stamp)));
   }


   void VssComponent::SetPostRestoreFailureMsg(String^ msg)
   {
      CheckCom(m_vssComponent->SetPostRestoreFailureMsg(NoNullAutoMStr(msg)));
   }


   void VssComponent::SetPreRestoreFailureMsg(String^ msg)
   {
      CheckCom(m_vssComponent->SetPreRestoreFailureMsg(NoNullAutoMStr(msg)));
   }


   void VssComponent::SetRestoreMetadata(String^ metadata)
   {
      CheckCom(m_vssComponent->SetRestoreMetadata(NoNullAutoMStr(metadata)));
   }


   void VssComponent::SetRestoreTarget(VssRestoreTarget target)
   {
      CheckCom(m_vssComponent->SetRestoreTarget((VSS_RESTORE_TARGET)target));
   }

#endif

   bool VssComponent::AdditionalRestores::get()
   {
      bool bAdditionalRestores;
      CheckCom(m_vssComponent->GetAdditionalRestores(&bAdditionalRestores));
      return bAdditionalRestores;
   }

   String^ VssComponent::BackupOptions::get()
   {
      AutoBStr str;
      CheckCom(m_vssComponent->GetBackupOptions(&str));
      return str;
   }

   String^ VssComponent::BackupStamp::get()
   {
      AutoBStr str;
      CheckCom(m_vssComponent->GetBackupStamp(&str));
      return str;
   }

   bool VssComponent::BackupSucceeded::get()
   {
      bool b;
      CheckCom(m_vssComponent->GetBackupSucceeded(&b));
      return b;
   }

   String^ VssComponent::ComponentName::get()
   {
      AutoBStr str;
      CheckCom(m_vssComponent->GetComponentName(&str));
      return str;
   }

   VssComponentType VssComponent::ComponentType::get()
   {
      VSS_COMPONENT_TYPE type;
      CheckCom(m_vssComponent->GetComponentType(&type));
      return (VssComponentType)type;
   }

   VssFileRestoreStatus VssComponent::FileRestoreStatus::get()
   {
      VSS_FILE_RESTORE_STATUS status;
      CheckCom(m_vssComponent->GetFileRestoreStatus(&status));
      return (VssFileRestoreStatus)status;
   }

   String^ VssComponent::LogicalPath::get()
   {
      AutoBStr path;
      CheckCom(m_vssComponent->GetLogicalPath(&path));
      return path;
   }

   String^ VssComponent::PostRestoreFailureMsg::get()
   {
      AutoBStr s;
      CheckCom(m_vssComponent->GetPostRestoreFailureMsg(&s));
      return s;
   }

   String^ VssComponent::PreRestoreFailureMsg::get()
   {
      AutoBStr s;
      CheckCom(m_vssComponent->GetPreRestoreFailureMsg(&s));
      return s;
   }

   String^ VssComponent::PreviousBackupStamp::get()
   {
      AutoBStr s;
      CheckCom(m_vssComponent->GetPreviousBackupStamp(&s));
      return s;
   }

   String^ VssComponent::RestoreOptions::get()
   {
      AutoBStr s;
      CheckCom(m_vssComponent->GetRestoreOptions(&s));
      return s;
   }

   VssRestoreTarget VssComponent::RestoreTarget::get()
   {
      VSS_RESTORE_TARGET tgt;
      CheckCom(m_vssComponent->GetRestoreTarget(&tgt));
      return (VssRestoreTarget)tgt;
   }

   bool VssComponent::IsSelectedForRestore::get()
   {
      bool b;
      CheckCom(m_vssComponent->IsSelectedForRestore(&b));
      return b;
   }


   IList<VssDirectedTargetInfo^>^ VssComponent::DirectedTargets::get()
   {
      return m_directedTargets;
   }

   IList<VssPartialFileInfo^>^ VssComponent::PartialFiles::get()
   {
      return m_partialFiles;
   }

   IList<VssWMFileDescriptor^>^ VssComponent::NewTargets::get()
   {
      return m_newTargets;
   }

   IList<VssDifferencedFileInfo^>^ VssComponent::DifferencedFiles::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      return m_differencedFiles;
#else
      throw gcnew UnsupportedOperatingSystemException();
#endif
   }


   IList<VssRestoreSubcomponentInfo^>^ VssComponent::RestoreSubcomponents::get()
   {
      return m_restoreSubcomponents;
   }

   IList<VssWMFileDescriptor^>^ VssComponent::AlternateLocationMappings::get()
   {
      return m_alternateLocationMappings;
   }


   //
   // DirectedTargetList
   //
   VssComponent::DirectedTargetList::DirectedTargetList(VssComponent^ component)
      : m_component(component)
   {
   }

   int VssComponent::DirectedTargetList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetDirectedTargetCount(&count));
      return count;
   }

   VssDirectedTargetInfo^ VssComponent::DirectedTargetList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      AutoBStr bsSourcePath, bsSourceFileName, bsSourceRangeList;
      AutoBStr bsDestPath, bsDestFileName, bsDestRangeList;

      CheckCom(m_component->m_vssComponent->GetDirectedTarget(index, &bsSourcePath, &bsSourceFileName, &bsSourceRangeList, &bsDestPath, 
         &bsDestFileName, &bsDestRangeList));
      
      return gcnew VssDirectedTargetInfo(bsSourcePath, bsSourceFileName, bsSourceRangeList, bsDestPath, bsDestFileName, bsDestRangeList);
   }



   //
   // NewTargetList 
   //
   int VssComponent::NewTargetList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetNewTargetCount(&count));
      return count;
   }

   
   VssWMFileDescriptor^ VssComponent::NewTargetList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      IVssWMFiledesc *vssWMFiledesc;
      CheckCom(m_component->m_vssComponent->GetNewTarget(index, &vssWMFiledesc));
      return CreateVssWMFileDescriptor(vssWMFiledesc);
   }

   VssComponent::NewTargetList::NewTargetList(VssComponent^ component)
      : m_component(component)
   {
   }

   //
   // PartialFileList 
   //
   int VssComponent::PartialFileList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetPartialFileCount(&count));
      return count;
   }

   
   VssPartialFileInfo^ VssComponent::PartialFileList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      AutoBStr bsPath, bsFileName, bsRange, bsMetadata;
      CheckCom(m_component->m_vssComponent->GetPartialFile(index, &bsPath, &bsFileName, &bsRange, &bsMetadata));
      return gcnew VssPartialFileInfo(bsPath, bsFileName, bsRange, bsMetadata);
   }

   VssComponent::PartialFileList::PartialFileList(VssComponent^ component)
      : m_component(component)
   {
   }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
   //
   // DifferencedFileList 
   //
   int VssComponent::DifferencedFileList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetDifferencedFilesCount(&count));
      return count;
   }

   VssDifferencedFileInfo^ VssComponent::DifferencedFileList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      AutoBStr bstrPath, bstrFilespec, bstrLsnString;
      BOOL bRecursive;
      FILETIME ftLastModifyTime;
      CheckCom(m_component->m_vssComponent->GetDifferencedFile(index, &bstrPath, &bstrFilespec, &bRecursive, &bstrLsnString, &ftLastModifyTime));
      return gcnew VssDifferencedFileInfo(bstrPath, bstrFilespec, bRecursive != 0, ToDateTime(ftLastModifyTime));
   }

   VssComponent::DifferencedFileList::DifferencedFileList(VssComponent^ component)
      : m_component(component)
   {
   }
#endif


   //
   // RestoreSubcomponentList 
   //
   int VssComponent::RestoreSubcomponentList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetRestoreSubcomponentCount(&count));
      return count;
   }

   VssRestoreSubcomponentInfo^ VssComponent::RestoreSubcomponentList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      AutoBStr bsLogicalPath, bsComponentName;
      bool bRepair;
      CheckCom(m_component->m_vssComponent->GetRestoreSubcomponent(index, &bsLogicalPath, &bsComponentName, &bRepair));
      return gcnew VssRestoreSubcomponentInfo(bsLogicalPath, bsComponentName);
   }

   VssComponent::RestoreSubcomponentList::RestoreSubcomponentList(VssComponent^ component)
      : m_component(component)
   {
   }


   //
   // AlternateLocationMappingList 
   //
   int VssComponent::AlternateLocationMappingList::Count::get()
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT count;
      CheckCom(m_component->m_vssComponent->GetAlternateLocationMappingCount(&count));
      return count;
   }

   
   VssWMFileDescriptor^ VssComponent::AlternateLocationMappingList::default::get(int index)
   {
      if (m_component->m_vssComponent == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      if (index < 0 || index >= Count)
         throw gcnew ArgumentOutOfRangeException("index");

      IVssWMFiledesc *vssWMFiledesc;
      CheckCom(m_component->m_vssComponent->GetAlternateLocationMapping(index, &vssWMFiledesc));
      return CreateVssWMFileDescriptor(vssWMFiledesc);
   }

   VssComponent::AlternateLocationMappingList::AlternateLocationMappingList(VssComponent^ component)
      : m_component(component)
   {
   }

   bool VssComponent::IsAuthoritativeRestore::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      bool bAuth;
      if (SUCCEEDED(RequireIVssComponentEx()->GetAuthoritativeRestore(&bAuth)))
         return bAuth;
#endif
      return false;
   }

    String^ VssComponent::PostSnapshotFailureMsg::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      AutoBStr bstrMsg;
      if (SUCCEEDED(RequireIVssComponentEx()->GetPostSnapshotFailureMsg(&bstrMsg)))
         return bstrMsg;
#endif
      return nullptr;
   }

    String^ VssComponent::PrepareForBackupFailureMsg::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      AutoBStr bstrMsg;
      if (SUCCEEDED(RequireIVssComponentEx()->GetPrepareForBackupFailureMsg(&bstrMsg)))
         return bstrMsg;
#endif
      return nullptr;
   }

    String^ VssComponent::RestoreName::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      AutoBStr bstrName;
      if (SUCCEEDED(RequireIVssComponentEx()->GetRestoreName(&bstrName)))
         return bstrName;
#endif
      return nullptr;
   }

   String^ VssComponent::RollForwardRestorePoint::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      VSS_ROLLFORWARD_TYPE eRollType;
      AutoBStr bstrPoint;

      if (SUCCEEDED(RequireIVssComponentEx()->GetRollForward(&eRollType, &bstrPoint)))
         return bstrPoint;
#endif
      return nullptr;
   }

   VssRollForwardType VssComponent::RollForwardType::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      VSS_ROLLFORWARD_TYPE eRollType;
      AutoBStr bstrPoint;

      if (SUCCEEDED(RequireIVssComponentEx()->GetRollForward(&eRollType, &bstrPoint)))
         return (VssRollForwardType)eRollType;
#endif
      return VssRollForwardType::Undefined;
   }

   VssComponentFailure^ VssComponent::Failure::get()
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      HRESULT hr;
      HRESULT hrApplication;
      AutoBStr bstrApplicationMessage;
      DWORD dwReserved;

      CheckCom(RequireIVssComponentEx2()->GetFailure(&hr, &hrApplication, &bstrApplicationMessage, &dwReserved));

      return gcnew VssComponentFailure(hr, hrApplication, bstrApplicationMessage);
#endif
      return nullptr;
   }

   void VssComponent::Failure::set(VssComponentFailure^ failure)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      if (failure == nullptr)
         return;

      CheckCom(RequireIVssComponentEx2()->SetFailure(failure->ErrorCode, failure->ApplicationErrorCode, AutoMBStr(failure->ApplicationMessage), 0));      
#endif
   }
}
} }
