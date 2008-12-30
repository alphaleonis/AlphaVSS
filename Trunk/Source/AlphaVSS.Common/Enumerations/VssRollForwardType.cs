using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///     The <c>VssRollForwardType</c> enumeration is used by a requester to indicate the type of roll-forward operation it is about to perform.
    /// </summary>
    /// <remarks>
    ///     A requester sets the roll-forward operation type and specifies the restore point for partial roll-forward operations 
    ///     using <see cref="IVssBackupComponents.SetRollForward"/>.
    /// </remarks>
    public enum VssRollForwardType
    {
        /// <summary>
        /// <para>
        ///     No roll-forward type is defined.
        /// </para>
        /// <para>
        ///     This indicates an error on the part of the requester.
        /// </para>
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The roll-forward operation should not roll forward through logs.
        /// </summary>
        None = 1,
        /// <summary>
        /// The roll-forward operation should roll forward through all logs.
        /// </summary>
        All = 2,
        /// <summary>
        /// The roll-forward operation should roll forward through logs up to a specified restore point.
        /// </summary>
        Partial = 3 
    }
}
