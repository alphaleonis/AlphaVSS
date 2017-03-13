
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Contains information about a volume's shadow copy protection level.
   /// </summary>
   [Serializable]
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
         ProtectionLevel = protectionLevel;
         VolumeIsOfflineForProtection = volumeIsOfflineForProtection;
         ProtectionFault = protectionFault;
         FailureStatus = failureStatus;
         VolumeHasUnusedDiffArea = volumeHasUnusedDiffArea;
      }

      #region Public Properties

      /// <summary>
      /// Gets the target protection level for the volume.
      /// </summary>
      /// <value>The target protection level for the volume.</value>
      public VssProtectionLevel ProtectionLevel { get; private set; }

      /// <summary>
      /// Gets a value indicating whether the volume is offline due to a protection fault.
      /// </summary>
      /// <value>
      /// 	<c>true</c> if the volume is offline due to a protection fault; otherwise, <c>false</c>.
      /// </value>
      public bool VolumeIsOfflineForProtection { get; private set; }

      /// <summary>
      /// Gets a value that describes the shadow copy protection fault that caused the volume to go offline.
      /// </summary>
      /// <value>A value that describes the shadow copy protection fault that caused the volume to go offline.</value>
      public VssProtectionFault ProtectionFault { get; private set; }

      /// <summary>
      /// Gets the internal failure status code.
      /// </summary>
      /// <value>The internal failure status code.</value>
      public UInt32 FailureStatus { get; private set; }

      /// <summary>
      /// Gets a value indicating whether the volume has unused shadow copy storage area files or not.
      /// </summary>
      /// <value>
      /// 	<c>true</c> if the volume has unused shadow copy storage area files; otherwise, <c>false</c>.
      /// </value>
      public bool VolumeHasUnusedDiffArea { get; private set; }

      #endregion
   }
}
