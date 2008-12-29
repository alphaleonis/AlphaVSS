using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Alphaleonis.Win32.Vss
{
    public static class VssUtils
    {
        public static IVssImplementation LoadImplementation(string path)
        {
            Assembly assembly = Assembly.LoadFrom("AlphaVSS.Win2008.Debug.x64.dll");
            return (IVssImplementation)assembly.CreateInstance("Alphaleonis.Win32.Vss.VssImplementation");
        }
    }
}
