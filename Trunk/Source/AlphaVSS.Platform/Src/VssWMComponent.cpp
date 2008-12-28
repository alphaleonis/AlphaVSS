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

#include "VssWMComponent.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssWMComponent^ VssWMComponent::Adopt(IVssWMComponent *component)
	{
		try
		{
			return gcnew VssWMComponent(component);
		}
		catch (...)
		{
			component->Release();
			throw;
		}
	}

	VssWMComponent::VssWMComponent(IVssWMComponent *component)
		: mComponent(component)
	{		
		PVSSCOMPONENTINFO info;
		CheckCom((mComponent->GetComponentInfo(&info)));
		try
		{
			mType = ((VssComponentType)info->type);
			mLogicalPath = (FromBStr(info->bstrLogicalPath));
			mComponentName = (FromBStr(info->bstrComponentName));
			mCaption = (FromBStr(info->bstrCaption));
			mRestoreMetadata = (info->bRestoreMetadata);
			mNotifyOnBackupComplete = (info->bNotifyOnBackupComplete);
			mSelectable = (info->bSelectable);


#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
			// The following should be available in WinXP version as well supposedly, but does not 
			// appear to be.
			mSelectableForRestore = (info->bSelectableForRestore);
			mDependencyCount = (info->cDependencies);
			mComponentFlags = ((VssComponentFlags)info->dwComponentFlags);
#endif
			mFileCount = (info->cFileCount);
			mDatabaseFileCount = (info->cDatabases);
			mDatabaseLogFileCount = (info->cLogFiles);
			if (info->pbIcon != 0)
			{
				mIcon = gcnew array<byte>(info->cbIcon);
				System::Runtime::InteropServices::Marshal::Copy((IntPtr)info->pbIcon, mIcon, 0, info->cbIcon);
			}
		}
		finally
		{
			CheckCom(mComponent->FreeComponentInfo(info));
		}
	}

	VssWMComponent::~VssWMComponent()
	{
		this->!VssWMComponent();
	}

	VssWMComponent::!VssWMComponent()
	{
		if (mComponent != 0)
		{
			mComponent->Release();
			mComponent = 0;
		}
	}

	VssComponentType VssWMComponent::Type::get()
	{
		return mType; 
	}

	String^ VssWMComponent::LogicalPath::get()
	{
		return mLogicalPath; 
	}

	String^ VssWMComponent::ComponentName::get()
	{
		return mComponentName;
	}

	String^ VssWMComponent::Caption::get()
	{
		return mCaption;
	}

	array<byte>^ VssWMComponent::Icon::get()
	{
		return mIcon;
	}

	bool VssWMComponent::RestoreMetadata::get()
	{
		return mRestoreMetadata;
	}

	bool VssWMComponent::NotifyOnBackupComplete::get()
	{
		return mNotifyOnBackupComplete;
	}

	bool VssWMComponent::Selectable::get()
	{
		return mSelectable;
	}

	bool VssWMComponent::SelectableForRestore::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		return mSelectableForRestore;
#else
		throw gcnew NotSupportedException(L"This method requires Windows Server 2003 or later");
#endif
	}

	VssComponentFlags VssWMComponent::ComponentFlags::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		return mComponentFlags;
#else
		throw gcnew NotSupportedException(L"This method requires Windows Server 2003 or later");
#endif
	}

	IList<VssWMFileDescription^>^ VssWMComponent::Files::get()
	{
		if (mFiles != nullptr)
			return mFiles;

		List<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(mFileCount);

		for (UINT i = 0; i < mFileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mComponent->GetFile(i, &filedesc));
			list->Add(VssWMFileDescription::Adopt(filedesc));
		}
		mFiles = list->AsReadOnly();
		return mFiles;
	}

	IList<VssWMFileDescription^>^ VssWMComponent::DatabaseFiles::get()
	{
		if (mDatabaseFiles != nullptr)
			return mDatabaseFiles;

		List<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(mDatabaseFileCount);

		for (UINT i = 0; i < mDatabaseFileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mComponent->GetDatabaseFile(i, &filedesc));
			list->Add(VssWMFileDescription::Adopt(filedesc));
		}
		mDatabaseFiles = list->AsReadOnly();
		return mDatabaseFiles;
	}

	IList<VssWMFileDescription^>^ VssWMComponent::DatabaseLogFiles::get()
	{
		if (mDatabaseLogFiles != nullptr)
			return mDatabaseLogFiles;

		List<VssWMFileDescription^>^ list = gcnew List<VssWMFileDescription^>(mDatabaseLogFileCount);

		for (UINT i = 0; i < mDatabaseLogFileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(mComponent->GetDatabaseLogFile(i, &filedesc));
			list->Add(VssWMFileDescription::Adopt(filedesc));
		}
		mDatabaseLogFiles = list->AsReadOnly();
		return mDatabaseLogFiles;
	}


	IList<VssWMDependency^>^ VssWMComponent::Dependencies::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		if (mDependencies != nullptr)
			return mDependencies;

		List<VssWMDependency^>^ list = gcnew List<VssWMDependency^>(mDependencyCount);

		for (UINT i = 0; i < mDependencyCount; i++)
		{
			IVssWMDependency *dependency;
			CheckCom(mComponent->GetDependency(i, &dependency));
			list->Add(VssWMDependency::Adopt(dependency));
		}
		mDependencies = list->AsReadOnly();
		return mDependencies;
#else
		throw gcnew NotSupportedException(L"This method requires Windows Server 2003 or later");
#endif
	}


}
} }