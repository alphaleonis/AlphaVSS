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
	VssExamineWriterMetadata^ VssExamineWriterMetadata::Adopt(IVssExamineWriterMetadata *ewm)
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

	VssExamineWriterMetadata::VssExamineWriterMetadata(IVssExamineWriterMetadata *examineWriterMetadata)
		: mExamineWriterMetadata(examineWriterMetadata)
	{
		Initialize();
	}

	VssExamineWriterMetadata^ VssExamineWriterMetadata::Create(String^ xml)
	{
#if NTDDI_VERSION >= NTDDI_WS03SP1
		IVssExamineWriterMetadata *pMetadata;
		CheckCom(CreateVssExamineWriterMetadata(NoNullAutoMBStr(xml), &pMetadata));
		return VssExamineWriterMetadata::Adopt(pMetadata);
#else
		throw gcnew NotSupportedException(L"This method is not supported on Windows XP");
#endif

	}

	void VssExamineWriterMetadata::Initialize()
	{
		VSS_ID idInstance, idWriter;
		AutoBStr bsWriterName;
		VSS_USAGE_TYPE usage;
		VSS_SOURCE_TYPE source;
		CheckCom(mExamineWriterMetadata->GetIdentity(&idInstance, &idWriter, &bsWriterName, &usage, &source));

		mInstanceId = ToGuid(idInstance);
		mWriterId = ToGuid(idWriter);
		mWriterName = bsWriterName;
		mUsage = (VssUsageType)usage;
		mSource = (VssSourceType)source;

		mExcludeFiles = nullptr;
		mComponents = nullptr;
		mRestoreMethod = nullptr;
		mAlternateLocationMappings = nullptr;
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
	}

	bool VssExamineWriterMetadata::LoadFromXML(String^ xml)
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

	String^ VssExamineWriterMetadata::SaveAsXML()
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

	IList<VssWMFiledesc^>^ VssExamineWriterMetadata::ExcludeFiles::get()
	{
		if (mExcludeFiles != nullptr)
			return mExcludeFiles;

		UINT cIncludeFiles, cExcludeFiles, cComponents;
		CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

		IList<VssWMFiledesc^>^ list = gcnew List<VssWMFiledesc^>(cExcludeFiles);
		for (UINT i = 0; i < cExcludeFiles; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mExamineWriterMetadata->GetExcludeFile(i, &filedesc));
			list->Add(VssWMFiledesc::Adopt(filedesc));
		}
		mExcludeFiles = list;
		return mExcludeFiles;
	}

	IList<VssWMComponent^>^ VssExamineWriterMetadata::Components::get()
	{
		if (mComponents != nullptr)
			return mComponents;

		UINT cIncludeFiles, cExcludeFiles, cComponents;
		CheckCom(mExamineWriterMetadata->GetFileCounts(&cIncludeFiles, &cExcludeFiles, &cComponents));

		IList<VssWMComponent^>^ list = gcnew List<VssWMComponent^>(cComponents);
		for (UINT i = 0; i < cComponents; i++)
		{
			IVssWMComponent *component;
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

	IList<VssWMFiledesc^>^ VssExamineWriterMetadata::AlternateLocationMappings::get()
	{
		if (mAlternateLocationMappings != nullptr)
			return mAlternateLocationMappings;

		// Return an empty list if no restore method is available
		if (this->RestoreMethod == nullptr)
			return (gcnew List<VssWMFiledesc^>(0))->AsReadOnly();

		IList<VssWMFiledesc^>^ list = gcnew List<VssWMFiledesc^>(this->RestoreMethod->MappingCount);

		for (int i = 0; i < this->RestoreMethod->MappingCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mExamineWriterMetadata->GetAlternateLocationMapping(i, &filedesc));
			list->Add(VssWMFiledesc::Adopt(filedesc));
		}
		mAlternateLocationMappings = list;
		return mAlternateLocationMappings;
	}

	VssBackupSchema VssExamineWriterMetadata::BackupSchema::get()
	{
#if NTDDI_VERSION >= NTDDI_WS03
		DWORD schema;
		CheckCom(mExamineWriterMetadata->GetBackupSchema(&schema));
		return (VssBackupSchema)schema;
#else
		throw gcnew NotSupportedException(L"This method is not supported until Windows Server 2003");
#endif
	}

}
} }

