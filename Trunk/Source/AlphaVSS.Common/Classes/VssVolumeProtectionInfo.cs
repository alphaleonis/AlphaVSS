using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public class VssVolumeProtectionInfo
    {
        public VssVolumeProtectionInfo(VssProtectionLevel protectionLevel, bool volumeIsOfflineForProtection, VssProtectionFault protectionFault, UInt32 failureStatus, bool volumeHasUnusedDiffArea)
        {
            mProtectionLevel = protectionLevel;
            mVolumeIsOfflineForProtection = volumeIsOfflineForProtection;
            mProtectionFault = protectionFault;
            mFailureStatus = failureStatus;
            mVolumeHasUnusedDiffArea = volumeHasUnusedDiffArea;
        }

        public VssProtectionLevel ProtectionLevel { get { return mProtectionLevel; } }
        public bool VolumeIsOfflineForProtection { get { return mVolumeIsOfflineForProtection; } }
        public VssProtectionFault ProtectionFault { get { return mProtectionFault; } }
        public UInt32 FailureStatus { get { return mFailureStatus; } }
        public bool VolumeHasUnusedDiffArea { get { return mVolumeHasUnusedDiffArea; } }

        private VssProtectionLevel mProtectionLevel;
        private bool mVolumeIsOfflineForProtection;
        private VssProtectionFault mProtectionFault;
        private UInt32 mFailureStatus;
        private bool mVolumeHasUnusedDiffArea;
    }
}
