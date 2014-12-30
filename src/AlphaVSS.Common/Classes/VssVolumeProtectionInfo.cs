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
