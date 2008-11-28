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


#include "VssVolumeSnapshotAttributes.h"
#include "VssSnapshotState.h"
#include "IVssObjectProp.h"
#include <vss.h>

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	/// <summary>The <see cref="VssSnapshotProp"/> class contains the properties of a shadow copy or shadow copy set.</summary>
	public ref class VssSnapshotProp : IVssObjectProp
	{
	public:
		/// <summary>Indicates the type of this property object as <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.Snapshot"/> or 
		/// <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.Snapshot"/>.</summary>
		/// <value>The value of the <see cref="VssObjectType"/> enumeration representing the type of data contained in this instance.</value>
		virtual property VssObjectType Type { VssObjectType get(); }

		/// <summary>A <see cref="Guid" /> uniquely identifying the shadow copy identifier.</summary>
		property Guid SnapshotId { Guid get(); };

		/// <summary>A <see cref="Guid" /> uniquely identifying the shadow copy set containing the shadow copy.</summary>
		property Guid SnapshotSetId { Guid get(); };

		/// <summary>
		/// 	<para>
		/// 		Number of volumes included with the shadow copy in the shadow copy set when it was created. 
		/// 		Because it is possible for applications to release individual shadow copies without releasing the shadow copy 
		/// 		set, at any given time the number of shadow copies in the shadow copy set may be less than <see cref="SnapshotsCount" />
		/// 	</para>
		/// 	<para>
		/// 		The maximum number of shadow-copied volumes permitted in a shadow copy set is 64.
		/// 	</para>
		/// </summary>
		property long SnapshotsCount { long get(); }

		/// <summary>
		/// 	<para>
		/// 		The name of the device object for the shadow copy of the volume. The device object can be thought of as 
		/// 		the root of a shadow copy of a volume. Requesters will use this device name when accessing files on a 
		/// 		shadow-copied volume that it needs to work with.
		/// 	</para>
		/// 	<para>
		/// 		The device name does not contain a trailing "\".
		/// 	</para>
		/// </summary>
		property String^ SnapshotDeviceObject { String^ get(); }

		/// <summary>The name of the volume that had been shadow copied.</summary>
		property String^ OriginalVolumeName { String^ get(); }

		/// <summary>The name of the machine containing the original volume.</summary>
		property String^ OriginatingMachine { String^ get(); }

		/// <summary>The name of the machine running the Volume Shadow Copy Service that created the shadow copy.</summary>
		property String^ ServiceMachine { String^ get(); }

		/// <summary>The name of the shadow copy when it is exposed. This is a drive letter or mount point (if the shadow copy is exposed as a local volume), or a share name. </summary>
		property String^ ExposedName { String^ get(); }

		/// <summary>The portion of the shadow copy of a volume made available if it is exposed as a share.</summary>
		property String^ ExposedPath { String^ get(); }

		/// <summary>A <see cref="Guid"/> uniquely identifying the provider used to create this shadow copy.</summary>
		property Guid ProviderId { Guid get(); }

		/// <summary>
		///		The attributes of the shadow copy expressed as a bit mask (or bitwise OR) of members of 
		///		the <see cref="VssVolumeSnapshotAttributes"/> enumeration.
		/// </summary>
		property VssVolumeSnapshotAttributes ^SnapshotAttributes { VssVolumeSnapshotAttributes^ get(); }
		
		/// <summary>Time stamp indicating when the shadow copy was created. The exact time is determined by the provider.</summary>
		property DateTime CreationTimestamp { DateTime get(); }

		/// <summary>Current shadow copy creation status. See <see cref="VssSnapshotState"/>.</summary>
		property VssSnapshotState^ Status { VssSnapshotState^ get(); }

	internal:
		static VssSnapshotProp^ Adopt(VSS_SNAPSHOT_PROP *pProp);
	private:
		VssSnapshotProp(const VSS_SNAPSHOT_PROP &snap);

		Guid mSnapshotId;
		Guid mSnapshotSetId;
		long mSnapshotsCount;
		String^ mSnapshotDeviceObject;
		String^ mOriginalVolumeName;  
		String^ mOriginatingMachine;  
		String^ mServiceMachine;  
		String^ mExposedName;  
		String^ mExposedPath;  
		Guid mProviderId;
		VssVolumeSnapshotAttributes^ mSnapshotAttributes;
		DateTime mCreationTimestamp;
		VssSnapshotState^ mStatus;
	};
}
} }
