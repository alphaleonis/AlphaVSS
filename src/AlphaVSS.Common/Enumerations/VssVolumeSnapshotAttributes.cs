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
   /// Allows additional attributes to be specified for a shadow copy. The context of a shadow copy (as set by the SetContext method in <see cref="IVssBackupComponents" /> method) 
   /// may be modified by a bitmask that contains a valid combination of <see cref="VssVolumeSnapshotAttributes" /> and 
   /// <see cref="VssSnapshotContext" /> enumeration values.
   /// </summary>
   /// <remarks>In the VSS API, these values are represented by the enumeration 
   /// <seealso cref="VssSnapshotContext" /> </remarks>
   [System.Flags]
   public enum VssVolumeSnapshotAttributes : int
   {
      /// <summary>The shadow copy is persistent across reboots.
      /// This attribute is automatically set for <see cref="VssSnapshotContext" /> contexts of <c>AppRollback</c>
      /// <c>ClientAccessible</c>, <c>ClientAccessibleWriters</c> and <c>NasRollback</c>.
      /// This attribute should not be used explicitly by requesters when setting the context of a shadow copy.
      ///</summary>
      Persistent = 0x00000001,
      /// <summary>
      /// <para>Auto-recovery is disabled for the shadow copy.</para>
      /// <para>A requester can modify a shadow copy context with a bitwise OR of this attribute. By doing this, the requester instructs 
      /// VSS to make the shadow copy read-only immediately after it is created, without allowing writers or other applications to update 
      /// components in the shadow copy.</para>
      /// <para>Disabling auto-recovery can cause the shadow copy to be in an inconsistent state if any of its components are involved in 
      /// transactional database operations, such as transactional read and write operations managed by Transactional NTFS (TxF). 
      /// This is because disabling auto-recovery prevents incomplete transactions from being rolled back.</para>
      /// <para>Disabling auto-recovery also prevents writers from excluding files from the shadow copy. </para>
      /// <para><b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.</para>
      /// </summary>
      NoAutoRecovery = 0x00000002,
      /// <summary>
      /// <para>The specified shadow copy is a client-accessible copy, supports Shadow Copies for Shared Folders, and should not be exposed. </para>
      /// <para>This attribute is automatically set for <c>ClientAccessible</c> and <c>ClientAccessibleWriters</c>.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      ClientAccessible = 0x00000004,
      /// <summary>
      /// <para>The shadow copy is not automatically deleted when the shadow copy requester process ends. 
      /// The shadow copy can be deleted only by a call to <see cref="IVssBackupComponents.DeleteSnapshot"/> or 
      /// <see cref="IVssBackupComponents.DeleteSnapshotSet"/>.</para>
      /// <para>This attribute is automatically set for <see cref="VssSnapshotContext"/> contexts of <c>Rollback</c>,
      /// <c>ClientAccessible</c>, <c>ClientAccessibleWriters</c> and <c>Rollback</c>.</para>
      /// </summary>
      NoAutoRelease = 0x00000008,
      /// <summary>
      /// <para>No writers are involved in creating the shadow copy. </para>
      /// <para>This attribute is automatically set for <see cref="VssSnapshotContext"/> contexts of <c>NasRollback</c>,
      /// <c>FileShareBackup</c> and <c>ClientAccessible</c>.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      NoWriters = 0x00000010,
      /// <summary>
      /// <para>The shadow copy is to be transported and therefore should not be surfaced locally. </para>
      /// <para>This attribute can be used explicitly by requesters when setting the context of a shadow copy, if the provider for 
      /// shadow copy supports transportable shadow copies.</para>
      /// <para><b>Windows Server 2003, Standard Edition, Windows Server 2003, Web Edition, and Windows XP:</b> This attribute is not supported. 
      /// All editions of Windows Server 2003 SP1 support this attribute.</para>
      /// </summary>
      Transportable = 0x00000020,
      /// <summary>
      /// <para>The shadow copy is not currently exposed. </para>
      /// <para>Unless the shadow copy is explicitly exposed or mounted, this attribute is set for all shadow copies.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      NotSurfaced = 0x00000040,
      /// <summary>
      /// <para>The shadow copy is not transacted.</para>
      /// <para>A requester can modify a shadow copy context with a bitwise OR of this attribute. By doing this, the requester instructs VSS to 
      /// disable built-in integration between VSS and transaction and resource managers.</para>
      /// <para>Setting this attribute guarantees that the requester will not receive <see cref="VssTransactionFreezeTimeoutException"/> errors. However, it may 
      /// cause unwanted consequences, such as the loss of transactional integrity or even data loss.</para>
      /// <para><b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.</para>
      /// </summary>
      NotTransacted = 0x00000080,
      /// <summary>
      /// <para>Indicates that a given provider is a hardware-based provider. </para>
      /// <para>This attribute is automatically set for hardware-based providers.</para>
      /// <para>This enumeration value cannot be used to manually set the context (using the <see cref="IVssBackupComponents.SetContext(Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes)"/> method) 
      /// of a shadow copy by a bit mask (or bitwise OR) of this enumeration value and a valid shadow copy context value from 
      /// <see cref="VssSnapshotContext" />.</para>
      /// </summary>
      HardwareAssisted = 0x00010000,
      /// <summary>
      /// <para>Indicates that a given provider uses differential data or a copy-on-write mechanism to implement shadow copies. </para>
      /// <para>A requester can modify a shadow copy context with a bitwise OR of this attribute. By doing this, the requester instructs providers 
      /// to create a shadow copy using a differential implementation. If no shadow copy provider installed on the system supports the 
      /// requested attributes, a <see cref="VssVolumeNotSupportedException"/> error will be returned to <see cref="IVssBackupComponents.AddToSnapshotSet(System.String,System.Guid)"/>.</para>
      /// </summary>
      Differential = 0x00020000,
      /// <summary>
      /// <para>Indicates that a given provider uses a PLEX or mirrored split mechanism to implement shadow copies. </para>
      /// <para>A requester can modify a shadow copy context with a bitwise OR of this attribute. By doing this, the requester instructs the providers to create a shadow copy using a PLEX implementation. If no shadow copy provider installed on the system supports the requested 
      /// attributes, a <see cref="VssVolumeNotSupportedException"/> error will be returned to <see cref="IVssBackupComponents.AddToSnapshotSet(System.String,System.Guid)"/>.</para>
      /// </summary>
      Plex = 0x00040000,
      /// <summary>
      /// <para>The shadow copy of the volume was imported onto this machine using the <see cref="IVssBackupComponents.ImportSnapshots"/> method 
      /// rather than created using the <see cref="IVssBackupComponents.DoSnapshotSet"/> method. </para>
      /// <para>This attribute is automatically set if a shadow copy is imported.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      Imported = 0x00080000,
      /// <summary>
      /// <para>The shadow copy is locally exposed. If this bit flag and the <c>ExposedRemotely</c> bit flag are not set, 
      /// the shadow copy is hidden. </para>
      /// <para>The attribute is automatically added to a shadow copy context upon calling the <see cref="IVssBackupComponents.ExposeSnapshot"/>
      /// method to expose a shadow copy locally.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      ExposedLocally = 0x00100000,
      /// <summary>
      /// <para>The shadow copy is remotely exposed. If this bit flag and the <c>ExposedLocally</c> bit flag are not set, the shadow copy is hidden. </para>
      /// <para>The attribute is automatically added to a shadow copy context upon calling the <see cref="IVssBackupComponents.ExposeSnapshot"/>
      /// method to expose a shadow copy locally.</para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      ExposedRemotely = 0x00200000,
      /// <summary>
      /// <para>Indicates that the writer will need to auto-recover the component in <c>CVssWriter::OnPostSnapshot</c>. </para>
      /// <para>This attribute should not be used explicitly by requesters when setting the context of a shadow copy.</para>
      /// </summary>
      AutoRecover = 0x00400000,
      /// <summary>
      /// <para>Indicates that the writer will need to auto-recover the component in <c>CVssWriter::OnPostSnapshot</c> if the shadow copy is being used 
      /// for rollback (for data mining, for example). </para>
      /// <para>A requester would set this flag in the shadow copy context to indicate that the shadow copy is being created for a non-backup 
      /// purpose such as data mining.</para>
      /// </summary>
      RollbackRecovery = 0x00800000,
      /// <summary>
      /// <para>Reserved for system use.</para>
      /// <para><b>Windows Vista, Windows Server 2003, and Windows XP:</b>  This value is not supported until Windows Server 2008.</para>
      /// </summary>
      DelayedPostSnapshot = 0x01000000,
      /// <summary>
      /// <para>Indicates that TxF recovery should be enforced during shadow copy creation.</para>
      /// <para><b>Windows Vista, Windows Server 2003, and Windows XP:</b>  This value is not supported until Windows Server 2008.</para>
      /// </summary>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Tx")]
      TxFRecovery = 0x02000000
   }
}
