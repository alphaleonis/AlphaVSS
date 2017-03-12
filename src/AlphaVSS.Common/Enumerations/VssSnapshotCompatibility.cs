

using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///	The <see cref="VssSnapshotCompatibility"/> enumeration indicates which volume control or file I/O operations are disabled for the 
   ///	volume that has been shadow copied.</summary>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames"), Flags]
   public enum VssSnapshotCompatibility
   {
      /// <summary>None of the other flags.</summary>
      None = 0x00,
      /// <summary>The provider managing the shadow copies for a specified volume does not support defragmentation operations on that volume.</summary>
      DisableDefrag = 0x01,
      /// <summary>The provider managing the shadow copies for a specified volume does not support content index operations on that volume.</summary>
      DisableContentIndex = 0x02
   }

}