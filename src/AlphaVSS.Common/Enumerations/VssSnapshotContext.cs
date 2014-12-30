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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// The <see cref="VssSnapshotContext" /> enumeration enables a requester using <see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetContext" /> to specify how a 
   /// shadow copy is to be created, queried, or deleted and the degree of writer involvement.
   /// <see cref="IVssBackupComponents"/>::SetContext" method) may be modified by a bitmask that contains a valid combination of 
   /// <see cref="VssVolumeSnapshotAttributes"/> and <see cref="VssSnapshotContext"/> enumeration values.
   /// </summary>
   /// <remarks><see cref="VssSnapshotContext"/> is defined as a static class defining the base <see cref="VssVolumeSnapshotAttributes"/>
   /// combination of values representing the VSS_CTX_XXXXXX constants defined in the VSS API. 
   /// </remarks>
   /// <seealso cref="VssVolumeSnapshotAttributes" />
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
   public enum VssSnapshotContext : uint
   {
      /// <summary>
      /// The standard backup context. Specifies an auto-release, nonpersistent shadow copy in which writers are involved in the creation.
      /// </summary>		
      Backup = 0,

      /// <summary>
      /// Specifies a nonpersistent and auto-release shadow copy created without writer involvement.
      /// </summary>
      FileShareBackup = VssVolumeSnapshotAttributes.NoWriters,

      /// <summary>
      /// <para>Specifies a persistent and non-auto-release shadow copy without writer involvement. This context should be used when there is no need for writer involvement to ensure that files are in a consistent state at the time of the shadow copy. </para>
      /// <para>Lightweight automated file rollback mechanisms or persistent shadow copies of file shares or data volumes that are not expected to contain any system-related files or databases might run under this context. For example, a requester could use this context for creating a shadow copy of a NAS volume hosting documents and simple user shares. Those types of data do not need writer involvement to create a consistent shadow copy.</para>
      /// </summary>
      NasRollback = VssVolumeSnapshotAttributes.Persistent | VssVolumeSnapshotAttributes.NoAutoRelease | VssVolumeSnapshotAttributes.NoWriters,

      /// <summary>
      /// <para>Specifies a persistent and non-auto-release shadow copy with writer involvement. This context is designed to be used when writers are needed to ensure that files are in a well-defined state prior to shadow copy. </para>
      /// <para>Automated file rollback mechanisms of system volumes and shadow copies to be used in data mining or restore operations might run under this context. This context is similar to VSS_CTX_BACKUP but allows a requester more control over the persistence of the shadow copy.</para>
      /// </summary>
      AppRollback = VssVolumeSnapshotAttributes.Persistent | VssVolumeSnapshotAttributes.NoAutoRelease,

      /// <summary>
      /// <para>Specifies a read-only, client-accessible shadow copy supporting Shadow Copies for Shared Folders and created without writer involvement. Only the system provider (the default provider available on the system) can create this type of shadow copy. </para>
      /// <para>Most requesters will want to use the <see cref="NasRollback" /> context for persistent, non-auto-release shadow copies without writer involvement.</para>
      /// </summary>
      ClientAccessible = VssVolumeSnapshotAttributes.Persistent | VssVolumeSnapshotAttributes.ClientAccessible | VssVolumeSnapshotAttributes.NoAutoRelease | VssVolumeSnapshotAttributes.NoWriters,

      /// <summary>
      /// <para>Specifies a read-only, client-accessible shadow copy supporting Shadow Copies for Shared Folders and created with writer involvement. Only the system provider (the default provider available on the system) can create this type of shadow copy. </para>
      /// <para>Most requesters will want to use the <see cref="AppRollback"/> context for persistent, non-auto-release shadow copies with writer involvement.</para>
      /// <para><b>Windows Server 2003 and Windows XP:</b> This context is not supported by Windows Server 2003 and Windows XP.</para>
      /// </summary>
      ClientAccessibleWriters = VssVolumeSnapshotAttributes.Persistent | VssVolumeSnapshotAttributes.ClientAccessible | VssVolumeSnapshotAttributes.NoAutoRelease,

      /// <summary>
      /// All types of currently live shadow copies are available for administrative operations, such as shadow copy queries 
      /// (see the Query method in <see cref="IVssBackupComponents" />). <see cref="All"/> is a valid context for all VSS interfaces except 
      /// <see cref="IVssBackupComponents"/>::StartSnapshotSet and <see cref="IVssBackupComponents"/>::DoSnapshotSet.
      /// </summary>
      All = 0xFFFFFFFF,
   }
}
