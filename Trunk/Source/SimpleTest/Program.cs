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
            IVssImplementation impl = VssUtils.LoadImplementation(".");

            IVssSnapshotManagement mgmt = impl.GetSnapshotManagementInterface();
            IVssDifferentialSoftwareSnapshotManagement dm = mgmt.GetDifferentialSoftwareSnapshotManagementInterface();

            IList<IVssManagementObjectProperties> list = dm.QueryVolumesSupportedForDiffAreas("C:\\");
            foreach (IVssManagementObjectProperties prop in list)
            {
                switch (prop.Type)
                {
                    case VssManagementObjectType.Unknown:
                        break;
                    case VssManagementObjectType.Volume:
                        VssVolumeProperties vp = prop as VssVolumeProperties;
                        Console.WriteLine("Volume: {0} / {1}", vp.VolumeName, vp.VolumeDisplayName);
                        break;
                    case VssManagementObjectType.DiffVolume:
                        VssDiffVolumeProperties vdp = prop as VssDiffVolumeProperties;
                        Console.WriteLine("DiffVolume: {0} / {1}", vdp.VolumeName, vdp.VolumeDisplayName);
                        break;
                    case VssManagementObjectType.DiffArea:
                        break;
                    default:
                        break;
                }
                Console.WriteLine(prop.Type);
            }
            //Console.WriteLine(dm.GetVolumeProtectionLevel("C:\\"));
        }
    }
}
