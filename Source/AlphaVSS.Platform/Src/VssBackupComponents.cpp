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

#include "VsBackup.h"
#include "VssBackupComponents.h"
#include "VssAsyncResult.h"

#include "Utils.h"
#include "Macros.h"

using namespace System::Collections::Generic;
using namespace System::Security::Permissions;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssBackupComponents::VssBackupComponents()
      : m_backup(0), 
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      m_IVssBackupComponentsEx(0),
#endif
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      m_IVssBackupComponentsEx2(0),
#endif
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
        m_IVssBackupComponentsEx3(0),
#endif
      m_writerMetadata(nullptr),
      m_writerComponents(nullptr),
      m_writerStatus(nullptr)

   {
      m_writerMetadata = gcnew WriterMetadataList(this);
      m_writerComponents = gcnew WriterComponentsList(this);
      m_writerStatus = gcnew WriterStatusList(this);

      pin_ptr<::IVssBackupComponents *> pVssObject = &m_backup;
      CheckCom( CreateVssBackupComponents(pVssObject) );
   }

   VssBackupComponents::~VssBackupComponents()
   {
      this->!VssBackupComponents();
   }

   VssBackupComponents::!VssBackupComponents()
   {
      if (m_backup != 0)
      {
         m_backup->Release();
         m_backup = 0;
      }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      if (m_IVssBackupComponentsEx != 0)
      {
         m_IVssBackupComponentsEx->Release();
         m_IVssBackupComponentsEx = 0;
      }
#endif

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      if (m_IVssBackupComponentsEx2 != 0)
      {
         m_IVssBackupComponentsEx2->Release();
         m_IVssBackupComponentsEx2 = 0;
      }
#endif

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
        if (m_IVssBackupComponentsEx3 != 0)
        {
            m_IVssBackupComponentsEx3->Release();
            m_IVssBackupComponentsEx3 = 0;
        }
#endif
   }


   void VssBackupComponents::AbortBackup()
   {
      CheckCom(m_backup->AbortBackup());
   }

   void VssBackupComponents::AddAlternativeLocationMapping(Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName, String ^ path, String ^ filespec, bool recursive, String ^ destination)
   {
      CheckCom(m_backup->AddAlternativeLocationMapping(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, 
         AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(path),
         NoNullAutoMStr(filespec), recursive, NoNullAutoMStr(destination)));
   }

   void VssBackupComponents::AddComponent(Guid instanceId, Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName)
   {
      CheckCom(m_backup->AddComponent(ToVssId(instanceId), ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, 
         AutoMStr(logicalPath), NoNullAutoMStr(componentName)));
   }

   void VssBackupComponents::AddNewTarget(Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName, String ^ path, String ^ fileName, bool recursive, String ^ alternatePath)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
      CheckCom(m_backup->AddNewTarget(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType,
         AutoMStr(logicalPath), NoNullAutoMStr(componentName),
         NoNullAutoMStr(path), NoNullAutoMStr(fileName),
         recursive, NoNullAutoMStr(alternatePath)));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::AddRestoreSubcomponent(Guid writerId, VssComponentType componentType, String^ logicalPath, String ^componentName, String^ subcomponentLogicalPath, String^ subcomponentName)
   {
      CheckCom(m_backup->AddRestoreSubcomponent(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType,
         AutoMStr(logicalPath),
         NoNullAutoMStr(componentName),
         NoNullAutoMStr(subcomponentLogicalPath),
         NoNullAutoMStr(subcomponentName),
         false));
   }

   Guid VssBackupComponents::AddToSnapshotSet(String ^ volumeName, Guid providerId)
   {
      VSS_ID idSnapshot;
      CheckCom(m_backup->AddToSnapshotSet(NoNullAutoMStr(volumeName), ToVssId(providerId), &idSnapshot));
      return ToGuid(idSnapshot);
   }

   Guid VssBackupComponents::AddToSnapshotSet(String^ volumeName)
   {
      VSS_ID idSnapshot;
      CheckCom(m_backup->AddToSnapshotSet(NoNullAutoMStr(volumeName), ToVssId(Guid::Empty), &idSnapshot));
      return ToGuid(idSnapshot);
   }

   [SecurityPermissionAttribute(SecurityAction::LinkDemand)]
   void VssBackupComponents::BackupComplete()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->BackupComplete(&pAsync));      
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
   }

   IVssAsyncResult^ VssBackupComponents::BeginBackupComplete(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->BackupComplete(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndBackupComplete(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::BreakSnapshotSet(Guid snapshotSetId)
   {
      CheckCom(m_backup->BreakSnapshotSet(ToVssId(snapshotSetId)));
   }

   void VssBackupComponents::BreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      ::IVssAsync *pAsync;
      CheckCom(RequireIVssBackupComponentsEx2()->BreakSnapshotSetEx(ToVssId(snapshotSetId), (_VSS_HARDWARE_OPTIONS)breakFlags, &pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
#else
      UnsupportedOs();
#endif
   }   

   IVssAsyncResult^ VssBackupComponents::BeginBreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags, AsyncCallback^ userCallback, Object^ stateObject)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      ::IVssAsync *pAsync;
      CheckCom(RequireIVssBackupComponentsEx2()->BreakSnapshotSetEx(ToVssId(snapshotSetId), (_VSS_HARDWARE_OPTIONS)breakFlags, &pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::EndBreakSnapshotSet(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::DeleteSnapshot(Guid snapshotId, bool forceDelete)
   {
      LONG lDeletedSnapshots;
      VSS_ID nonDeletedSnapshotID;
      CheckCom(m_backup->DeleteSnapshots(ToVssId(snapshotId), VSS_OBJECT_SNAPSHOT, forceDelete, &lDeletedSnapshots, &nonDeletedSnapshotID));
   }

   int VssBackupComponents::DeleteSnapshotSet(Guid snapshotSetId, bool forceDelete)
   {
      LONG lDeletedSnapshots;
      VSS_ID nonDeletedSnapshotID;
      HRESULT hr = m_backup->DeleteSnapshots(ToVssId(snapshotSetId), VSS_OBJECT_SNAPSHOT_SET, forceDelete, &lDeletedSnapshots, &nonDeletedSnapshotID);
      
      if (FAILED(hr))
      {
         throw gcnew VssDeleteSnapshotsFailedException(lDeletedSnapshots, ToGuid(nonDeletedSnapshotID), GetExceptionForHr(hr));
      }

      return lDeletedSnapshots;
   }

   void VssBackupComponents::DisableWriterClasses(array<Guid> ^ writerClassIds)
   {
      CheckCom(m_backup->DisableWriterClasses(VssIds(writerClassIds), writerClassIds->Length));
   }

   void VssBackupComponents::DisableWriterInstances(array<Guid> ^ writerInstanceIds)
   {
      CheckCom(m_backup->DisableWriterInstances(VssIds(writerInstanceIds), writerInstanceIds->Length));
   }

   void VssBackupComponents::DoSnapshotSet()
   {
      ::IVssAsync *vssAsync;
      CheckCom(m_backup->DoSnapshotSet(&vssAsync));
      WaitCheckAndReleaseVssAsyncOperation(vssAsync);
   }
   

   IVssAsyncResult^ VssBackupComponents::BeginDoSnapshotSet(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->DoSnapshotSet(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndDoSnapshotSet(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::EnableWriterClasses(array<Guid> ^ writerClassIds)
   {
      CheckCom(m_backup->EnableWriterClasses(VssIds(writerClassIds), writerClassIds->Length));
   }

   String^ VssBackupComponents::ExposeSnapshot(Guid snapshotId, String ^ pathFromRoot, VssVolumeSnapshotAttributes attributes, String ^ expose)
   {
      AutoPwsz pwszExposed;

      CheckCom(m_backup->ExposeSnapshot(ToVssId(snapshotId), AutoMStr(pathFromRoot), (LONG)attributes,
         AutoMStr(expose), &pwszExposed));

      return pwszExposed;
   }

   void VssBackupComponents::FreeWriterMetadata()
   {
      CheckCom(m_backup->FreeWriterMetadata());
   }

   void VssBackupComponents::FreeWriterStatus()
   {
      CheckCom(m_backup->FreeWriterStatus());
   }

   void VssBackupComponents::GatherWriterMetadata()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->GatherWriterMetadata(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
   }

   IVssAsyncResult^ VssBackupComponents::BeginGatherWriterMetadata(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->GatherWriterMetadata(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndGatherWriterMetadata(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::GatherWriterStatus()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->GatherWriterStatus(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
   }

   IVssAsyncResult^ VssBackupComponents::BeginGatherWriterStatus(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->GatherWriterStatus(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndGatherWriterStatus(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   VssSnapshotProperties^ VssBackupComponents::GetSnapshotProperties(Guid snapshotId)
   {
      VSS_SNAPSHOT_PROP prop;
      CheckCom(m_backup->GetSnapshotProperties(ToVssId(snapshotId), &prop));
      return CreateVssSnapshotProperties(&prop);
   }

   VssBackupComponents::WriterStatusList::WriterStatusList(VssBackupComponents^ backupComponents)
      : m_backupComponents(backupComponents)
   {
   }

   int VssBackupComponents::WriterStatusList::Count::get()
   {
      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT cWriters;
      CheckCom(m_backupComponents->m_backup->GetWriterStatusCount(&cWriters));
      return (int)cWriters;
   }

   VssWriterStatusInfo^ VssBackupComponents::WriterStatusList::default::get(int index)
   {
      if (index < 0 || index > Count)
         throw gcnew ArgumentOutOfRangeException("index");

      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      VSS_ID idInstance, idWriter;
      AutoBStr bstrWriter;
      VSS_WRITER_STATE eState;
      HRESULT hrResultFailure;

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      HRESULT hrApplication;
      AutoBStr bstrApplicationMessage = NULL;
      IVssBackupComponentsEx3 *pVbc3 = m_backupComponents->GetIVssBackupComponentsEx3();
      if (pVbc3 != NULL)
      {
         CheckCom(pVbc3->GetWriterStatusEx(index, &idInstance, &idWriter, &bstrWriter, &eState, &hrResultFailure, &hrApplication, &bstrApplicationMessage));
         return gcnew VssWriterStatusInfo(ToGuid(idInstance), ToGuid(idWriter), bstrWriter, (VssWriterState)eState, (VssError)hrResultFailure, hrApplication, bstrApplicationMessage);
      }
#endif
      CheckCom(m_backupComponents->m_backup->GetWriterStatus(index, &idInstance, &idWriter, &bstrWriter, &eState, &hrResultFailure));
      return gcnew VssWriterStatusInfo(ToGuid(idInstance), ToGuid(idWriter), bstrWriter, (VssWriterState)eState, (VssError)hrResultFailure);
   }


   VssBackupComponents::WriterComponentsList::WriterComponentsList(VssBackupComponents^ backupComponents)
      : m_backupComponents(backupComponents)
   {
   }

   int VssBackupComponents::WriterComponentsList::Count::get()
   {
      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT cComponent = 0;
      CheckCom(m_backupComponents->m_backup->GetWriterComponentsCount(&cComponent));
      return (int)cComponent;
   }

   IVssWriterComponents^ VssBackupComponents::WriterComponentsList::default::get(int index)
   {
      if (index < 0 || index > Count)
         throw gcnew ArgumentOutOfRangeException("index");

      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      IVssWriterComponentsExt *pWriterComponents;
      CheckCom(m_backupComponents->m_backup->GetWriterComponents(index, &pWriterComponents));
      return VssWriterComponents::Adopt(pWriterComponents);
   }

   VssBackupComponents::WriterMetadataList::WriterMetadataList(VssBackupComponents^ backupComponents)
      : m_backupComponents(backupComponents)
   {
   }

   int VssBackupComponents::WriterMetadataList::Count::get()
   {
      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      UINT iCount;
      CheckCom(m_backupComponents->m_backup->GetWriterMetadataCount(&iCount));
      return (int)iCount;
   }

   IVssExamineWriterMetadata^ VssBackupComponents::WriterMetadataList::default::get(int index)
   {
      if (index < 0 || index > Count)
         throw gcnew ArgumentOutOfRangeException("index");

      if (m_backupComponents->m_backup == 0)
         throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

      VSS_ID idWriterInstance;
      ::IVssExamineWriterMetadata *ewm;
      CheckCom(m_backupComponents->m_backup->GetWriterMetadata(index, &idWriterInstance, &ewm));
      return VssExamineWriterMetadata::Adopt(ewm);
   }

   IList<IVssExamineWriterMetadata^>^ VssBackupComponents::WriterMetadata::get()
   {
      return m_writerMetadata;
   }

   IList<IVssWriterComponents^>^ VssBackupComponents::WriterComponents::get()
   {
      return m_writerComponents;
   }

   IList<VssWriterStatusInfo^>^ VssBackupComponents::WriterStatus::get()
   {
      return m_writerStatus;
   }

   void VssBackupComponents::ImportSnapshots()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->ImportSnapshots(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
   }


   IVssAsyncResult^ VssBackupComponents::BeginImportSnapshots(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->ImportSnapshots(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndImportSnapshots(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::InitializeForBackup(String^ xml)
   {
      CheckCom(m_backup->InitializeForBackup(AutoMBStr(xml)));
   }

   void VssBackupComponents::InitializeForRestore(String^ xml)
   {
      CheckCom(m_backup->InitializeForRestore(NoNullAutoMBStr(xml)));
   }

   bool VssBackupComponents::IsVolumeSupported(String^ volumeName, Guid providerId)
   {
      BOOL eSupported;
      CheckCom(m_backup->IsVolumeSupported(ToVssId(providerId), NoNullAutoMBStr(volumeName), &eSupported));
      return (eSupported != 0);
   }

   bool VssBackupComponents::IsVolumeSupported(String^ volumeName)
   {
      BOOL eSupported;
      CheckCom(m_backup->IsVolumeSupported(ToVssId(Guid::Empty), NoNullAutoMBStr(volumeName), &eSupported));
      return (eSupported != 0);
   }

   void VssBackupComponents::PostRestore()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PostRestore(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);
   }

   IVssAsyncResult^ VssBackupComponents::BeginPostRestore(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PostRestore(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndPostRestore(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::PrepareForBackup()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PrepareForBackup(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);      
   }

   IVssAsyncResult^ VssBackupComponents::BeginPrepareForBackup(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PrepareForBackup(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndPrepareForBackup(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   void VssBackupComponents::PreRestore()
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PreRestore(&pAsync));
      WaitCheckAndReleaseVssAsyncOperation(pAsync);      
   }

   IVssAsyncResult^ VssBackupComponents::BeginPreRestore(AsyncCallback^ userCallback, Object^ stateObject)
   {
      ::IVssAsync *pAsync;
      CheckCom(m_backup->PreRestore(&pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
   }

   void VssBackupComponents::EndPreRestore(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }

   IEnumerable<VssSnapshotProperties ^>^ VssBackupComponents::QuerySnapshots()
   {
      IVssEnumObject *pEnum;
      VSS_OBJECT_PROP rgelt;
      ULONG celtFetched = 0;
      IList<VssSnapshotProperties^> ^list = gcnew List<VssSnapshotProperties^>();

      CheckCom(m_backup->Query(GUID_NULL, VSS_OBJECT_NONE, VSS_OBJECT_SNAPSHOT, &pEnum));

      try
      {
         while (true)
         {
            CheckCom(pEnum->Next(1, &rgelt, &celtFetched));
            
            if (celtFetched == 0)
               return list;

            // Should always be snapshot, but just in case it isn't, we simply skip it.
            if (rgelt.Type == VSS_OBJECT_SNAPSHOT)
            {
               list->Add(CreateVssSnapshotProperties(&rgelt.Obj.Snap));
            }
         }
      }
      finally
      {
         pEnum->Release();
      }
   }

   IEnumerable<VssProviderProperties ^>^ VssBackupComponents::QueryProviders()
   {
      IVssEnumObject *pEnum;
      VSS_OBJECT_PROP rgelt;
      ULONG celtFetched = 0;
      IList<VssProviderProperties^> ^list = gcnew List<VssProviderProperties^>();

      CheckCom(m_backup->Query(GUID_NULL, VSS_OBJECT_NONE, VSS_OBJECT_PROVIDER, &pEnum));

      try
      {
         while (true)
         {
            CheckCom(pEnum->Next(1, &rgelt, &celtFetched));
            
            if (celtFetched == 0)
               return list;

            // Should always be a provider, but just in case it isn't, we simply skip it.
            if (rgelt.Type == VSS_OBJECT_PROVIDER)
            {
               list->Add(CreateVssProviderProperties(&rgelt.Obj.Prov));
            }
         }
      }
      finally
      {
         pEnum->Release();
      }
   }



   IVssAsyncResult^ VssBackupComponents::BeginQueryRevertStatus(String^ volume, AsyncCallback^ userCallback, Object^ stateObject)
   {
#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003 || ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINVISTAORLATER
      OperatingSystemInfo::RequireServerOrClientAtLeast(OSVersionName::WindowsServer2003, 1, OSVersionName::WindowsVista, 1);
      ::IVssAsync *pAsync;
      CheckCom(m_backup->QueryRevertStatus(NoNullAutoMStr(volume), &pAsync));
      return VssAsyncResult::Create(pAsync, userCallback, stateObject);
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::EndQueryRevertStatus(IAsyncResult^ asyncResult)
   {
      VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
      result->EndInvoke();
   }


   void VssBackupComponents::RevertToSnapshot(Guid snapshotId, bool forceDismount)
   {
#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003 || ALPHAVSS_TARGET == ALPHAVSS_TARGET_WINVISTAORLATER
      OperatingSystemInfo::RequireServer(OSVersionName::WindowsServer2003, 1);
      CheckCom(m_backup->RevertToSnapshot(ToVssId(snapshotId), forceDismount));
#else
      UnsupportedOs();
#endif
   }

   String^ VssBackupComponents::SaveAsXml()
   {
      AutoBStr bstrXML;
      CheckCom(m_backup->SaveAsXML(&bstrXML));
      return bstrXML;
   }

   void VssBackupComponents::SetAdditionalRestores(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool additionalResources)
   {
      CheckCom(m_backup->SetAdditionalRestores(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), additionalResources));
   }

   void VssBackupComponents::SetAuthoritativeRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool isAuthorative)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssBackupComponentsEx2()->SetAuthoritativeRestore(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), isAuthorative));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::SetRestoreName(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreName)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssBackupComponentsEx2()->SetRestoreName(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(restoreName)));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::SetBackupOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ backupOptions)
   {
      CheckCom(m_backup->SetBackupOptions(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(backupOptions)));
   }

   void VssBackupComponents::SetBackupState(bool selectComponents, bool backupBootableSystemState, VssBackupType backupType, bool partialFileSupport)
   {
      CheckCom(m_backup->SetBackupState(selectComponents, backupBootableSystemState, (VSS_BACKUP_TYPE)backupType, partialFileSupport));
   }

   void VssBackupComponents::SetBackupSucceeded(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool succeeded)
   {
      CheckCom(m_backup->SetBackupSucceeded(ToVssId(instanceId), ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), succeeded));
   }

   void VssBackupComponents::SetContext(VssVolumeSnapshotAttributes context)
   {
      CheckCom(m_backup->SetContext((LONG)context));
   }

   void VssBackupComponents::SetContext(VssSnapshotContext context)
   {
      CheckCom(m_backup->SetContext((LONG)context));
   }

   void VssBackupComponents::SetFileRestoreStatus(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssFileRestoreStatus status)
   {
      CheckCom(m_backup->SetFileRestoreStatus(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), (VSS_FILE_RESTORE_STATUS)status));
   }

   void VssBackupComponents::SetPreviousBackupStamp(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ previousBackupStamp)
   {
      CheckCom(m_backup->SetPreviousBackupStamp(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(previousBackupStamp)));
   }

   void VssBackupComponents::SetRangesFilePath(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, int partialFileIndex, String^ rangesFile)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
      CheckCom(m_backup->SetRangesFilePath(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), partialFileIndex, NoNullAutoMStr(rangesFile)));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::SetRestoreOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreOptions)
   {
      CheckCom(m_backup->SetRestoreOptions(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(restoreOptions)));
   }

   void VssBackupComponents::SetRestoreState(VssRestoreType restoreType)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
      CheckCom(m_backup->SetRestoreState((VSS_RESTORE_TYPE)restoreType));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::SetRollForward(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssRollForwardType rollType, String^ rollForwardPoint)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssBackupComponentsEx2()->SetRollForward(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), (VSS_ROLLFORWARD_TYPE)rollType, NoNullAutoMStr(rollForwardPoint)));
#else
      UnsupportedOs();
#endif
   }

   void VssBackupComponents::SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore)
   {
      CheckCom(m_backup->SetSelectedForRestore(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), selectedForRestore));
   }

   void VssBackupComponents::SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore, Guid instanceId)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
      OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003, 1);
      CheckCom(RequireIVssBackupComponentsEx()->SetSelectedForRestoreEx(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), selectedForRestore, ToVssId(instanceId)));
#else
      UnsupportedOs();
#endif
   }

   Guid VssBackupComponents::StartSnapshotSet()
   {
      VSS_ID snapshotSetId;
      CheckCom(m_backup->StartSnapshotSet(&snapshotSetId));
      return ToGuid(snapshotSetId);
   }

   void VssBackupComponents::UnexposeSnapshot(Guid snapshotId)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssBackupComponentsEx2()->UnexposeSnapshot(ToVssId(snapshotId)));
#else
      UnsupportedOs();
#endif
   }

    Guid VssBackupComponents::GetSessionId()
    {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
        VSS_ID sessionId;
        CheckCom(RequireIVssBackupComponentsEx3()->GetSessionId(&sessionId));
        return ToGuid(sessionId);
#else
        UnsupportedOs();
#endif
    }

    void VssBackupComponents::AddSnapshotToRecoverySet(Guid snapshotId, String^ destinationVolume)
    {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
       CheckCom(RequireIVssBackupComponentsEx3()->AddSnapshotToRecoverySet(ToVssId(snapshotId), 0, AutoMStr(destinationVolume)));
#else
       UnsupportedOs();
#endif
    }

    void VssBackupComponents::RecoverSet(VssRecoveryOptions options)
    {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
       ::IVssAsync *pAsync;
       CheckCom(RequireIVssBackupComponentsEx3()->RecoverSet((DWORD)options, &pAsync));
       WaitCheckAndReleaseVssAsyncOperation(pAsync);
#else
       UnsupportedOs();
#endif
    }

    IVssAsyncResult^ VssBackupComponents::BeginRecoverSet(VssRecoveryOptions options, AsyncCallback^ userCallback, Object^ stateObject)
    {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
       ::IVssAsync *pAsync;
       CheckCom(RequireIVssBackupComponentsEx3()->RecoverSet((DWORD)options, &pAsync));
       return VssAsyncResult::Create(pAsync, userCallback, stateObject);
#else
       UnsupportedOs();
#endif
    }

    void VssBackupComponents::EndRecoverSet(IAsyncResult^ asyncResult)
    {
       VssAsyncResult^ result = safe_cast<VssAsyncResult^>(asyncResult);
       result->EndInvoke();
    }
}
} }