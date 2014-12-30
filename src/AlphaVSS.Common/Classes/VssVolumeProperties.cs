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
   /// The <see cref="VssVolumeProperties"/> class contains the properties of a shadow copy source volume.
   /// </summary>
   [Serializable]
   public class VssVolumeProperties
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssVolumeProperties"/> class.
      /// </summary>
      /// <param name="volumeName">Name of the volume.</param>
      /// <param name="volumeDisplayName">Display name of the volume.</param>
      public VssVolumeProperties(string volumeName, string volumeDisplayName)
      {
         VolumeName = volumeName;
         VolumeDisplayName = volumeDisplayName;
      }

      #region Public Properties

      /// <summary>
      /// Gets the volume name, in <c>\\?\Volume{GUID}\</c> format.
      /// </summary>
      /// <value>The volume name, in <c>\\?\Volume{GUID}\</c> format.</value>
      public string VolumeName { get; private set; }

      /// <summary>
      /// Gets a string that can be displayed to the user containing the shortest mount point (for example C:\).
      /// </summary>
      /// <value>A string that can be displayed to the user containing the shortest mount point (for example C:\).</value>
      public string VolumeDisplayName { get; private set; }

      #endregion
   }
}
