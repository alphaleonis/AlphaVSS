using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Permissions;
using System.Globalization;

namespace Alphaleonis.Win32.Vss
{
    public enum OSVersionName
    {
        Windows2000 = 0,
        WindowsXP = 1,
        WindowsServer2003 = 2,
        WindowsVista = 3,
        WindowsServer2008 = 4,
        Unknown = 0xffff
    }

    public enum ProcessorArchitecture : ushort
    {
        X86 = 0x00,
        IA64 = 0x06,
        X64 = 0x09,
        Unknown = 0xFFFF,
    }

    public static class OperatingSystemInfo
    {
        public static OSVersionName OSVersionName
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            get 
            { 
                if (mServicePackVersion == null)
                    UpdateData();
                return mOSVersionName; 
            }
        }

        public static Version OSVersion
        {
            get { return mOSVersion; }
        }

        public static Version ServicePackVersion
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            get 
            { 
                if (mServicePackVersion == null)
                    UpdateData();
                return mServicePackVersion; 
            }
        }

        public static ProcessorArchitecture ProcessorArchitecture
        {
            [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
            get 
            { 
                if (mServicePackVersion == null)
                    UpdateData();
                return mProcessorArchitecture; 
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
        public static bool IsWow64Process()
        {
            IntPtr processHandle = System.Diagnostics.Process.GetCurrentProcess().Handle;
            bool value = false;
            if (!NativeMethods.IsWow64Process(processHandle, out value))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }
            return value;
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsAtLeast(OSVersionName os)
        {
            Debug.Assert(os != OSVersionName.Unknown);
            return OSVersionName >= os;
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsAtLeast(OSVersionName os, int spMajorVersion)
        {
            Debug.Assert(os != OSVersionName.Unknown);
            return IsWithSPAtLeast(os, spMajorVersion) || OSVersionName > os;
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static bool IsWithSPAtLeast(OSVersionName os, int spMajorVersion)
        {
            Debug.Assert(os != OSVersionName.Unknown);
            return (OSVersionName == os && ServicePackVersion.Major >= spMajorVersion);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Require(OSVersionName os)
        {
            Debug.Assert(os != OSVersionName.Unknown);
            if (os != OSVersionName)
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0DetectedOperatingSystemWas1, ToString(os), ToString(OSVersionName)));
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void Require(OSVersionName os, int spMajorVersion)
        {
            Debug.Assert(os != OSVersionName.Unknown);
            if (os != OSVersionName || spMajorVersion != ServicePackVersion.Major)
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires01DetectedOperatingSystemWas23, 
                    ToString(os), SpToString(spMajorVersion), ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RequireWithSPAtLeast(OSVersionName os, int spMajorVersion)
        {
            if (!IsWithSPAtLeast(os, spMajorVersion))
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0WithAtLeastServicePack1DetectedOperatingSystemWas23,
                    ToString(os), spMajorVersion, ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RequireWithSPAtLeast(OSVersionName os, int spMajorVersion, OSVersionName os2, int spMajorVersion2)
        {
            if (!IsWithSPAtLeast(os, spMajorVersion) && !IsWithSPAtLeast(os2, spMajorVersion2))
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0WithAtLeastServicePack1Or2WithAtLeastServicePack3DetectedOperatingSystemWas45,
                    ToString(os), spMajorVersion, ToString(os2), spMajorVersion2, ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RequireAtLeast(OSVersionName os)
        {
            if (!IsAtLeast(os))
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0OrLaterDetectedOperatingSystemWas1, 
                    ToString(os), ToString(OSVersionName)));
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
        public static void RequireAtLeast(OSVersionName os, int spMajorVersion)
        {
            if (!IsAtLeast(os, spMajorVersion))
                throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires01OrLaterDetectedOperatingSystemWas23,
                    ToString(os), SpToString(spMajorVersion), ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
        }

        #region Private members

        private static string ToString(OSVersionName name)
        {
            switch (name)
            {
                case OSVersionName.Windows2000:
                    return "Windows 2000";
                case OSVersionName.WindowsXP:
                    return "Windows XP";
                case OSVersionName.WindowsServer2003:
                    return "Windows Server 2003";
                case OSVersionName.WindowsVista:
                    return "Windows Vista";
                case OSVersionName.WindowsServer2008:
                    return "Windows Server 2008";
                default:
                    return "Unknown";
            }
        }

        private static string SpToString(int spMajorVersion)
        {
            if (spMajorVersion == 0)
                return String.Empty;

            return String.Format(" SP{0}", spMajorVersion);
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode=true)]
        private static void UpdateData()
        {
            NativeMethods.OSVERSIONINFOEX info = new NativeMethods.OSVERSIONINFOEX();
            info.dwOSVersionInfoSize = Marshal.SizeOf(info);

            NativeMethods.SYSTEM_INFO sysInfo = new NativeMethods.SYSTEM_INFO();

            NativeMethods.GetSystemInfo(out sysInfo);
            if (!NativeMethods.GetVersionExW(ref info))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
            }

            Debug.Assert(info.dwMajorVersion == Environment.OSVersion.Version.Major);
            Debug.Assert(info.dwMinorVersion == Environment.OSVersion.Version.MajorRevision);
            Debug.Assert(info.dwBuildNumber == Environment.OSVersion.Version.Minor);

            mProcessorArchitecture = (ProcessorArchitecture)sysInfo.processorArchitecture;

            mServicePackVersion = new Version(info.wServicePackMajor, info.wServicePackMinor);

            if (info.dwMajorVersion == 6) // Vista or 2008
            {
                if (info.wProductType == NativeMethods.VER_NT_WORKSTATION) // Vista
                {
                    mOSVersionName = OSVersionName.WindowsVista;
                }
                else
                {
                    mOSVersionName = OSVersionName.WindowsServer2008;
                }
            }
            else if (info.dwMajorVersion == 5)
            {
                if (info.dwMinorVersion == 0)
                {
                    mOSVersionName = OSVersionName.Windows2000;
                }
                if (info.dwMinorVersion == 1)
                {
                    mOSVersionName = OSVersionName.WindowsXP;
                }
                else if (info.dwMinorVersion == 2)
                {
                    if (info.wProductType == NativeMethods.VER_NT_WORKSTATION && mProcessorArchitecture == ProcessorArchitecture.X64)
                    {
                        mOSVersionName = OSVersionName.WindowsXP;
                    }
                    else if (info.wProductType != NativeMethods.VER_NT_WORKSTATION)
                    {
                        mOSVersionName = OSVersionName.WindowsServer2003;
                    }
                    else
                    {
                        mOSVersionName = OSVersionName.Unknown;
                    }
                }
                else
                {
                    mOSVersionName = OSVersionName.Unknown;
                }
            }
        }

        private static OSVersionName mOSVersionName = OSVersionName.Unknown;
        private static Version mOSVersion = Environment.OSVersion.Version;
        private static Version mServicePackVersion;
        private static ProcessorArchitecture mProcessorArchitecture;

        #endregion

        #region P/Invoke members

        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct OSVERSIONINFOEX
            {
                public int dwOSVersionInfoSize;
                public int dwMajorVersion;
                public int dwMinorVersion;
                public int dwBuildNumber;
                public int dwPlatformId;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
                public string szCSDVersion;
                public UInt16 wServicePackMajor;
                public UInt16 wServicePackMinor;
                public UInt16 wSuiteMask;
                public byte wProductType;
                public byte wReserved;
            }

            [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetVersionExW(ref OSVERSIONINFOEX osvi);

            [StructLayout(LayoutKind.Sequential)]
            public struct SYSTEM_INFO
            {
                public ushort processorArchitecture;
                ushort reserved;
                public uint pageSize;
                public IntPtr minimumApplicationAddress;
                public IntPtr maximumApplicationAddress;
                public IntPtr activeProcessorMask;
                public uint numberOfProcessors;
                public uint processorType;
                public uint allocationGranularity;
                public ushort processorLevel;
                public ushort processorRevision;
            }

            [DllImport("kernel32.dll")]
            public static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

            public const short VER_NT_WORKSTATION = 1;
            public const short VER_NT_DOMAIN_CONTROLLER = 2;
            public const short VER_NT_SERVER = 3;

            [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool IsWow64Process(
                 [In] IntPtr hProcess,
                 [Out] out bool lpSystemInfo
                 );
        }
        #endregion
    }
}
