using System;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Contains information about a volume's shadow copy protection level.
    /// </summary>
    public class VssVolumeProtectionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VssVolumeProtectionInfo"/> class.
        /// </summary>
        /// <param name="protectionLevel">The protection level.</param>
        /// <param name="volumeIsOfflineForProtection">if set to <c>true</c> the volume is offline for protection.</param>
        /// <param name="protectionFault">The protection fault.</param>
        /// <param name="failureStatus">The failure status.</param>
        /// <param name="volumeHasUnusedDiffArea">if set to <c>true</c> the volume has unused diff area.</param>
        public VssVolumeProtectionInfo(VssProtectionLevel protectionLevel, bool volumeIsOfflineForProtection, VssProtectionFault protectionFault, UInt32 failureStatus, bool volumeHasUnusedDiffArea)
        {
            mProtectionLevel = protectionLevel;
            mVolumeIsOfflineForProtection = volumeIsOfflineForProtection;
            mProtectionFault = protectionFault;
            mFailureStatus = failureStatus;
            mVolumeHasUnusedDiffArea = volumeHasUnusedDiffArea;
        }

        /// <summary>
        /// Gets the target protection level for the volume.
        /// </summary>
        /// <value>The target protection level for the volume.</value>
        public VssProtectionLevel ProtectionLevel { get { return mProtectionLevel; } }

        /// <summary>
        /// Gets a value indicating whether the volume is offline due to a protection fault.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the volume is offline due to a protection fault; otherwise, <c>false</c>.
        /// </value>
        public bool VolumeIsOfflineForProtection { get { return mVolumeIsOfflineForProtection; } }
        
        /// <summary>
        /// Gets a value that describes the shadow copy protection fault that caused the volume to go offline.
        /// </summary>
        /// <value>A value that describes the shadow copy protection fault that caused the volume to go offline.</value>
        public VssProtectionFault ProtectionFault { get { return mProtectionFault; } }

        /// <summary>
        /// Gets the internal failure status code.
        /// </summary>
        /// <value>The internal failure status code.</value>
        public UInt32 FailureStatus { get { return mFailureStatus; } }

        /// <summary>
        /// Gets a value indicating whether the volume has unused shadow copy storage area files or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the volume has unused shadow copy storage area files; otherwise, <c>false</c>.
        /// </value>
        public bool VolumeHasUnusedDiffArea { get { return mVolumeHasUnusedDiffArea; } }

        private VssProtectionLevel mProtectionLevel;
        private bool mVolumeIsOfflineForProtection;
        private VssProtectionFault mProtectionFault;
        private UInt32 mFailureStatus;
        private bool mVolumeHasUnusedDiffArea;
    }
}
