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

#include "VssSnapshotProperties.h"


namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssSnapshotProperties^ VssSnapshotProperties::Adopt(VSS_SNAPSHOT_PROP *pProp)
	{
		try
		{
			return gcnew VssSnapshotProperties(*pProp);
		}
		finally
		{
			::VssFreeSnapshotProperties(pProp);
		}
	}

	VssSnapshotProperties::VssSnapshotProperties(const VSS_SNAPSHOT_PROP &prop)
		: mSnapshotId(ToGuid(prop.m_SnapshotId)),
		  mSnapshotSetId(ToGuid(prop.m_SnapshotSetId)),
		  mSnapshotsCount(prop.m_lSnapshotsCount),
		  mSnapshotDeviceObject(gcnew String(prop.m_pwszSnapshotDeviceObject)),
		  mOriginalVolumeName(gcnew String(prop.m_pwszOriginalVolumeName)),
		  mOriginatingMachine(gcnew String(prop.m_pwszOriginatingMachine)),
		  mServiceMachine(gcnew String(prop.m_pwszServiceMachine)),
		  mExposedName(gcnew String(prop.m_pwszExposedName)),
		  mExposedPath(gcnew String(prop.m_pwszExposedPath)),
		  mProviderId(ToGuid(prop.m_ProviderId)),
		  mSnapshotAttributes((VssVolumeSnapshotAttributes)prop.m_lSnapshotAttributes),
		  mCreationTimestamp(ToDateTime(prop.m_tsCreationTimestamp)),
		  mStatus((VssSnapshotState)prop.m_eStatus)
	{
	}



	System::Guid VssSnapshotProperties::SnapshotId::get()
	{
		return mSnapshotId;
	}

	 System::Guid VssSnapshotProperties::SnapshotSetId::get()
	 {
		 return mSnapshotSetId;
	 }

	long VssSnapshotProperties::SnapshotsCount::get()
	{
		return mSnapshotsCount;
	}

	System::String^ VssSnapshotProperties::SnapshotDeviceObject::get()
	{
		return mSnapshotDeviceObject;
	}

    System::String^ VssSnapshotProperties::OriginalVolumeName::get()
	{
		return mOriginalVolumeName;
	}

	System::String^ VssSnapshotProperties::OriginatingMachine::get()
	{
		return mOriginatingMachine;
	}

	System::String^ VssSnapshotProperties::ServiceMachine::get()
	{
		return mServiceMachine;
	}

	System::String^ VssSnapshotProperties::ExposedName::get()
	{
		return mExposedName;
	}

	System::String^ VssSnapshotProperties::ExposedPath::get()
	{
		return mExposedPath;
	}
	
	System::Guid VssSnapshotProperties::ProviderId::get()
	{
		return mProviderId;
	}

	VssVolumeSnapshotAttributes ^VssSnapshotProperties::SnapshotAttributes::get()
	{
		return mSnapshotAttributes;
	}

	System::DateTime VssSnapshotProperties::CreationTimestamp::get()
	{
		return mCreationTimestamp;
	}

	VssSnapshotState^ VssSnapshotProperties::Status::get()
	{
		return mStatus;
	}
}
} }
