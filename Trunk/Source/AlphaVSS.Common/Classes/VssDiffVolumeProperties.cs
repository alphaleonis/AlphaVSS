using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public class VssDiffVolumeProperties : IVssManagementObjectProperties
    {
        public VssManagementObjectType  Type { get { return VssManagementObjectType.DiffVolume; } }

        public VssDiffVolumeProperties(string volumeName, string volumeDisplayName, Int64 volumeFreeSpace, Int64 volumeTotalSpace)
        {
            mVolumeName = volumeName;
            mVolumeDisplayName = volumeDisplayName;
            mVolumeFreeSpace = volumeFreeSpace;
            mVolumeTotalSpace = volumeTotalSpace;
        }

        public string VolumeName { get { return mVolumeName; } }
        public string VolumeDisplayName { get { return mVolumeDisplayName; } }
        public Int64 VolumeFreeSpace { get { return mVolumeFreeSpace; } }
        public Int64 VolumeTotalSpace { get { return mVolumeTotalSpace; } }

        private string mVolumeName;
        private string mVolumeDisplayName;
        private Int64 mVolumeFreeSpace;
        private Int64 mVolumeTotalSpace;

    }
}
