using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public enum VssManagementObjectType
    {
        /// <summary>
        /// The object type is unknown.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The object is a volume to be shadow copied.
        /// </summary>
        Volume = 1,
        /// <summary>
        /// The object is a volume to hold a shadow copy storage area.
        /// </summary>
        DiffVolume = 2,
        /// <summary>
        /// The object is an association between a volume to be shadow copied and a volume to hold the shadow copy storage area.
        /// </summary>
        DiffArea = 3
    }
}
