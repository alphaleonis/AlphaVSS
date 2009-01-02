using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alphaleonis.Win32.Vss;

namespace SimpleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain domain = AppDomain.CreateDomain("myDomain");
            IVssImplementation impl = VssUtils.LoadImplementation(domain);
            IVssBackupComponents backup = impl.CreateVssBackupComponents();
            Console.WriteLine(OperatingSystemInfo.OSVersionName);
            Console.WriteLine(OperatingSystemInfo.ProcessorArchitecture);
            Console.WriteLine(OperatingSystemInfo.IsWow64Process());
            OperatingSystemInfo.RequireAtLeast(OSVersionName.WindowsServer2008, 3);
#if false
            IVssImplementation impl = VssUtils.LoadImplementation(".");

            IVssSnapshotManagement mgmt = impl.GetSnapshotManagementInterface();
            IVssDifferentialSoftwareSnapshotManagement dm = mgmt.GetDifferentialSoftwareSnapshotManagementInterface();

            IList<VssDiffAreaProperties> list = dm.QueryDiffAreasForVolume("C:\\");
            foreach (VssDiffAreaProperties vdpa in list)
            {
                Console.WriteLine("DiffVolume: {0} / {1}", vdpa.VolumeName, vdpa.DiffAreaVolumeName);
            }
            //Console.WriteLine(dm.GetVolumeProtectionLevel("C:\\"));
#endif
        }
    }
}
