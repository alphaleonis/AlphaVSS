
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Used by a requester to specify how a resynchronization operation is to be performed.
   /// </summary>
   [Flags]
   public enum VssRecoveryOptions
   {
      /// <summary>
      /// No options.
      /// </summary>
      None = 0x000,
      /// <summary>
      /// After the resynchronization operation is complete, the signature of each target LUN should be identical to that of the original LUN that was used to create the shadow copy.
      /// </summary>
      RevertIdentityAll = 0x100,
      /// <summary>
      /// Volume safety checks should not be performed.
      /// </summary>
      NoVolumeCheck = 0x200,
   }
}
