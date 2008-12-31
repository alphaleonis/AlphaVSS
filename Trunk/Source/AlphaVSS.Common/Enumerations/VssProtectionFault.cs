using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    public enum VssProtectionFault
    {
        None = 0,
        DiffAreaMissing = 1,
        IOFailureDuringOnline = 2,
        MetaDataCorruption = 3,
        MemoryAllocationFailure = 4,
        MappedMemoryFailure = 5,
        CowReadFailure = 6,
        CowWriteFailure = 7,
        DiffAreaFull = 8,
        GrowTooSlow = 9,
        GrowFailed = 10,
        DestroyAllSnapshots = 11,
        FileSystemFailure = 12,
        IOFailure = 13,
        DiffAreaRemoved = 14,
        ExternalWriterToDiffArea = 15
    }
}
