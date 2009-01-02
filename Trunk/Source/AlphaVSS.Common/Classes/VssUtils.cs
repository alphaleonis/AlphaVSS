using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace Alphaleonis.Win32.Vss
{
    public static class VssUtils
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static string GetPlatformSpecificAssemblyShortName()
        {
            string winVer;
            switch (OperatingSystemInfo.OSVersionName)
            {
                case OSVersionName.Windows2000:
                    throw new UnsupportedOperatingSystemException("Windows 2000 is not supported");
                case OSVersionName.WindowsXP:
                    winVer = "WinXP";
                    break;
                case OSVersionName.WindowsServer2003:
                    winVer = "Win2003";
                    break;
                case OSVersionName.WindowsVista:
                    winVer = "Win2008";
                    break;
                case OSVersionName.WindowsServer2008:
                    winVer = "Win2003";
                    break;
                case OSVersionName.Unknown:
                default:
                    throw new UnsupportedOperatingSystemException("Failed to detect running operating system, or current operating system not supported.");
            }

            string archName;
            switch (OperatingSystemInfo.ProcessorArchitecture)
            {
                case ProcessorArchitecture.X86:
                    archName = "x86";
                    break;
                case ProcessorArchitecture.IA64:
                    throw new UnsupportedOperatingSystemException("IA64 architecture is not supported.");
                case ProcessorArchitecture.X64:
                    archName = "x64";
                    break;
                case ProcessorArchitecture.Unknown:
                default:
                    throw new UnsupportedOperatingSystemException("Failed to detect architecture of running operating system.");
            }
#if DEBUG
            return "AlphaVSS." + winVer + ".Debug." + archName;
#else
            return "AlphaVSS." + winVer + "." + archName;
#endif
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public static AssemblyName GetPlatformSpecificAssemblyName()
        {
            return new AssemblyName(
                GetPlatformSpecificAssemblyShortName() + 
                ", Version=" + 
                Assembly.GetExecutingAssembly().GetName().Version.ToString() + 
                ", PublicKeyToken=3033cf2dbd31cad3");
        }
       
        public static IVssImplementation LoadImplementation()
        {
            Assembly assembly = Assembly.Load(GetPlatformSpecificAssemblyName());           
            return (IVssImplementation)assembly.CreateInstance("Alphaleonis.Win32.Vss.VssImplementation");
        }

        public static IVssImplementation LoadImplementation(AppDomain domain)
        {
            domain.Load(GetPlatformSpecificAssemblyName());            
            ObjectHandle handle = domain.CreateInstance(GetPlatformSpecificAssemblyName().ToString(), "Alphaleonis.Win32.Vss.VssImplementation");
            return (IVssImplementation)handle.Unwrap();
        }
    }
}
