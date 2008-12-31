using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// The <see cref="IVssManagementObjectProperties"/> is an interface implemented by all classes 
    /// that defines the properties of a volume, shadow copy storage volume, or a shadow copy storage area.
    /// </summary>
    public interface IVssManagementObjectProperties
    {
        /// <summary>
        /// Gets the type of this management object properties instance.
        /// </summary>
        /// <value>The type of this management object properties instance.</value>
        VssManagementObjectType Type { get; }
    }
}
