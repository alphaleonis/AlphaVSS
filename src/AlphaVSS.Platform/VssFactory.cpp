

#include "pch.h"

#include "VssFactory.h"
#include "VssInfoProvider.h"
#include "VssBackupComponents.h"
#include "VssSnapshotManagement.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssFactory::VssFactory()
	{
	}

	IVssBackupComponents^ VssFactory::CreateVssBackupComponents()
	{
		return gcnew VssBackupComponents();
	}

	IVssExamineWriterMetadata^ VssFactory::CreateVssExamineWriterMetadata(String^ xml)
	{
		::IVssExamineWriterMetadata *pMetadata;
		CheckCom(::CreateVssExamineWriterMetadata(NoNullAutoMBStr(xml), &pMetadata));
		return VssExamineWriterMetadata::Adopt(pMetadata);

	}

	IVssSnapshotManagement^ VssFactory::CreateVssSnapshotManagement()
	{
		return gcnew VssSnapshotManagement();
	}

	IVssInfoProvider^ VssFactory::GetInfoProvider()
	{
		return gcnew VssInformationProvider();
	}
}
}}