using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public class VssDiffAreaProperties : IVssManagementObjectProperties
    {
        public VssManagementObjectType Type { get { return VssManagementObjectType.DiffArea; } }

        public VssDiffAreaProperties(string volumeName, string diffAreaVolumeName, Int64 maximumDiffSpace, Int64 allocatedDiffSpace, Int64 usedDiffSpace)
        {
            mVolumeName = volumeName;
            mDiffAreaVolumeName = diffAreaVolumeName;
            mMaximumDiffSpace = maximumDiffSpace;
            mAllocatedDiffSpace = allocatedDiffSpace;
            mUsedDiffSpace = usedDiffSpace;
        }

        public string VolumeName { get { return mVolumeName; } }
        public string DiffAreaVolumeName { get { return mDiffAreaVolumeName; } }
        public Int64 MaximumDiffSpace { get { return mMaximumDiffSpace; } }
        public Int64 AllocatedDiffSpace { get { return mAllocatedDiffSpace; } }
        public Int64 UsedDiffSpace { get { return mUsedDiffSpace; } }

        private string mVolumeName;
        private string mDiffAreaVolumeName;
        private Int64 mMaximumDiffSpace;
        private Int64 mAllocatedDiffSpace;
        private Int64 mUsedDiffSpace;
    }
}
