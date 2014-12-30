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

#include "VssWMComponent.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssWMComponent^ VssWMComponent::Adopt(::IVssWMComponent *component)
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

	VssWMComponent::VssWMComponent(::IVssWMComponent *component)
		: m_component(component)
	{		
		PVSSCOMPONENTINFO info;
		CheckCom((m_component->GetComponentInfo(&info)));
		try
		{
			m_type = ((VssComponentType)info->type);
			m_logicalPath = (FromBStr(info->bstrLogicalPath));
			m_componentName = (FromBStr(info->bstrComponentName));
			m_caption = (FromBStr(info->bstrCaption));
			m_restoreMetadata = (info->bRestoreMetadata);
			m_notifyOnBackupComplete = (info->bNotifyOnBackupComplete);
			m_selectable = (info->bSelectable);


#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
			m_selectableForRestore = (info->bSelectableForRestore);
			m_dependencyCount = (info->cDependencies);
			m_componentFlags = ((VssComponentFlags)info->dwComponentFlags);
#endif
			m_fileCount = (info->cFileCount);
			m_databaseFileCount = (info->cDatabases);
			m_databaseLogFileCount = (info->cLogFiles);
			if (info->pbIcon != 0)
			{
				m_icon = gcnew array<byte>(info->cbIcon);
				System::Runtime::InteropServices::Marshal::Copy((IntPtr)info->pbIcon, m_icon, 0, info->cbIcon);
			}
		}
		finally
		{
			CheckCom(m_component->FreeComponentInfo(info));
		}
	}

	VssWMComponent::~VssWMComponent()
	{
		this->!VssWMComponent();
	}

	VssWMComponent::!VssWMComponent()
	{
		if (m_component != 0)
		{
			m_component->Release();
			m_component = 0;
		}
	}

	VssComponentType VssWMComponent::Type::get()
	{
		return m_type; 
	}

	String^ VssWMComponent::LogicalPath::get()
	{
		return m_logicalPath; 
	}

	String^ VssWMComponent::ComponentName::get()
	{
		return m_componentName;
	}

	String^ VssWMComponent::Caption::get()
	{
		return m_caption;
	}

	array<byte>^ VssWMComponent::GetIcon()
	{
		return m_icon;
	}

	bool VssWMComponent::RestoreMetadata::get()
	{
		return m_restoreMetadata;
	}

	bool VssWMComponent::NotifyOnBackupComplete::get()
	{
		return m_notifyOnBackupComplete;
	}

	bool VssWMComponent::Selectable::get()
	{
		return m_selectable;
	}

	bool VssWMComponent::SelectableForRestore::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		return m_selectableForRestore;
#else
		return false;
#endif
	}

	VssComponentFlags VssWMComponent::ComponentFlags::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		return m_componentFlags;
#else
		return VssComponentFlags::None;
#endif
	}

	IList<VssWMFileDescriptor^>^ VssWMComponent::Files::get()
	{
		if (m_files != nullptr)
			return m_files;

		List<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(m_fileCount);

		for (UINT i = 0; i < m_fileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(m_component->GetFile(i, &filedesc));
			list->Add(CreateVssWMFileDescriptor(filedesc));
		}
		m_files = list->AsReadOnly();
		return m_files;
	}

	IList<VssWMFileDescriptor^>^ VssWMComponent::DatabaseFiles::get()
	{
		if (m_databaseFiles != nullptr)
			return m_databaseFiles;

		List<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(m_databaseFileCount);

		for (UINT i = 0; i < m_databaseFileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(m_component->GetDatabaseFile(i, &filedesc));
			list->Add(CreateVssWMFileDescriptor(filedesc));
		}
		m_databaseFiles = list->AsReadOnly();
		return m_databaseFiles;
	}

	IList<VssWMFileDescriptor^>^ VssWMComponent::DatabaseLogFiles::get()
	{
		if (m_databaseLogFiles != nullptr)
			return m_databaseLogFiles;

		List<VssWMFileDescriptor^>^ list = gcnew List<VssWMFileDescriptor^>(m_databaseLogFileCount);

		for (UINT i = 0; i < m_databaseLogFileCount; i++)
		{
			IVssWMFiledesc *filedesc;
			CheckCom(m_component->GetDatabaseLogFile(i, &filedesc));
			list->Add(CreateVssWMFileDescriptor(filedesc));
		}
		m_databaseLogFiles = list->AsReadOnly();
		return m_databaseLogFiles;
	}


	IList<VssWMDependency^>^ VssWMComponent::Dependencies::get()
	{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		if (m_dependencies != nullptr)
			return m_dependencies;

		List<VssWMDependency^>^ list = gcnew List<VssWMDependency^>(m_dependencyCount);

		for (UINT i = 0; i < m_dependencyCount; i++)
		{
			IVssWMDependency *dependency;
			CheckCom(m_component->GetDependency(i, &dependency));
			list->Add(CreateVssWMDependency(dependency));
		}
		m_dependencies = list->AsReadOnly();
		return m_dependencies;
#else
		return gcnew List<VssWMDependency^>();
#endif
	}


}
} }