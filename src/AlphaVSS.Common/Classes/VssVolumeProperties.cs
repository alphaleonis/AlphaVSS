

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
