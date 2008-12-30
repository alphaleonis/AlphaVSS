using System;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Defines shadow copy LUN flags.
    /// </summary>
    /// <remarks>
    ///     Only supported on Windows Server 2008.
    /// </remarks>
    [Flags]
    public enum VssHardwareOptions
    {
        /// <summary>
        /// The shadow copy LUN will be masked from the host.
        /// </summary>
        MaskLuns = 0x00000001,
        /// <summary>
        /// The shadow copy LUN will be exposed to the host as a read-write volume.
        /// </summary>
        MakeReadWrite = 0x00000002,
        /// <summary>
        /// The disk identifiers of all of the shadow copy LUNs will be reverted to that of the 
        /// original LUNs. However, if any of the original LUNs are present on the system, the operation will 
        /// fail and none of the identifiers will be reverted.
        /// </summary>
        RevertIdentityAll = 0x00000004,
        /// <summary>
        /// None of the disk identifiers of the shadow copy LUNs will be reverted.
        /// </summary>
        RevertIdentityNone = 0x00000008,
        /// <summary>
        /// The shadow copy LUNs will be converted permanently to read-write. 
        /// This flag is set only as a notification for the provider; no provider action is required. 
        /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange</c> method.
        /// </summary>
        OnLunStateChangeNotifyReadWrite = 0x00000100,
        /// <summary>
        /// The shadow copy LUNs will be converted temporarily to read-write and are about to undergo TxF recovery 
        /// or VSS auto-recovery. This flag is set only as a notification for the provider; no provider action is required. 
        /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
        /// </summary>
        OnLunStateChangeNotifyLunPreRecovery = 0x00000200,
        /// <summary>
        /// The shadow copy LUNs have just undergone TxF recovery or VSS auto-recovery and have been converted back to 
        /// read-only. This flag is set only as a notification for the provider; no provider action is required. 
        /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
        /// </summary>
        OnLunStateChangeNotifyLunPostRecovery = 0x00000400,
        /// <summary>
        /// The provider must mask shadow copy LUNs from this computer. 
        /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
        /// </summary>
        OnLunStateChangeDoMaskLuns = 0x00000800
    }
}
