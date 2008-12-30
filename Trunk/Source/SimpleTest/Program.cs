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

            using (IVssBackupComponents backup = impl.CreateVssBackupComponents())
            {
                backup.InitializeForBackup(null);
                backup.SetRestoreName(Guid.Empty, VssComponentType.Database, null, "hede", "hodo");
                foreach (VssProviderProperties prop in backup.QueryProviders())
                {
                    Console.WriteLine(prop.ProviderName);
                }
            }
        }
    }
}
