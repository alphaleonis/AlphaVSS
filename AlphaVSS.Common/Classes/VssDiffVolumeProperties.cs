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
   /// The <see cref="VssDiffVolumeProperties"/> structure describes a shadow copy storage area volume.
   /// </summary>
   [Serializable]
   public class VssDiffVolumeProperties
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssDiffVolumeProperties"/> class.
      /// </summary>
      /// <param name="volumeName">Name of the volume.</param>
      /// <param name="volumeDisplayName">Display name of the volume.</param>
      /// <param name="volumeFreeSpace">The volume free space.</param>
      /// <param name="volumeTotalSpace">The volume total space.</param>
      public VssDiffVolumeProperties(string volumeName, string volumeDisplayName, Int64 volumeFreeSpace, Int64 volumeTotalSpace)
      {
         VolumeName = volumeName;
         VolumeDisplayName = volumeDisplayName;
         VolumeFreeSpace = volumeFreeSpace;
         VolumeTotalSpace = volumeTotalSpace;
      }

      #region Properties

      /// <summary>
      /// Gets the shadow copy storage area volume name, in <c>\\?\Volume{GUID}\</c> format.
      /// </summary>
      /// <value>The shadow copy storage area volume name, in <c>\\?\Volume{GUID}\</c> format.</value>
      public string VolumeName { get; private set; }

      /// <summary>
      /// Gets a string that can be displayed to a user, for example <c>C:\</c>, for the shadow copy storage area volume.
      /// </summary>
      /// <value>A string that can be displayed to a user, for example <c>C:\</c>, for the shadow copy storage area volume.</value>
      public string VolumeDisplayName { get; private set; }

      /// <summary>
      /// Gets the free space, in bytes, on the shadow copy storage area volume.
      /// </summary>
      /// <value>The free space, in bytes, on the shadow copy storage area volume.</value>
      public Int64 VolumeFreeSpace { get; private set; }

      /// <summary>
      /// Gets the total space, in bytes, on the shadow copy storage area volume.
      /// </summary>
      /// <value>The total space, in bytes, on the shadow copy storage area volume..</value>
      public Int64 VolumeTotalSpace { get; private set; }

      #endregion
   }
}
