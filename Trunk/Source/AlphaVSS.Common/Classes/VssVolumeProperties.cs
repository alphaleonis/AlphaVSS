using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// The <see cref="VssVolumeProperties"/> class contains the properties of a shadow copy source volume.
    /// </summary>
    public class VssVolumeProperties : IVssManagementObjectProperties
    {
        public VssManagementObjectType Type
        {
            get { return VssManagementObjectType.Volume; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VssVolumeProperties"/> class.
        /// </summary>
        /// <param name="volumeName">Name of the volume.</param>
        /// <param name="volumeDisplayName">Display name of the volume.</param>
        public VssVolumeProperties(string volumeName, string volumeDisplayName)
        {
            mVolumeName = volumeName;
            mVolumeDisplayName = volumeDisplayName;
        }

        /// <summary>
        /// Gets the volume name, in <c>\\?\Volume{GUID}\</c> format.
        /// </summary>
        /// <value>The volume name, in <c>\\?\Volume{GUID}\</c> format.</value>
        public string VolumeName { get { return mVolumeName; } }

        /// <summary>
        /// Gets a string that can be displayed to the user containing the shortest mount point (for example C:\).
        /// </summary>
        /// <value>A string that can be displayed to the user containing the shortest mount point (for example C:\).</value>
        public string VolumeDisplayName { get { return mVolumeDisplayName; } }

        private string mVolumeName;
        private string mVolumeDisplayName;
    }
}
