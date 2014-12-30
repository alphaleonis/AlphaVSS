/* Copyright (c) 2008-2012 Peter Palotas
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
         get
         {
            if (s_servicePackVersion == null)
               UpdateData();
            return s_osVersionName;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the operating system is a server os.
      /// </summary>
      /// <value>
      ///   <c>true</c> if the current operating system is a server os; otherwise, <c>false</c>.
      /// </value>
      public static bool IsServer
      {
         get
         {
            if (s_servicePackVersion == null)
               UpdateData();
            return s_isServer;
         }
      }

      /// <summary>
      /// Gets the numeric version of the operating system. This is the same as returned by 
      /// <see cref="System.Environment.OSVersion"/>.
      /// </summary>
      /// <value>The numeric version of the operating system.</value>
      public static Version OSVersion
      {
         get { return s_osVersion; }
      }

      /// <summary>
      /// Gets the version of the service pack currently installed on the operating system.
      /// </summary>
      /// <value>The version of the service pack currently installed on the operating system.</value>
      /// <remarks>Only the <see cref="Version.Major"/> and <see cref="Version.Minor"/> fields are 
      /// used.</remarks>
      public static Version ServicePackVersion
      {         
         get
         {
            if (s_servicePackVersion == null)
               UpdateData();
            return s_servicePackVersion;
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
         get
         {
            if (s_servicePackVersion == null)
               UpdateData();
            return s_processorArchitecture;
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
      public static bool IsAtLeast(OSVersionName version)
      {
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
      public static bool IsAtLeast(OSVersionName version, int servicePackVersion)
      {
         return OSVersionName == version && ServicePackVersion.Major >= servicePackVersion || OSVersionName > version;
      }

      /// <summary>
      /// Determines whether operating system is of the specified server version or later or if it is of the specified client 
      /// version or later and throws <see cref="UnsupportedOperatingSystemException"/> otherwise.
      /// </summary>
      /// <param name="serverVersion">The minimum server version.</param>
      /// <param name="serverServicePackVersion">The minimum server service pack version (applies only if the version exactly matches the specified server version).</param>
      /// <param name="clientVersion">The minimum client version.</param>
      /// <param name="clientServicePackVersion">The minimum client service pack version (applies only if the version exactly matches the specified client version).</param>
      public static void RequireServerOrClientAtLeast(OSVersionName serverVersion, int serverServicePackVersion, OSVersionName clientVersion, int clientServicePackVersion)
      {
         if (IsServer && !IsAtLeast(serverVersion, serverServicePackVersion) || !IsServer && !IsAtLeast(clientVersion, clientServicePackVersion))
            throw new UnsupportedOperatingSystemException();
      }

      /// <summary>
      /// Determines whether the operating system is a server operating system of atleast the specified <paramref name="serverVersion"/> and
      /// <paramref name="serverServicePackVersion"/> and throws an <see cref="UnsupportedOperatingSystemException"/> otherwise.
      /// </summary>
      /// <param name="serverVersion">The server version.</param>
      /// <param name="serverServicePackVersion">The server service pack version.</param>
      public static void RequireServer(OSVersionName serverVersion, int serverServicePackVersion)
      {
         if (!IsServer || !IsAtLeast(serverVersion, serverServicePackVersion))
            throw new UnsupportedOperatingSystemException();
      }

      /// <summary>
      ///     Determines whether the assembly is executing on the specified operating system version or later.
      ///     If not, an exception is thrown.
      /// </summary>
      /// <param name="osVersion">The minimum operating system version required.</param>
      /// <exception cref="UnsupportedOperatingSystemException">The current operating system is of a version earlier than the specified <paramref name="osVersion"/></exception>      
      public static void RequireAtLeast(OSVersionName osVersion)
      {
         if (!IsAtLeast(osVersion))
            throw new UnsupportedOperatingSystemException();
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
      public static void RequireAtLeast(OSVersionName osVersion, int servicePackVersion)
      {
         if (!IsAtLeast(osVersion, servicePackVersion))
            throw new UnsupportedOperatingSystemException();
      }

      #endregion

      #region Private members            
      
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

         s_processorArchitecture = (ProcessorArchitecture)sysInfo.processorArchitecture;

         s_servicePackVersion = new Version(info.wServicePackMajor, info.wServicePackMinor);

         s_isServer = info.wProductType == NativeMethods.VER_NT_DOMAIN_CONTROLLER || info.wProductType == NativeMethods.VER_NT_SERVER;

         if (info.dwMajorVersion > 6)
         {
            s_osVersionName = OSVersionName.Later;
         }
         else if (info.dwMajorVersion == 6) 
         {
            if (info.dwMinorVersion == 0) // Windows Vista or Windows Server 2008
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION) // Vista
               {
                  s_osVersionName = OSVersionName.WindowsVista;
               }
               else
               {
                  s_osVersionName = OSVersionName.WindowsServer2008;
               }
            }
            else if (info.dwMinorVersion == 1) // Windows 7 or Windows Server 2008 R2
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION)
               {
                  s_osVersionName = Vss.OSVersionName.Windows7;
               }
               else
               {
                  s_osVersionName = Vss.OSVersionName.WindowsServer2008R2;
               }
            }
            else
            {
               s_osVersionName = Vss.OSVersionName.Later;
            }
         }
         else if (info.dwMajorVersion == 5)
         {
            if (info.dwMinorVersion == 0)
            {
               s_osVersionName = OSVersionName.Windows2000;
            }
            if (info.dwMinorVersion == 1)
            {
               s_osVersionName = OSVersionName.WindowsXP;
            }
            else if (info.dwMinorVersion == 2)
            {
               if (info.wProductType == NativeMethods.VER_NT_WORKSTATION && s_processorArchitecture == ProcessorArchitecture.X64)
               {
                  s_osVersionName = OSVersionName.WindowsXP;
               }
               else if (info.wProductType != NativeMethods.VER_NT_WORKSTATION)
               {
                  s_osVersionName = OSVersionName.WindowsServer2003;
               }
               else
               {
                  s_osVersionName = OSVersionName.Later;
               }
            }
            else
            {
               s_osVersionName = OSVersionName.Later;
            }
         }
      }

      private static OSVersionName s_osVersionName = OSVersionName.Later;
      private static Version s_osVersion = Environment.OSVersion.Version;
      private static Version s_servicePackVersion;
      private static ProcessorArchitecture s_processorArchitecture;
      private static bool s_isServer;

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
