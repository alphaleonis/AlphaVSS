
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Provides access to global methods of the Win32 VSS API that allow querying for specific information.
   /// </summary>
   /// <seealso cref="IVssFactory"/>
   public interface IVssInfoProvider
   {
      /// <summary>
      /// The <c>IsVolumeSnapshotted</c> function determines whether any shadow copies exist for the specified volume.
      /// </summary>
      ///  <remarks>
      ///     Use <see cref="GetSnapshotCompatibility"/> to determine whether certain volume control or file I/O operations are 
      ///     disabled for the given volume if a shadow copy of it exists.
      ///  </remarks>
      /// <param name="volumeName">
      ///     Name of the volume. The name of the volume to be checked must be in one of the following formats:
      ///     <list type="bullet">
      ///     <item><description>The path of a volume mount point with a backslash (\)</description></item>
      ///     <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      ///     <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      ///     </list>
      ///  </param>
      /// <returns><c>true</c> if the volume has a shadow copy, and <c>false</c> if the volume does not have a shadow copy.</returns>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the event log file.</exception>
      bool IsVolumeSnapshotted(string volumeName);

      /// <summary>
      ///     Determines whether certain volume control or file I/O operations are disabled for the given volume if a shadow copy of it exists.
      /// </summary>
      /// <param name="volumeName">
      ///     Name of the volume. The name of the volume to be checked must be in one of the following formats:
      ///     <list type="bullet">
      ///     <item><description>The path of a volume mount point with a backslash (\)</description></item>
      ///     <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      ///     <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      ///     </list>
      ///  </param>
      /// <returns>
      ///     A bit mask (or bitwise OR) of <see cref="VssSnapshotCompatibility"/> values that indicates whether certain 
      ///     volume control or file I/O operations are disabled for the given volume if a shadow copy of it exists.
      /// </returns>
      /// <remarks>
      ///     <para>
      ///         Use <see cref="IsVolumeSnapshotted"/> to determine whether a snapshot exists for the specified volume or not.
      ///     </para>
      ///     <para>
      ///         If no volume control or file I/O operations are disabled for the selected volume, then the shadow copy capability of the 
      ///         selected volume returned will <see cref="VssSnapshotCompatibility.None"/>.
      ///     </para>
      /// </remarks>
      /// <seealso cref="IsVolumeSnapshotted"/>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the event log file.</exception>
      VssSnapshotCompatibility GetSnapshotCompatibility(string volumeName);

      /// <summary>
      /// Checks the registry for writers that should block revert operations on the specified volume.
      /// </summary>
      /// <param name="volumeName">
      ///     The name of the volume. The name must be in one of the following formats:
      ///     <list type="bullet">
      ///         <item><description>The path of a volume mount point with a backslash (\)</description></item>
      ///         <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      ///         <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      ///     </list>
      ///  </param>
      /// <returns>
      ///     <see langword="true" /> if the volume contains components from any writers that are listed in the registry as writers that should block 
      ///     revert operations; otherwise, <see langword="false"/>
      /// </returns>
      bool ShouldBlockRevert(string volumeName);

   }
}
