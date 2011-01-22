/* Copyright (c) 2008-2011 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Static class providing access to information about the operating system under which the
   /// assembly is executing.
   /// </summary>
   public static class OperatingSystemInfo
   {
      #region Public Properties

      /// <summary>
      /// Gets the named version of the operating system.
      /// </summary>
      /// <value>The named version of the operating system.</value>
      public static OSVersionName OSVersionName
      {
         [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
         get
         {
            if (m_ServicePackVersion == null)
               UpdateData();
            return m_OSVersionName;
         }
      }

      /// <summary>
      /// Gets the numeric version of the operating system. This is the same as returned by 
      /// <see cref="System.Environment.OSVersion"/>.
      /// </summary>
      /// <value>The numeric version of the operating system.</value>
      public static Version OSVersion
      {
         get { return m_OSVersion; }
      }

      /// <summary>
      /// Gets the version of the service pack currently installed on the operating system.
      /// </summary>
      /// <value>The version of the service pack currently installed on the operating system.</value>
      /// <remarks>Only the <see cref="Version.Major"/> and <see cref="Version.Minor"/> fields are 
      /// used.</remarks>
      public static Version ServicePackVersion
      {
         [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
         get
         {
            if (m_ServicePackVersion == null)
               UpdateData();
            return m_ServicePackVersion;
         }
      }

      /// <summary>
      /// Gets the processor architecture for which the operating system is targeted.
      /// </summary>
      /// <value>The processor architecture for which the operating system is targeted.</value>
      /// <remarks>If running under WOW64 this will return a 32-bit processor. Use <see cref="IsWow64Process"/> to
      /// determine if this is the case.
      /// </remarks>
      public static ProcessorArchitecture ProcessorArchitecture
      {
         [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
         get
         {
            if (m_ServicePackVersion == null)
               UpdateData();
            return m_ProcessorArchitecture;
         }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// Determines whether the current process is running under WOW64.
      /// </summary>
      /// <returns>
      /// 	<c>true</c> if the current process is running under WOW64; otherwise, <c>false</c>.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
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

      /// <summary>
      /// Determines whether the operating system is of the specified version or later.
      /// </summary>
      /// <param name="version">The lowest version for which to return <c>true</c>.</param>
      /// <returns>
      /// 	<c>true</c> if the operating system is of the specified <paramref name="version"/> or later; otherwise, <c>false</c>.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static bool IsAtLeast(OSVersionName version)
      {
         Debug.Assert(version != OSVersionName.Unknown);
         return OSVersionName >= version;
      }

      /// <summary>
      /// Determines whether operating system is of the specified version or later, allowing specification of 
      /// a minimum service pack that must be installed on the lowest version.
      /// </summary>
      /// <param name="version">The minimum required version.</param>
      /// <param name="servicePackVersion">The major version of the service pack that must be installed on the 
      /// minimum required version to return <c>true</c>. This can be 0 to indicate that no service pack is required.</param>
      /// <returns>
      /// 	<c>true</c> if the operating system matches the specified <paramref name="version"/> with the specified service pack, or if the operating system is of a later version; otherwise, <c>false</c>.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static bool IsAtLeast(OSVersionName version, int servicePackVersion)
      {
         Debug.Assert(version != OSVersionName.Unknown);
         return IsWithSPAtLeast(version, servicePackVersion) || OSVersionName > version;
      }

      /// <summary>
      /// Determines whether the current operating system matches the specified version and has at least the 
      /// specified service pack installed.
      /// </summary>
      /// <param name="version">The required operating system version.</param>
      /// <param name="servicePackVersion">The required service pack major version number.</param>
      /// <returns>
      /// 	<c>true</c> if the current operating system version matches <paramref name="version"/>
      /// 	and has atleast service pack <paramref name="servicePackVersion"/> installed; otherwise, <c>false</c>.
      /// </returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static bool IsWithSPAtLeast(OSVersionName version, int servicePackVersion)
      {
         Debug.Assert(version != OSVersionName.Unknown);
         return (OSVersionName == version && ServicePackVersion.Major >= servicePackVersion);
      }

      /// <summary>
      ///     Determines whether the assembly is executing on the specified operating system version, and throws
      ///     an <see cref="UnsupportedOperatingSystemException"/> otherwise.
      /// </summary>
      /// <param name="version">The operating system version to match.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system version does not match the specified <paramref name="version"/>.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Require(OSVersionName version)
      {
         Debug.Assert(version != OSVersionName.Unknown);
         if (version != OSVersionName)
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0DetectedOperatingSystemWas1, ToString(version), ToString(OSVersionName)));
      }

      /// <summary>
      ///     Determines whether the assembly is executing on the specified operating system version with the
      ///     service pack specified installed, and throws an <see cref="UnsupportedOperatingSystemException"/> otherwise.
      /// </summary>
      /// <param name="version">The operating system version to match.</param>
      /// <param name="servicePackVersion">The major service pack version to match.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system version does not match the specified <paramref name="version"/>,
      /// or the major version of the installed service pack does not match <paramref name="servicePackVersion"/>.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void Require(OSVersionName version, int servicePackVersion)
      {
         Debug.Assert(version != OSVersionName.Unknown);
         if (version != OSVersionName || servicePackVersion != ServicePackVersion.Major)
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires01DetectedOperatingSystemWas23,
                ToString(version), SpToString(servicePackVersion), ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
      }

      /// <summary>
      ///     Determines whether the assembly is executing under the specified operating system version with 
      ///     at least the specified service pack installed, and throws an exception otherwise.
      /// </summary>
      /// <param name="osVersion">The operating system version to match.</param>
      /// <param name="servicePackVersion">The major service pack version to match.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system version does not match
      /// the specified <paramref name="osVersion"/> with at least service pack <paramref name="servicePackVersion"/>.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void RequireWithSPAtLeast(OSVersionName osVersion, int servicePackVersion)
      {
         if (!IsWithSPAtLeast(osVersion, servicePackVersion))
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0WithAtLeastServicePack1DetectedOperatingSystemWas23,
                ToString(osVersion), servicePackVersion, ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
      }

      /// <summary>
      ///     Determines whether the assembly is executing under one of the specified operating system versions with 
      ///     at least the specified service pack installed, and throws an exception otherwise.
      /// </summary>
      /// <param name="osVersion1">The first operating system version to match.</param>
      /// <param name="servicePackVersion1">The first major service pack version to match.</param>
      /// <param name="osVersion2">The second operating system version to match.</param>
      /// <param name="servicePackVersion2">The second major service pack version to match.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system version does not match
      /// the specified <paramref name="osVersion1"/> with at least service pack <paramref name="servicePackVersion1"/> installed, <b>and</b>
      /// it does not match the specified <paramref name="osVersion2"/> with at least service pack <paramref name="servicePackVersion2"/> installed.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void RequireWithSPAtLeast(OSVersionName osVersion1, int servicePackVersion1, OSVersionName osVersion2, int servicePackVersion2)
      {
         if (!IsWithSPAtLeast(osVersion1, servicePackVersion1) && !IsWithSPAtLeast(osVersion2, servicePackVersion2))
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0WithAtLeastServicePack1Or2WithAtLeastServicePack3DetectedOperatingSystemWas45,
                ToString(osVersion1), servicePackVersion1, ToString(osVersion2), servicePackVersion2, ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
      }

      /// <summary>
      ///     Determines whether the assembly is executing on the specified operating system version or later.
      ///     If not, an exception is thrown.
      /// </summary>
      /// <param name="osVersion">The minimum operating system version required.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system is of a version earlier than the specified <paramref name="osVersion"/></exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void RequireAtLeast(OSVersionName osVersion)
      {
         if (!IsAtLeast(osVersion))
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires0OrLaterDetectedOperatingSystemWas1,
                ToString(osVersion), ToString(OSVersionName)));
      }

      /// <summary>
      ///     Determines whether the assembly is executing on the specified operating system version with
      ///     the specified service pack installed or any later version of windows. If not, an exception is thrown.
      /// </summary>
      /// <param name="osVersion">The minimum operating system version required.</param>
      /// <param name="servicePackVersion">The minimum service pack version required.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system is of a version earlier 
      /// than the specified <paramref name="osVersion"/> or the versions match but the operating system does not 
      /// have at least the specified service pack version <paramref name="servicePackVersion"/> installed.</exception>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static void RequireAtLeast(OSVersionName osVersion, int servicePackVersion)
      {
         if (!IsAtLeast(osVersion, servicePackVersion))
            throw new UnsupportedOperatingSystemException(String.Format(CultureInfo.CurrentCulture, Resources.LocalizedStrings.ThisOperationRequires01OrLaterDetectedOperatingSystemWas23,
                ToString(osVersion), SpToString(servicePackVersion), ToString(OSVersionName), SpToString(ServicePackVersion.Major)));
      }

      #endregion

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
            case OSVersionName.Windows7:
               return "Windows 7";
            case OSVersionName.WindowsServer2008R2:
               return "Windows Server 2008 R2";
            default:
               return "Unknown";
         }
      }

      private static string SpToString(int servicePackVersion)
      {
         if (servicePackVersion == 0)
            return String.Empty;

         return String.Format(CultureInfo.InvariantCulture, " SP{0}", servicePackVersion);
      }

      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
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
         Debug.Assert(info.dwMinorVersion == Environment.OSVersion.Version.Minor);
         Debug.Assert(info.dwBuildNumber == Environment.OSVersion.Version.Build);

         m_ProcessorArchitecture = (ProcessorArchitecture)sysInfo.processorArchitecture;

         m_ServicePackVersion = new Version(info.wServicePackMajor, info.wServicePackMinor);

         if (info.dwMajorVersion == 6) // Vista or 2008
         {
            if (info.dwMinorVersion == 0)
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION) // Vista
               {
                  m_OSVersionName = OSVersionName.WindowsVista;
               }
               else
               {
                  m_OSVersionName = OSVersionName.WindowsServer2008;
               }
            }
            else
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION)
               {
                  m_OSVersionName = Vss.OSVersionName.Windows7;
               }
               else
               {
                  m_OSVersionName = Vss.OSVersionName.WindowsServer2008R2;
               }
            }
         }
         else if (info.dwMajorVersion == 5)
         {
            if (info.dwMinorVersion == 0)
            {
               m_OSVersionName = OSVersionName.Windows2000;
            }
            if (info.dwMinorVersion == 1)
            {
               m_OSVersionName = OSVersionName.WindowsXP;
            }
            else if (info.dwMinorVersion == 2)
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION && m_ProcessorArchitecture == ProcessorArchitecture.X64)
               {
                  m_OSVersionName = OSVersionName.WindowsXP;
               }
               else if (info.wProductType != NativeMethods.VER_NT_WORKSTATION)
               {
                  m_OSVersionName = OSVersionName.WindowsServer2003;
               }
               else
               {
                  m_OSVersionName = OSVersionName.Unknown;
               }
            }
            else
            {
               m_OSVersionName = OSVersionName.Unknown;
            }
         }
      }

      private static OSVersionName m_OSVersionName = OSVersionName.Unknown;
      private static Version m_OSVersion = Environment.OSVersion.Version;
      private static Version m_ServicePackVersion;
      private static ProcessorArchitecture m_ProcessorArchitecture;

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

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2205:UseManagedEquivalentsOfWin32Api")]
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
              [Out, MarshalAs(UnmanagedType.Bool)] out bool lpSystemInfo
              );
      }
      #endregion
   }
}
