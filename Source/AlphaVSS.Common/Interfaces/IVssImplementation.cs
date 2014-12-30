/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     <c>IVssImplementation</c> provides an interface to the global methods of the VSS API compiled for a specific 
   ///     platform. 
   /// </summary>
   /// <remarks>
   ///     An instance of <c>IVssImplementation</c> can be obtained either by using the factory methods of 
   ///     <see cref="VssUtils"/> for dynamically loading the suitable assembly containing the correct implementation (preferred), 
   ///     or by statically referencing the correct platform-specific assembly and manually creating an instance of <c>VssImplementation</c>
   ///     from that assembly.
   /// </remarks>
   /// <seealso cref="VssUtils"/>
   public interface IVssImplementation
   {
      /// <summary>
      /// The <c>CreateVssBackupComponents</c> method creates an <see cref="IVssBackupComponents" /> interface object 
      /// for the current implementation and returns a reference to it.
      /// </summary>
      /// <remarks>
      ///     The calling application is responsible for calling <c>Dispose"</c> to release the 
      ///     resources held by the <see cref="IVssBackupComponents"/> instance when it is no longer needed.
      /// </remarks>
      /// <returns>A reference to the newly created <see cref="IVssBackupComponents"/> instance.</returns>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <seealso cref="IVssBackupComponents"/>
      IVssBackupComponents CreateVssBackupComponents();

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

      /// <summary>
      /// 	The <b>CreateVssExamineWriterMetadata</b> function creates a new <see cref="IVssExamineWriterMetadata"/> instance from an 
      /// 	XML document for the current implementation.
      /// </summary>
      /// <param name="xml">A string containing a Writer Metadata Document with which to initialize the returned <see cref="IVssExamineWriterMetadata"/> object.</param>
      /// <remarks>
      /// 	This method attempts to load the returned <see cref="IVssExamineWriterMetadata"/> object with metadata previously stored by a call to 
      /// 	<see cref="IVssExamineWriterMetadata.SaveAsXml"/>. Users should not tamper with this metadata document.
      /// </remarks>
      /// <returns>a <see cref="IVssExamineWriterMetadata"/> instance initialized with the specified XML document.</returns>
      IVssExamineWriterMetadata CreateVssExamineWriterMetadata(string xml);

      /// <summary>
      /// Gets a snapshot management interface for the current implementation.
      /// </summary>
      /// <returns>A snapshot management interface for the current implementation.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      IVssSnapshotManagement GetSnapshotManagementInterface();
   }
}
