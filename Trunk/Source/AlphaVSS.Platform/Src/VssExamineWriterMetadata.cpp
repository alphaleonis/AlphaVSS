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
#include "StdAfx.h"

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

#ifdef ALPHAVSS_HAS_EWMEX
		if (OperatingSystemInfo::IsAtLeast(OSVersionName::WindowsServer2003, 1))
		{	
			IVssExamineWriterMetadataEx *ex = GetIVssExamineWriterMetadataEx();
			if (ex != 0)
			{
				CheckCom(ex->GetIdentityEx(&idInstance, &idWriter, &bsWriterName, &bsInstanceName, &usage, &source));			
				hasExIdentity = true;
			}
		}
#endif
		if (!hasExIdentity)
			CheckCom(mExamineWriterMetadata->GetIdentity(&idInstance, &idWriter, &bsWriterName, &usage, &source));

		mInstanceId = ToGuid(idInstance);
		mWriterId = ToGuid(idWriter);
		mWriterName = bsWriterName;
		mInstanceName = bsInstanceName;
		mUsage = (VssUsageType)usage;
		mSource = (VssSourceType)source;

		mExcludeFiles = nullptr;
		mComponents = nullptr;
		mRestoreMethod = nullptr;
		mAlternateLocationMappings = nullptr;
		mVersion = nullptr;
		mExcludeFilesFromSnapshot = nullptr;
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
#ifdef ALPHAVSS_HAS_EWMEX
		if (mIVssExamineWriterMetadataEx != 0)
		{
			mIVssExamineWriterMetadataEx->Release();
			mIVssExamineWriterMetadataEx = 0;
		}
#endif

#ifdef ALPHAVSS_HAS_EWMEX2
		if (mIVssExamineWriterMetadataEx2 != 0)
		{
			mIVssExamineWriterMetadataEx2->Release();
			mIVssExamineWriterMetadataEx2 = 0;
		}
#endif		
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
		return mInstanceId;
	}

	Guid VssExamineWriterMetadata::WriterId::get()
	{
		return mWriterId;
	}

	String^ VssExamineWriterMetadata::WriterName::get()
	{
		return mWriterName;
	}

	VssUsageType VssExamineWriterMetadata::Usage::get()
	{
		return mUsage;
	}

	VssSourceType VssExamineWriterMetadata::Source::get()
	{
		return mSource;
	}

	IList<VssWMFileDescription^>^ VssExamineWriterMetadata::ExcludeFiles::get()
	{
		if (mExcludeFiles != nullptr)
			return mExcludeFiles;

		UINT cIncludeFiles, cExcludeFiles, cComponents;
		CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

		IList<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(cExcludeFiles);
		for (UINT i = 0; i < cExcludeFiles; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mExamineWriterMetadata->GetExcludeFile(i, &filedesc));
			list->Add(CreateVssWMFileDescription(filedesc));
		}
		mExcludeFiles = list;
		return mExcludeFiles;
	}

	IList<IVssWMComponent^>^ VssExamineWriterMetadata::Components::get()
	{
		if (mComponents != nullptr)
			return mComponents;

		UINT cIncludeFiles, cExcludeFiles, cComponents;
		CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

		IList<IVssWMComponent^>^ list = gcnew List<IVssWMComponent^>(cComponents);
		for (UINT i = 0; i < cComponents; i++)
		{
			::IVssWMComponent *component;
			CheckCom(mExamineWriterMetadata->GetComponent(i, &component));
			list->Add(VssWMComponent::Adopt(component));
		}
		mComponents = list;
		return mComponents;
	}

	VssWMRestoreMethod^ VssExamineWriterMetadata::RestoreMethod::get()
	{
		if (mRestoreMethod != nullptr)
			return mRestoreMethod;

		VSS_RESTOREMETHOD_ENUM eMethod;
		AutoBStr bstrService, bstrUserProcedure;
		VSS_WRITERRESTORE_ENUM eWriterRestore;
		bool bRebootRequired;
		UINT iMappings;
		HRESULT result = mExamineWriterMetadata->GetRestoreMethod(&eMethod, &bstrService, &bstrUserProcedure, &eWriterRestore, &bRebootRequired, &iMappings);

		if (FAILED(result))
			ThrowException(result, 0);

		if (result == S_FALSE)
			return nullptr;

		mRestoreMethod = gcnew VssWMRestoreMethod((VssRestoreMethod)eMethod, bstrService, bstrUserProcedure, (VssWriterRestore)eWriterRestore, bRebootRequired, iMappings);
		return mRestoreMethod;

	}

	IList<VssWMFileDescription^>^ VssExamineWriterMetadata::AlternateLocationMappings::get()
	{
		if (mAlternateLocationMappings != nullptr)
			return mAlternateLocationMappings;

		// Return an empty list if no restore method is available
		if (this->RestoreMethod == nullptr)
			return (gcnew List<VssWMFileDescription^>(0))->AsReadOnly();

		IList<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(this->RestoreMethod->MappingCount);

		for (int i = 0; i < this->RestoreMethod->MappingCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mExamineWriterMetadata->GetAlternateLocationMapping(i, &filedesc));
			list->Add(CreateVssWMFileDescription(filedesc));
		}
		mAlternateLocationMappings = list;
		return mAlternateLocationMappings;
	}

	VssBackupSchema VssExamineWriterMetadata::BackupSchema::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		OperatingSystemInfo::RequireAtLeast(OSVersionName::WindowsServer2003);
		DWORD schema;
		CheckCom(mExamineWriterMetadata->GetBackupSchema(&schema));
		return (VssBackupSchema)schema;
#else
		throw gcnew NotSupportedException(L"This method is not supported until Windows Server 2003");
#endif
	}

	String^ VssExamineWriterMetadata::InstanceName::get()
	{
		return mInstanceName;
	}

	Version^ VssExamineWriterMetadata::Version::get()
	{
		if (mVersion == nullptr)
		{
			DWORD dwMajorVersion = 0;
			DWORD dwMinorVersion = 0;
#ifdef ALPHAVSS_HAS_EWMEX2
			CheckCom(RequireIVssExamineWriterMetadataEx2()->GetVersion(&dwMajorVersion, &dwMinorVersion));
#endif
			mVersion = gcnew System::Version(dwMajorVersion, dwMinorVersion);
		}
		return mVersion;
	}
	
	IList<VssWMFileDescription^>^ VssExamineWriterMetadata::ExcludeFromSnapshotFiles::get()
	{
		if (mExcludeFilesFromSnapshot != nullptr)
			return mExcludeFilesFromSnapshot;

#ifdef ALPHAVSS_HAS_EWMEX2
		UINT cExcludedFromSnapshot;

		CheckCom(RequireIVssExamineWriterMetadataEx2()->GetExcludeFromSnapshotCount(&cExcludedFromSnapshot));

		IList<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(cExcludedFromSnapshot);
		for (UINT i = 0; i < cExcludedFromSnapshot; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(RequireIVssExamineWriterMetadataEx2()->GetExcludeFromSnapshotFile(i, &filedesc));
			list->Add(CreateVssWMFileDescription(filedesc));
		}
		mExcludeFiles = list;
#else
		mExcludeFiles = gcnew List<VssWMFileDescription^>();
#endif
		return mExcludeFiles;
	}

}
} }

