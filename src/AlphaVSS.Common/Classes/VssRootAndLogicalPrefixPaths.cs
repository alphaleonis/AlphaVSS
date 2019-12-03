using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>Contains a normalized local volume path or UNC share path as returned by <see cref="IVssBackupComponents.GetRootAndLogicalPrefixPaths"/>.</summary>
   public sealed class VssRootAndLogicalPrefixPaths
   {
      /// <summary>Constructor.</summary>
      /// <param name="rootPath">The root path that should be passed to the <see cref="IVssBackupComponents.AddToSnapshotSet(string)" /> method.</param>
      /// <param name="logicalPrefix">The logical prefix.</param>
      public VssRootAndLogicalPrefixPaths(string rootPath, string logicalPrefix)
      {
         RootPath = rootPath;
         LogicalPrefix = logicalPrefix;
      }

      /// <summary>The root path that should be passed to the <see cref="IVssBackupComponents.AddToSnapshotSet(string)" /> method.</summary>
      public string RootPath { get; }

      /// <summary>If the original file path was a local path, this property contains the volume GUID name. If it was a UNC path, this property contains a fully evaluated share path.</summary>      
      public string LogicalPrefix { get; }
   }
}
