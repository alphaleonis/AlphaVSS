using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// The <see cref="VssDiffAreaProperties"/> structure describes associations between volumes containing the original file data 
    /// and volumes containing the shadow copy storage area (also known as the diff area).
    /// </summary>
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
        public VssDiffAreaProperties(string volumeName, string diffAreaVolumeName, Int64 maximumDiffSpace, Int64 allocatedDiffSpace, Int64 usedDiffSpace)
        {
            mVolumeName = volumeName;
            mDiffAreaVolumeName = diffAreaVolumeName;
            mMaximumDiffSpace = maximumDiffSpace;
            mAllocatedDiffSpace = allocatedDiffSpace;
            mUsedDiffSpace = usedDiffSpace;
        }

        /// <summary>
        /// Gets the original volume name.
        /// </summary>
        /// <value>The original volume name.</value>
        public string VolumeName { get { return mVolumeName; } }

        /// <summary>
        /// Gets the shadow copy storage area volume name.
        /// </summary>
        /// <value>The shadow copy storage area volume name.</value>
        public string DiffAreaVolumeName { get { return mDiffAreaVolumeName; } }

        /// <summary>
        /// Gets the maximum space used on the shadow copy storage area volume for this association.
        /// </summary>
        /// <value>The maximum space used on the shadow copy storage area volume for this association.</value>
        public Int64 MaximumDiffSpace { get { return mMaximumDiffSpace; } }

        /// <summary>
        /// Gets the allocated space on the shadow copy storage area volume by this association. 
        /// This must be less than or equal to <see cref="MaximumDiffSpace"/>.
        /// </summary>
        /// <value>The allocated space on the shadow copy storage area volume by this association.</value>
        public Int64 AllocatedDiffSpace { get { return mAllocatedDiffSpace; } }

        /// <summary>
        /// Gets the used space from the allocated area. This must be less than or equal to <see cref="AllocatedDiffSpace"/>.
        /// </summary>
        /// <value>The the used space from the allocated area.</value>
        public Int64 UsedDiffSpace { get { return mUsedDiffSpace; } }

        private string mVolumeName;
        private string mDiffAreaVolumeName;
        private Int64 mMaximumDiffSpace;
        private Int64 mAllocatedDiffSpace;
        private Int64 mUsedDiffSpace;
    }
}
