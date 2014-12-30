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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using AlphaShadow;
using Alphaleonis.Win32.Vss;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>
   /// This class is a slimmed down version of the Volume-class in AlphaFS (http://alphafs.codeplex.com). It is included
   /// in this sample to avoid having a dependency on AlphaFS in here, but please use the AlphaFS version for any 
   /// production code.
   /// </summary>
   internal static class Volume
   {
      /// <summary>
      /// Retrieves information about MS-DOS device names. 
      /// The function can obtain the current mapping for a particular MS-DOS device name. 
      /// The function can also obtain a list of all existing MS-DOS device names.
      /// </summary>
      /// <param name="device">The device.</param>
      /// <returns>An MS-DOS device name string specifying the target of the query. The device name cannot have a 
      /// trailing backslash. This parameter can be <see langword="null"/>. In that case, the QueryDosDevice function 
      /// will return an array of all existing MS-DOS device names</returns>
      /// <remarks>See documentation on MSDN for the Windows QueryDosDevice() method for more information.</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string[] QueryDosDevice(string device)
      {
         uint returnSize = 0;
         int maxSize = 260;

         List<string> l = new List<string>();

         while (true)
         {
            char[] buffer = new char[maxSize];

            returnSize = NativeMethods.QueryDosDeviceW(device, buffer, (uint)buffer.Length);
            int lastError = Marshal.GetLastWin32Error();
            
            if (lastError == 0 && returnSize > 0)
            {
               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < returnSize; i++)
               {
                  if (buffer[i] != '\0')
                     sb.Append(buffer[i]);
                  else if (sb.Length > 0)
                  {
                     l.Add(sb.ToString());
                     sb.Length = 0;
                  }
               }

               return l.ToArray();
            }
            else if (lastError == NativeMethods.ERROR_INSUFFICIENT_BUFFER)
            {
               maxSize *= 2;
            }
            else
            {
               throw new Win32Exception(lastError);
            }
         }
      }

      /// <summary>
      /// Gets the shortest display name for the specified <paramref name="volumeName"/>.
      /// </summary>
      /// <param name="volumeName">The volume name.</param>
      /// <returns>The shortest display name for the specified volume found, or <see langword="null"/> if no display names were found.</returns>
      /// <exception cref="ArgumentNullException"><paramref name="volumeName"/> is a <see langword="null"/> reference</exception>
      /// <exception cref="Win32Exception">An error occured during a system call, such as the volume name specified was invalid or did not exist.</exception>
      /// <remarks>This method basically returns the shortest string returned by <see cref="GetVolumePathNamesForVolume"/></remarks>        
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string GetDisplayNameForVolume(string volumeName)
      {
         string[] volumeMountPoints = GetVolumePathNamesForVolume(volumeName);

         if (volumeMountPoints.Length == 0)
            return null;

         string smallestMountPoint = volumeMountPoints[0];
         for (int i = 1; i < volumeMountPoints.Length; i++)
         {
            if (volumeMountPoints[i].Length < smallestMountPoint.Length)
               smallestMountPoint = volumeMountPoints[i];
         }
         return smallestMountPoint;
      }

      /// <summary>
      /// Retrieves a list of path names for the specified volume name.
      /// </summary>
      /// <param name="volumeName">The volume name.</param>
      /// <returns>An array containing the path names for the specified volume.</returns>
      /// <exception cref="ArgumentNullException"><paramref name="volumeName"/> is a <see langword="null"/> reference</exception>
      /// <exception cref="System.IO.FileNotFoundException">The volume name specified was invalid, did not exist or was not ready.</exception>
      /// <remarks>For more information about this method see the MSDN documentation on GetVolumePathNamesForVolumeName().</remarks>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string[] GetVolumePathNamesForVolume(string volumeName)
      {
         if (volumeName == null)
            throw new ArgumentNullException("volumeName");

         uint requiredLength = 0;
         char[] buffer = new char[NativeMethods.MAX_PATH];

         if (!NativeMethods.GetVolumePathNamesForVolumeNameW(volumeName, buffer, (uint)buffer.Length, ref requiredLength))
         {
            int errorCode = Marshal.GetLastWin32Error();
            if (errorCode == NativeMethods.ERROR_MORE_DATA || errorCode == NativeMethods.ERROR_INSUFFICIENT_BUFFER)
            {
               buffer = new char[requiredLength];
               if (!NativeMethods.GetVolumePathNamesForVolumeNameW(volumeName, buffer, (uint)buffer.Length, ref requiredLength))
                  Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            else
            {
               throw new Win32Exception();
            }
         }

         List<string> displayNames = new List<string>();
         StringBuilder displayName = new StringBuilder();

         for (int i = 0; i < requiredLength; i++)
         {
            if (buffer[i] == '\0')
            {
               if (displayName.Length > 0)
                  displayNames.Add(displayName.ToString());
               displayName.Length = 0;
            }
            else
            {
               displayName.Append(buffer[i]);
            }
         }

         return displayNames.ToArray();
      }
      
      public static bool IsVolume(IUIHost host, string volumePath)
      {
                
         if (volumePath == null)
            throw new ArgumentNullException("volumePath");

         if (volumePath.Length == 0)
            return false;

         host.WriteVerbose("- Checking if \"{0}\" is a real volume path...", volumePath);

         if (!volumePath.EndsWith("\\"))
            volumePath = volumePath + "\\";

         StringBuilder volumeNameBuilder = new StringBuilder(NativeMethods.MAX_PATH);
         if (ClusterIsPathOnSharedVolume(host, volumePath))
         {
            if (!NativeMethods.ClusterGetVolumeNameForVolumeMountPointW(volumePath, volumeNameBuilder, (uint)volumeNameBuilder.Capacity))
            {
               host.WriteVerbose("- ClusterGetVolumeNameForVolumeMountPointW(\"{0}\") failed with error code {1}.", volumePath, Marshal.GetLastWin32Error());
               return false;
            }
            return true;
         }
         else
         {
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumePath, volumeNameBuilder, (uint)volumeNameBuilder.Capacity))
            {
               host.WriteVerbose("- GetVolumeNameForVolumeMountPoint(\"{0}\") failed with error code {1}.", volumePath, Marshal.GetLastWin32Error());
               return false;
            }
            return true;
         }

      }

      /// <summary>
      /// Retrieves the unique volume name for the specified volume mount point or root directory.
      /// </summary>
      /// <param name="mountPoint">The path of a volume mount point (with or without a trailing backslash, "\") or a drive letter indicating a root directory (eg. "C:" or "D:\"). A trailing backslash is required.</param>
      /// <returns>The unique volume name of the form "\\?\Volume{GUID}\" where GUID is the GUID that identifies the volume.</returns>
      /// <exception cref="ArgumentNullException"><paramref name="mountPoint"/> is a <see langword="null"/> reference</exception>
      /// <exception cref="ArgumentException"><paramref name="mountPoint"/> is an empty string</exception>        
      /// <exception cref="Win32Exception">Upon error retreiving the volume name</exception>
      /// <remarks>See the MSDN documentation on the method GetVolumeNameForVolumeMountPoint() for more information.</remarks>
      public static string GetUniqueVolumeNameForVolumeMountPoint(string mountPoint)
      {
         if (mountPoint == null)
            throw new ArgumentNullException("mountPoint");

         if (mountPoint.Length == 0)
            throw new ArgumentException("Mount point must be non-empty");

         // Get the volume name alias. This may be different from the unique volume name in some
         // rare cases.
         StringBuilder volumeName = new StringBuilder(NativeMethods.MAX_PATH);
         if (!NativeMethods.GetVolumeNameForVolumeMountPointW(mountPoint, volumeName, (uint)volumeName.Capacity))
            throw new Win32Exception();

         // Get the unique volume name
         StringBuilder uniqueVolumeName = new StringBuilder(NativeMethods.MAX_PATH);
         if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeName.ToString(), uniqueVolumeName, (uint)volumeName.Capacity))
            throw new Win32Exception();

         return uniqueVolumeName.ToString();
      }

      public static string GetUniqueVolumeNameForPath(IUIHost host, string path, bool isBackup)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (path.Length == 0)
            throw new ArgumentException("Mount point must be non-empty");

         host.WriteVerbose("- Get volume path name for \"{0}\"...", path);

         if (!path.EndsWith("\\"))
            path = path + "\\";

         if (isBackup && ClusterIsPathOnSharedVolume(host, path))
         {
            string volumeRootPath, volumeUniqueName;
            ClusterPrepareSharedVolumeForBackup(path, out volumeRootPath, out volumeUniqueName);
            host.WriteVerbose("- Path name: {0}", volumeRootPath);
            host.WriteVerbose("- Unique volume name: {0}", volumeUniqueName);
            return volumeUniqueName;
         }
         else
         {            
            // Get the root path of the volume
            StringBuilder volumeRootPath = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumePathNameW(path, volumeRootPath, (uint)volumeRootPath.Capacity))
            {
               host.WriteVerbose("- GetVolumePathName(\"{0}\") failed with error code {1}", path, Marshal.GetLastWin32Error());
               throw new Win32Exception();
            }

            // Get the volume name alias (might be different from the unique volume name in rare cases)
            StringBuilder volumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeRootPath.ToString(), volumeName, (uint)volumeName.Capacity))
            {
               host.WriteVerbose("- GetVolumeNameForVolumeMountPoint(\"{0}\") failed with error code {1}", volumeRootPath.ToString(), Marshal.GetLastWin32Error());
               throw new Win32Exception();
            }

            // Gte the unique volume name
            StringBuilder uniqueVolumeName = new StringBuilder(NativeMethods.MAX_PATH);
            if (!NativeMethods.GetVolumeNameForVolumeMountPointW(volumeName.ToString(), uniqueVolumeName, (uint)uniqueVolumeName.Capacity))
            {
               host.WriteVerbose("- GetVolumeNameForVolumeMountPoint(\"{0}\") failed with error code {1}", volumeName.ToString(), Marshal.GetLastWin32Error());
               throw new Win32Exception();
            }

            return uniqueVolumeName.ToString();
         }
      }

      public static bool ClusterIsPathOnSharedVolume(IUIHost host, string path)
      {
         if (OperatingSystemInfo.IsAtLeast(OSVersionName.Windows7))
         {
            host.WriteVerbose("- Calling ClusterIsPathOnSharedVolume(\"{0}\")...", path);
            return NativeMethods.ClusterIsPathOnSharedVolume(path);
         }
         else
         {
            host.WriteVerbose("- Skipping call to ClusterIsPathOnSharedVolume; function does not exist on this OS.");
            return false;
         }
      }

      public static string ClusterGetVolumeNameForVolumeMountPoint(IUIHost host, string volumeMountPoint)
      {
         host.WriteVerbose("- Calling ClusterGetVolumeNameForVolumeMountPoint(\"{0}\")...", volumeMountPoint);
         StringBuilder result = new StringBuilder(NativeMethods.MAX_PATH);
         if (!NativeMethods.ClusterGetVolumeNameForVolumeMountPointW(volumeMountPoint, result, (uint)result.Capacity))
            throw new Win32Exception();
         return result.ToString();
      }

      private static void ClusterPrepareSharedVolumeForBackup(string fileName, out string volumePathName, out string volumeName)
      {
         StringBuilder volumeRootPath = new StringBuilder(NativeMethods.MAX_PATH);
         StringBuilder volumeUniqueName = new StringBuilder(NativeMethods.MAX_PATH);
         int volumeRootPathCount = volumeRootPath.Capacity;
         int volumeUniqueNameCount = volumeUniqueName.Capacity;
         int ret = NativeMethods.ClusterPrepareSharedVolumeForBackup(fileName, volumeRootPath, ref volumeRootPathCount, volumeUniqueName, ref volumeUniqueNameCount);
         if (ret != 0)
            throw new Win32Exception(ret);

         volumePathName = volumeRootPath.ToString();
         volumeName = volumeUniqueName.ToString();
      }

      /// <summary>
      ///  Retreives the Win32 device name from the volume name
      /// </summary>
      /// <param name="volumeName">Name of the volume. A trailing backslash is not allowed.</param>
      /// <returns>The Win32 device name from the volume name</returns>
      [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
      public static string GetDeviceForVolumeName(string volumeName)
      {
         if (volumeName == null)
            throw new ArgumentNullException("volumeName");

         if (volumeName.Length == 0)
            throw new ArgumentException("Volume name must be non-empty");

         // Eliminate the GLOBALROOT prefix if present
         const string globalRootPrefix = @"\\?\GLOBALROOT";

         if (volumeName.StartsWith(globalRootPrefix, StringComparison.OrdinalIgnoreCase))
            return volumeName.Substring(globalRootPrefix.Length);

         // If this is a volume name, get the device
         const string dosPrefix = @"\\?\";
         const string volumePrefix = @"\\?\Volume";

         if (volumeName.StartsWith(volumePrefix, StringComparison.OrdinalIgnoreCase))
         {
            // Isolate the DOS device for the volume name (in the format Volume{GUID})
            string dosDevice = volumeName.Substring(dosPrefix.Length);

            // Get the real device underneath
            return QueryDosDevice(dosDevice)[0];
         }

         return volumeName;
      }

 
      private static class NativeMethods
      {
         public const int MAX_PATH = 261;
         public const uint ERROR_INSUFFICIENT_BUFFER = 122;
         public const uint ERROR_MORE_DATA = 234;

         [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool GetVolumeNameForVolumeMountPointW([In] string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

         [DllImport("ResUtils.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool ClusterGetVolumeNameForVolumeMountPointW([In] string lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName, uint cchBufferLength);

         [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool GetVolumePathNameW([In] string lpszFileName, [Out] StringBuilder lpszVolumePathName, uint cchBufferLength);

         [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         public static extern bool GetVolumePathNamesForVolumeNameW(string lpszVolumeName, char[] lpszVolumePathNames, uint cchBuferLength, ref uint lpcchReturnLength);

         [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         internal static extern uint QueryDosDeviceW(string lpDeviceName, [Out] char[] lpTargetPath, uint ucchMax);

         [DllImport("ResUtils.dll", CharSet = CharSet.Unicode, SetLastError = true)]
         [return: MarshalAs(UnmanagedType.Bool)]
         internal static extern bool ClusterIsPathOnSharedVolume(string lpszPathName);

         [DllImport("ResUtils.dll", CharSet = CharSet.Unicode, SetLastError = false, PreserveSig=true)]
         [return: MarshalAs(UnmanagedType.U4)]
         internal static extern int ClusterPrepareSharedVolumeForBackup(string lpszFileName, [Out] StringBuilder lpszVolumePathName, ref int lpcchVolumePathName, [Out] StringBuilder lpszVolumeName, ref int lpcchVolumeName);
      }
   }
}
