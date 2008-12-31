using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public interface IVssManagementObjectProperties
    {
        VssManagementObjectType Type { get; }
    }
}
