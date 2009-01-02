/* Copyright (c) 2008-2009 Peter Palotas
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
#include "VssAsync.h"

#include "Utils.h"
#include "Macros.h"

using namespace System::Collections::Generic;
using namespace System::Security::Permissions;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssBackupComponents::VssBackupComponents()
		: mBackup(0), 
#ifdef ALPHAVSS_HAS_BACKUP_EX
		mBackupEx(0),
#endif
#ifdef ALPHAVSS_HAS_BACKUP_EX2
		mBackupEx2(0),
#endif
		mWriterMetadata(nullptr),
		mWriterComponents(nullptr),
		mWriterStatus(nullptr)

	{
		Console::WriteLine(System::AppDomain::CurrentDomain->FriendlyName);

		mWriterMetadata = gcnew WriterMetadataList(this);
		mWriterComponents = gcnew WriterComponentsList(this);
		mWriterStatus = gcnew WriterStatusList(this);

		pin_ptr<::IVssBackupComponents *> pVssObject = &mBackup;
		CheckCom( CreateVssBackupComponents(pVssObject) );
	}

	VssBackupComponents::~VssBackupComponents()
	{
		this->!VssBackupComponents();
	}

	VssBackupComponents::!VssBackupComponents()
	{
		if (mBackup != 0)
		{
			mBackup->Release();
			mBackup = 0;
		}

#ifdef ALPHAVSS_HAS_BACKUPEX
		if (mIVssBackupComponentsEx != 0)
		{
			mIVssBackupComponentsEx->Release();
			mIVssBackupComponentsEx = 0;
		}
#endif

#ifdef ALPHAVSS_HAS_BACKUPEX2
		if (mIVssBackupComponentsEx2 != 0)
		{
			mIVssBackupComponentsEx2->Release();
			mIVssBackupComponentsEx2 = 0;
		}
#endif
	}


	void VssBackupComponents::AbortBackup()
	{
		CheckCom(mBackup->AbortBackup());
	}

	void VssBackupComponents::AddAlternativeLocationMapping(Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName, String ^ path, String ^ filespec, bool recursive, String ^ destination)
	{
		CheckCom(mBackup->AddAlternativeLocationMapping(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, 
			AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(path),
			NoNullAutoMStr(filespec), recursive, NoNullAutoMStr(destination)));
	}

	void VssBackupComponents::AddComponent(Guid instanceId, Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName)
	{
		CheckCom(mBackup->AddComponent(ToVssId(instanceId), ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, 
			AutoMStr(logicalPath), NoNullAutoMStr(componentName)));
	}

	void VssBackupComponents::AddNewTarget(Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName, String ^ path, String ^ fileName, bool recursive, String ^ alternatePath)
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
		CheckCom(mBackup->AddNewTarget(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType,
			AutoMStr(logicalPath), NoNullAutoMStr(componentName),
			NoNullAutoMStr(path), NoNullAutoMStr(fileName),
			recursive, NoNullAutoMStr(alternatePath)));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::AddRestoreSubcomponent(Guid writerId, VssComponentType componentType, String^ logicalPath, String ^componentName, String^ subcomponentLogicalPath, String^ subcomponentName)
	{
		CheckCom(mBackup->AddRestoreSubcomponent(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType,
			AutoMStr(logicalPath),
			NoNullAutoMStr(componentName),
			NoNullAutoMStr(subcomponentLogicalPath),
			NoNullAutoMStr(subcomponentName),
			false));
	}

	Guid VssBackupComponents::AddToSnapshotSet(String ^ volumeName, Guid providerId)
	{
		VSS_ID idSnapshot;
		CheckCom(mBackup->AddToSnapshotSet(NoNullAutoMStr(volumeName), ToVssId(providerId), &idSnapshot));
		return ToGuid(idSnapshot);
	}

	Guid VssBackupComponents::AddToSnapshotSet(String^ volumeName)
	{
		VSS_ID idSnapshot;
		CheckCom(mBackup->AddToSnapshotSet(NoNullAutoMStr(volumeName), ToVssId(Guid::Empty), &idSnapshot));
		return ToGuid(idSnapshot);
	}

	[SecurityPermissionAttribute(SecurityAction::LinkDemand)]
	IVssAsync ^ VssBackupComponents::BackupComplete()
	{
		::IVssAsync *pAsync;
		CheckCom(mBackup->BackupComplete(&pAsync));
		return VssAsync::Adopt(pAsync);
	}

	void VssBackupComponents::BreakSnapshotSet(Guid snapshotSetId)
	{
		CheckCom(mBackup->BreakSnapshotSet(ToVssId(snapshotSetId)));
	}

	IVssAsync^ VssBackupComponents::BreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX2
		::IVssAsync *pAsync;
		CheckCom(RequireIVssBackupComponentsEx2()->BreakSnapshotSetEx(ToVssId(snapshotSetId), (_VSS_HARDWARE_OPTIONS)breakFlags, &pAsync));
		return VssAsync::Adopt(pAsync);
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::DeleteSnapshot(Guid snapshotId, bool forceDelete)
	{
		LONG lDeletedSnapshots;
		VSS_ID nonDeletedSnapshotID;
		CheckCom(mBackup->DeleteSnapshots(ToVssId(snapshotId), VSS_OBJECT_SNAPSHOT, forceDelete, &lDeletedSnapshots, &nonDeletedSnapshotID));
	}

	int VssBackupComponents::DeleteSnapshotSet(Guid snapshotSetId, bool forceDelete)
	{
		LONG lDeletedSnapshots;
		VSS_ID nonDeletedSnapshotID;
		HRESULT hr = mBackup->DeleteSnapshots(ToVssId(snapshotSetId), VSS_OBJECT_SNAPSHOT_SET, forceDelete, &lDeletedSnapshots, &nonDeletedSnapshotID);
		
		if (FAILED(hr))
		{
			throw gcnew VssDeleteSnapshotsFailedException(lDeletedSnapshots, ToGuid(nonDeletedSnapshotID), GetExceptionForHr(hr));
		}

		return lDeletedSnapshots;
	}

	void VssBackupComponents::DisableWriterClasses(array<Guid> ^ writerClassIds)
	{
		CheckCom(mBackup->DisableWriterClasses(VssIds(writerClassIds), writerClassIds->Length));
	}

	void VssBackupComponents::DisableWriterInstances(array<Guid> ^ writerInstanceIds)
	{
		CheckCom(mBackup->DisableWriterInstances(VssIds(writerInstanceIds), writerInstanceIds->Length));
	}

	IVssAsync^ VssBackupComponents::DoSnapshotSet()
	{
		::IVssAsync *vssAsync;
		CheckCom(mBackup->DoSnapshotSet(&vssAsync));
		return VssAsync::Adopt(vssAsync);
	}

	void VssBackupComponents::EnableWriterClasses(array<Guid> ^ writerClassIds)
	{
		CheckCom(mBackup->EnableWriterClasses(VssIds(writerClassIds), writerClassIds->Length));
	}

	String^ VssBackupComponents::ExposeSnapshot(Guid snapshotId, String ^ pathFromRoot, VssVolumeSnapshotAttributes attributes, String ^ expose)
	{
		AutoPwsz pwszExposed;

		CheckCom(mBackup->ExposeSnapshot(ToVssId(snapshotId), AutoMStr(pathFromRoot), (LONG)attributes,
			AutoMStr(expose), &pwszExposed));

		return pwszExposed;
	}

	void VssBackupComponents::FreeWriterMetadata()
	{
		CheckCom(mBackup->FreeWriterMetadata());
	}

	void VssBackupComponents::FreeWriterStatus()
	{
		CheckCom(mBackup->FreeWriterStatus());
	}

	IVssAsync^ VssBackupComponents::GatherWriterMetadata()
	{
		::IVssAsync *vssAsync;
		CheckCom(mBackup->GatherWriterMetadata(&vssAsync));
		return VssAsync::Adopt(vssAsync);
	}

	IVssAsync^ VssBackupComponents::GatherWriterStatus()
	{
		::IVssAsync *vssAsync;
		CheckCom(mBackup->GatherWriterStatus(&vssAsync));
		return VssAsync::Adopt(vssAsync);
	}

	VssSnapshotProperties^ VssBackupComponents::GetSnapshotProperties(Guid snapshotId)
	{
		VSS_SNAPSHOT_PROP prop;
		CheckCom(mBackup->GetSnapshotProperties(ToVssId(snapshotId), &prop));
		return CreateVssSnapshotProperties(&prop);
	}

	VssBackupComponents::WriterStatusList::WriterStatusList(VssBackupComponents^ backupComponents)
		: mBackupComponents(backupComponents)
	{
	}

	int VssBackupComponents::WriterStatusList::Count::get()
	{
		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		UINT cWriters;
		CheckCom(mBackupComponents->mBackup->GetWriterStatusCount(&cWriters));
		return (int)cWriters;
	}

	VssWriterStatusInfo^ VssBackupComponents::WriterStatusList::default::get(int index)
	{
		if (index < 0 || index > Count)
			throw gcnew ArgumentOutOfRangeException("index");

		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		VSS_ID idInstance, idWriter;
		AutoBStr bstrWriter;
		VSS_WRITER_STATE eState;
		HRESULT hrResultFailure;
		CheckCom(mBackupComponents->mBackup->GetWriterStatus(index, &idInstance, &idWriter, &bstrWriter, &eState, &hrResultFailure));
		return gcnew VssWriterStatusInfo(ToGuid(idInstance), ToGuid(idWriter), bstrWriter, (VssWriterState)eState, (VssError)hrResultFailure);
	}


	VssBackupComponents::WriterComponentsList::WriterComponentsList(VssBackupComponents^ backupComponents)
		: mBackupComponents(backupComponents)
	{
	}

	int VssBackupComponents::WriterComponentsList::Count::get()
	{
		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		UINT cComponent = 0;
		CheckCom(mBackupComponents->mBackup->GetWriterComponentsCount(&cComponent));
		return (int)cComponent;
	}

	IVssWriterComponents^ VssBackupComponents::WriterComponentsList::default::get(int index)
	{
		if (index < 0 || index > Count)
			throw gcnew ArgumentOutOfRangeException("index");

		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		IVssWriterComponentsExt *pWriterComponents;
		CheckCom(mBackupComponents->mBackup->GetWriterComponents(index, &pWriterComponents));
		return VssWriterComponents::Adopt(pWriterComponents);
	}

	VssBackupComponents::WriterMetadataList::WriterMetadataList(VssBackupComponents^ backupComponents)
		: mBackupComponents(backupComponents)
	{
	}

	int VssBackupComponents::WriterMetadataList::Count::get()
	{
		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		UINT iCount;
		CheckCom(mBackupComponents->mBackup->GetWriterMetadataCount(&iCount));
		return (int)iCount;
	}

	IVssExamineWriterMetadata^ VssBackupComponents::WriterMetadataList::default::get(int index)
	{
		if (index < 0 || index > Count)
			throw gcnew ArgumentOutOfRangeException("index");

		if (mBackupComponents->mBackup == 0)
			throw gcnew ObjectDisposedException("Instance of IList used after the object creating it was disposed.");

		VSS_ID idWriterInstance;
		::IVssExamineWriterMetadata *ewm;
		CheckCom(mBackupComponents->mBackup->GetWriterMetadata(index, &idWriterInstance, &ewm));
		return VssExamineWriterMetadata::Adopt(ewm);
	}

	IList<IVssExamineWriterMetadata^>^ VssBackupComponents::WriterMetadata::get()
	{
		return mWriterMetadata;
	}

	IList<IVssWriterComponents^>^ VssBackupComponents::WriterComponents::get()
	{
		return mWriterComponents;
	}

	IList<VssWriterStatusInfo^>^ VssBackupComponents::WriterStatus::get()
	{
		return mWriterStatus;
	}

	IVssAsync^ VssBackupComponents::ImportSnapshots()
	{
		::IVssAsync *pAsync;
		CheckCom(mBackup->ImportSnapshots(&pAsync));
		return VssAsync::Adopt(pAsync);
	}

	void VssBackupComponents::InitializeForBackup(String^ xml)
	{
		CheckCom(mBackup->InitializeForBackup(AutoMBStr(xml)));
	}

	void VssBackupComponents::InitializeForRestore(String^ xml)
	{
		CheckCom(mBackup->InitializeForRestore(NoNullAutoMBStr(xml)));
	}

	bool VssBackupComponents::IsVolumeSupported(String^ volumeName, Guid providerId)
	{
		BOOL eSupported;
		CheckCom(mBackup->IsVolumeSupported(ToVssId(providerId), NoNullAutoMBStr(volumeName), &eSupported));
		return (eSupported != 0);
	}

	bool VssBackupComponents::IsVolumeSupported(String^ volumeName)
	{
		BOOL eSupported;
		CheckCom(mBackup->IsVolumeSupported(ToVssId(Guid::Empty), NoNullAutoMBStr(volumeName), &eSupported));
		return (eSupported != 0);
	}

	IVssAsync^ VssBackupComponents::PostRestore()
	{
		::IVssAsync *pAsync;
		CheckCom(mBackup->PostRestore(&pAsync));
		return VssAsync::Adopt(pAsync);
	}

	IVssAsync^ VssBackupComponents::PrepareForBackup()
	{
		::IVssAsync *pAsync;
		CheckCom(mBackup->PrepareForBackup(&pAsync));
		return VssAsync::Adopt(pAsync);
	}

	IVssAsync^ VssBackupComponents::PreRestore()
	{
		::IVssAsync *pAsync;
		CheckCom(mBackup->PreRestore(&pAsync));
		return VssAsync::Adopt(pAsync);
	}

	IEnumerable<VssSnapshotProperties ^>^ VssBackupComponents::QuerySnapshots()
	{
		IVssEnumObject *pEnum;
		VSS_OBJECT_PROP rgelt;
		ULONG celtFetched = 0;
		IList<VssSnapshotProperties^> ^list = gcnew List<VssSnapshotProperties^>();

		CheckCom(mBackup->Query(GUID_NULL, VSS_OBJECT_NONE, VSS_OBJECT_SNAPSHOT, &pEnum));

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

		CheckCom(mBackup->Query(GUID_NULL, VSS_OBJECT_NONE, VSS_OBJECT_PROVIDER, &pEnum));

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

	IVssAsync^ VssBackupComponents::QueryRevertStatus(String^ volume)
	{
#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003 || ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2008
		OperatingSystemInfo::RequireWithSPAtLeast(OSVersionName::WindowsServer2003, 1, OSVersionName::WindowsServer2008, 0);
		::IVssAsync *pAsync;
		CheckCom(mBackup->QueryRevertStatus(NoNullAutoMStr(volume), &pAsync));
		return VssAsync::Adopt(pAsync);
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::RevertToSnapshot(Guid snapshotId, bool forceDismount)
	{
#if ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2003 || ALPHAVSS_TARGET == ALPHAVSS_TARGET_WIN2008
		OperatingSystemInfo::RequireWithSPAtLeast(OSVersionName::WindowsServer2003, 1, OSVersionName::WindowsServer2008, 0);		
		CheckCom(mBackup->RevertToSnapshot(ToVssId(snapshotId), forceDismount));
#else
		UnsupportedOs();
#endif
	}

	String^ VssBackupComponents::SaveAsXml()
	{
		AutoBStr bstrXML;
		CheckCom(mBackup->SaveAsXML(&bstrXML));
		return bstrXML;
	}

	void VssBackupComponents::SetAdditionalRestores(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool additionalResources)
	{
		CheckCom(mBackup->SetAdditionalRestores(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), additionalResources));
	}

	void VssBackupComponents::SetAuthoritativeRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool isAuthorative)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX2
		CheckCom(RequireIVssBackupComponentsEx2()->SetAuthoritativeRestore(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), isAuthorative));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::SetRestoreName(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreName)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX2
		CheckCom(RequireIVssBackupComponentsEx2()->SetRestoreName(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(restoreName)));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::SetBackupOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ backupOptions)
	{
		CheckCom(mBackup->SetBackupOptions(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(backupOptions)));
	}

	void VssBackupComponents::SetBackupState(bool selectComponents, bool backupBootableSystemState, VssBackupType backupType, bool partialFileSupport)
	{
		CheckCom(mBackup->SetBackupState(selectComponents, backupBootableSystemState, (VSS_BACKUP_TYPE)backupType, partialFileSupport));
	}

	void VssBackupComponents::SetBackupSucceeded(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool succeeded)
	{
		CheckCom(mBackup->SetBackupSucceeded(ToVssId(instanceId), ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), succeeded));
	}

	void VssBackupComponents::SetContext(VssVolumeSnapshotAttributes context)
	{
		CheckCom(mBackup->SetContext((LONG)context));
	}

	void VssBackupComponents::SetContext(VssSnapshotContext context)
	{
		CheckCom(mBackup->SetContext((LONG)context));
	}

	void VssBackupComponents::SetFileRestoreStatus(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssFileRestoreStatus status)
	{
		CheckCom(mBackup->SetFileRestoreStatus(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), (VSS_FILE_RESTORE_STATUS)status));
	}

	void VssBackupComponents::SetPreviousBackupStamp(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ previousBackupStamp)
	{
		CheckCom(mBackup->SetPreviousBackupStamp(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(previousBackupStamp)));
	}

	void VssBackupComponents::SetRangesFilePath(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, int partialFileIndex, String^ rangesFile)
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
		CheckCom(mBackup->SetRangesFilePath(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), partialFileIndex, NoNullAutoMStr(rangesFile)));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::SetRestoreOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreOptions)
	{
		CheckCom(mBackup->SetRestoreOptions(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), NoNullAutoMStr(restoreOptions)));
	}

	void VssBackupComponents::SetRestoreState(VssRestoreType restoreType)
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);		
		CheckCom(mBackup->SetRestoreState((VSS_RESTORE_TYPE)restoreType));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::SetRollForward(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssRollForwardType rollType, String^ rollForwardPoint)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX2
		CheckCom(RequireIVssBackupComponentsEx2()->SetRollForward(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), (VSS_ROLLFORWARD_TYPE)rollType, NoNullAutoMStr(rollForwardPoint)));
#else
		UnsupportedOs();
#endif
	}

	void VssBackupComponents::SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore)
	{
		CheckCom(mBackup->SetSelectedForRestore(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), selectedForRestore));
	}

	void VssBackupComponents::SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore, Guid instanceId)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003, 1);
		CheckCom(RequireIVssBackupComponentsEx()->SetSelectedForRestoreEx(ToVssId(writerId), (VSS_COMPONENT_TYPE)componentType, AutoMStr(logicalPath), NoNullAutoMStr(componentName), selectedForRestore, ToVssId(instanceId)));
#else
		UnsupportedOs();
#endif
	}

	Guid VssBackupComponents::StartSnapshotSet()
	{
		VSS_ID snapshotSetId;
		CheckCom(mBackup->StartSnapshotSet(&snapshotSetId));
		return ToGuid(snapshotSetId);
	}

	void VssBackupComponents::UnexposeSnapshot(Guid snapshotId)
	{
#ifdef ALPHAVSS_HAS_BACKUPEX2
		CheckCom(RequireIVssBackupComponentsEx2()->UnexposeSnapshot(ToVssId(snapshotId)));
#else
		UnsupportedOs();
#endif
	}

}
} }