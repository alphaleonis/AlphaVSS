
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// The <see cref="VssDiffAreaProperties"/> structure describes associations between volumes containing the original file data 
   /// and volumes containing the shadow copy storage area (also known as the diff area).
   /// </summary>
   [Serializable]
   public class VssDiffAreaProperties
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssDiffAreaProperties"/> class.
      /// </summary>
      /// <param name="volumeName">Name of the volume.</param>
      /// <param name="diffAreaVolumeName">Name of the diff area volume.</param>
      /// <param name="maximumDiffSpace">The maximum diff space.</param>
      /// <param name="allocatedDiffSpace">The allocated diff space.</param>
      /// <param name="usedDiffSpace">The used diff space.</param>
      public VssDiffAreaProperties(string volumeName, string diffAreaVolumeName, long maximumDiffSpace, long allocatedDiffSpace, long usedDiffSpace)
      {
         VolumeName = volumeName;
         DiffAreaVolumeName = diffAreaVolumeName;
         MaximumDiffSpace = maximumDiffSpace;
         AllocatedDiffSpace = allocatedDiffSpace;
         UsedDiffSpace = usedDiffSpace;
      }

      #region Properties

      /// <summary>
      /// Gets the original volume name.
      /// </summary>
      /// <value>The original volume name.</value>
      public string VolumeName { get; private set; }

      /// <summary>
      /// Gets the shadow copy storage area volume name.
      /// </summary>
      /// <value>The shadow copy storage area volume name.</value>
      public string DiffAreaVolumeName { get; private set; }

      /// <summary>
      /// Gets the maximum space used on the shadow copy storage area volume for this association.
      /// </summary>
      /// <value>The maximum space used on the shadow copy storage area volume for this association.</value>
      public long MaximumDiffSpace { get; private set; }

      /// <summary>
      /// Gets the allocated space on the shadow copy storage area volume by this association. 
      /// This must be less than or equal to <see cref="MaximumDiffSpace"/>.
      /// </summary>
      /// <value>The allocated space on the shadow copy storage area volume by this association.</value>
      public long AllocatedDiffSpace { get; private set; }

      /// <summary>
      /// Gets the used space from the allocated area. This must be less than or equal to <see cref="AllocatedDiffSpace"/>.
      /// </summary>
      /// <value>The the used space from the allocated area.</value>
      public long UsedDiffSpace { get; private set; }

      #endregion
   }
}
