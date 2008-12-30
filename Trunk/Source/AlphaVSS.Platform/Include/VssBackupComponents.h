/* Copyright (c) 2008 Peter Palotas
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

#include "VssError.h"
#include "VssWriterComponents.h"
#include "VssExamineWriterMetadata.h"
#include "Macros.h"

using namespace System;
using namespace System::Collections::Generic;

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
#define ALPHAVSS_HAS_BACKUPEX
#endif

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2008
#define ALPHAVSS_HAS_BACKUPEX2
#endif


namespace Alphaleonis { namespace Win32 { namespace Vss
{
	ref class VssAsync;

	private ref class VssBackupComponents : IDisposable, IVssBackupComponents
	{
	public:
		VssBackupComponents();
		~VssBackupComponents();
		!VssBackupComponents();

		virtual void AbortBackup();
		virtual void AddAlternativeLocationMapping(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ path, String^ filespec, bool recursive, String^ destination);
		virtual void AddComponent(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName);
		virtual void AddNewTarget(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ path, String^ fileName, bool recursive, String^ alternatePath);
		virtual void AddRestoreSubcomponent(Guid writerId, VssComponentType componentType, String^ logicalPath, String ^componentName, String^ subcomponentLogicalPath, String^ subcomponentName);
		
		virtual Guid AddToSnapshotSet(String^ volumeName, Guid providerId);
		virtual Guid AddToSnapshotSet(String^ volumeName);
		
		virtual IVssAsync^ BackupComplete();
		virtual void BreakSnapshotSet(Guid snapshotSetId);
		virtual IVssAsync^ BreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags);
		virtual void DeleteSnapshot(Guid snapshotId, bool forceDelete);
		virtual int DeleteSnapshotSet(Guid snapshotSetId, bool forceDelete);

		virtual void DisableWriterClasses(array<Guid> ^ writerClassIds);
		virtual void DisableWriterInstances(array<Guid> ^ writerInstanceIds);
		virtual IVssAsync^ DoSnapshotSet();
		virtual void EnableWriterClasses(array<Guid> ^ writerClassIds);
		virtual String^ ExposeSnapshot(Guid snapshotId, String ^ pathFromRoot, VssVolumeSnapshotAttributes attributes, String ^ expose);
		virtual void FreeWriterMetadata();
		virtual void FreeWriterStatus();
		virtual IVssAsync^ GatherWriterMetadata();
		virtual IVssAsync^ GatherWriterStatus();
		virtual VssSnapshotProperties^ GetSnapshotProperties(Guid snapshotId);
		property IList<IVssWriterComponents^>^ WriterComponents { virtual IList<IVssWriterComponents^>^ get(); }
		property IList<IVssExamineWriterMetadata^>^ WriterMetadata { virtual IList<IVssExamineWriterMetadata^>^ get(); }
		property IList<VssWriterStatusInfo^>^ WriterStatus { virtual IList<VssWriterStatusInfo^>^ get(); }
		virtual IVssAsync^ ImportSnapshots();
		virtual void InitializeForBackup(String^ xml);
		virtual void InitializeForRestore(String^ xml);
		virtual bool IsVolumeSupported(String^ volumeName, Guid providerId);
		virtual bool IsVolumeSupported(String^ volumeName);
		virtual IVssAsync^ PostRestore();
		virtual IVssAsync^ PrepareForBackup();
		virtual IVssAsync^ PreRestore();
		virtual System::Collections::Generic::IEnumerable<VssSnapshotProperties^> ^QuerySnapshots();
		virtual System::Collections::Generic::IEnumerable<VssProviderProperties^> ^QueryProviders();
		virtual IVssAsync^ QueryRevertStatus(String^ volumeName);
		virtual void RevertToSnapshot(Guid snapshotId, bool forceDismount);
		virtual String^ SaveAsXml();
		virtual void SetAdditionalRestores(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool additionalResources);
        virtual void SetAuthoritativeRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool isAuthorative);
		virtual void SetBackupOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ backupOptions);
		virtual void SetBackupState(bool selectComponents, bool backupBootableSystemState, VssBackupType backupType, bool partialFileSupport);
		virtual void SetBackupSucceeded(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool succeeded);
		virtual void SetContext(VssVolumeSnapshotAttributes context);
		virtual void SetFileRestoreStatus(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssFileRestoreStatus status);
		virtual void SetPreviousBackupStamp(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ previousBackupStamp);
		virtual void SetRangesFilePath(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, int partialFileIndex, String^ rangesFile);
		virtual void SetRestoreName(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreName);
		virtual void SetRestoreOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreOptions);
		virtual void SetRestoreState(VssRestoreType restoreType);
        virtual void SetRollForward(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssRollForwardType rollType, String^ rollForwardPoint);
		virtual void SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore);
		virtual void SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore, Guid instanceId);

		virtual Guid StartSnapshotSet();
		virtual void UnexposeSnapshot(Guid snapshotId);

	private:
		::IVssBackupComponents *mBackup;

#ifdef ALPHAVSS_HAS_BACKUPEX
		DEFINE_EX_INTERFACE_ACCESSOR(IVssBackupComponentsEx)
#endif

#ifdef ALPHAVSS_HAS_BACKUPEX2
		DEFINE_EX_INTERFACE_ACCESSOR(IVssBackupComponentsEx2)
#endif

		ref class WriterMetadataList : VssListAdapter<IVssExamineWriterMetadata^>
		{
		public:
			WriterMetadataList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property IVssExamineWriterMetadata^ default[int] { virtual IVssExamineWriterMetadata^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};


		ref class WriterComponentsList : VssListAdapter<IVssWriterComponents^>
		{
		public:
			WriterComponentsList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property IVssWriterComponents^ default[int] { virtual IVssWriterComponents^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};

		ref class WriterStatusList : VssListAdapter<VssWriterStatusInfo^>
		{
		public:
			WriterStatusList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property VssWriterStatusInfo^ default[int] { virtual VssWriterStatusInfo^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};

		WriterMetadataList^ mWriterMetadata;
		WriterComponentsList^ mWriterComponents;
		WriterStatusList^ mWriterStatus;
	};


}
} }
