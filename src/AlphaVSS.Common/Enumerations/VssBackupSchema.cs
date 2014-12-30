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
   /// <summary>The <see cref="VssBackupSchema"/> enumeration is used by a writer to indicate the types of backup operations it can participate in. 
   /// The supported kinds of backup are expressed as a bit mask (or bitwise OR) of <see cref="VssBackupSchema"/> values.</summary>
   /// <remarks>
   ///     <para>
   ///         <note>
   ///             <b>Windows XP:</b> This enumeration is not available until Windows Server 2003 or later.
   ///         </note>
   ///     </para>
   ///     <para>
   ///         Writer set their backup schemas with calls to <c>IVssCreateWriterMetadata.SetBackupSchema"</c>.
   ///     </para>
   ///     <para>
   ///         Requesters use <see cref="IVssExamineWriterMetadata.BackupSchema"/> to determine the backup schema that a writer supports.
   ///     </para>
   ///     <para>
   ///         For a specific kind of backup operation to be supported, the writer must support the corresponding schema, and the 
   ///         requester must set the corresponding backup type.
   ///     </para>
   ///     <para>
   ///         For example, to involve a writer in an incremental backup operation, the requester must set the backup type to 
   ///         <see cref="VssBackupType.Incremental"/>, and the writer should have a backup schema that includes <see cref="Incremental"/>.
   ///     </para>
   ///     <para>
   ///         A writer that does not support the backup schema corresponding to a requester's backup type should treat the backup operation 
   ///         that is being performed as if it were a default (full) backup. If the desired backup type is not supported by the writer's 
   ///         backup schema, the requester can either perform a full backup for this writer or exclude the writer from the backup operation. 
   ///         A requester can exclude a writer by selecting none of the writer's components, or by disabling the writer 
   ///         (see <see cref="IVssBackupComponents.DisableWriterClasses"/> or 
   ///         <see cref="IVssBackupComponents.DisableWriterInstances"/>).
   ///     </para>
   /// </remarks>
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1714"), Flags]
   public enum VssBackupSchema
   {
      /// <summary>
      /// The writer supports a simple full backup and restoration of entire files (as defined by a <see cref="VssBackupType"/> value of <see cref="VssBackupType.Full"/>). 
      /// This backup scheme can be used as the basis of an incremental or differential backup. This is the default value.
      /// </summary>
      Undefined = 0x00000000,
      /// <summary>
      /// The writer supports differential backups (corresponding to the <see cref="VssBackupType"/> value <see cref="VssBackupType.Differential"/>). 
      /// Files created or changed since the last full backup are saved. Files are not marked as having been backed up. This setting does not preclude mixing of incremental and differential backups.
      /// </summary>
      Differential = 0x00000001,
      /// <summary>
      /// The writer supports incremental backups (corresponding to the <see cref="VssBackupType"/> value <see cref="VssBackupType.Incremental"/>). Files created or changed since the last full or incremental backup are saved. Files are marked as having been backed up. This setting does not preclude mixing of incremental and differential backups.
      /// </summary>
      Incremental = 0x00000002,
      /// <summary>
      /// The writer supports both differential and incremental backup schemas, but only exclusively: for example, you cannot follow a differential backup with an incremental one. 
      /// A writer cannot support this schema if it does not support both incremental and differential schemas (<see cref="Differential" /> | <see cref="Incremental"/>).
      /// </summary>
      ExclusiveIncrementalDifferential = 0x00000004,
      /// <summary>
      /// <para>The writer supports backups that involve only the log files it manages (corresponding to a <see cref="VssBackupType"/> value of <see cref="VssBackupType.Log"/>). </para>
      /// </summary>
      Log = 0x00000008,
      /// <summary>
      /// Similar to the default backup schema (<see cref="VssBackupType.Undefined"/>), the writer supports copy backup operations 
      /// (corresponding to <see cref="VssBackupType.Copy"/>) where file access information (such as information as to when a file was 
      /// last backed up) will not be updated either in the writer's own state information or in the file system information. This type of 
      /// backup cannot be used as the basis of an incremental or differential backup.
      /// </summary>
      Copy = 0x00000010,

      /// <summary>
      ///     <para>
      ///         A writer supports using the VSS time-stamp mechanism when evaluating if a file should be included in 
      ///         differential or incremental operations (corresponding to <see cref="VssBackupType.Differential" /> and 
      ///         <see cref="VssBackupType.Incremental"/>, respectively) using the <see cref="IVssComponent.PreviousBackupStamp" />,
      ///         <see cref="IVssComponent.BackupStamp"/> setters, and the <see cref="IVssBackupComponents.SetPreviousBackupStamp"/> method.
      ///     </para>
      ///     <para>
      ///         A writer cannot support this schema if it does not support either differential or incremental backup schemas 
      ///         (<see cref="Differential"/> or <see cref="Incremental"/>).
      ///     </para>     
      ///
      /// </summary>
      Timestamped = 0x00000020,

      /// <summary>
      ///     <para>
      ///         When implementing incremental or differential backups with differenced files, a writer can provide last modification 
      ///         time information for files (using <c>IVssComponent.AddDifferencedFileByLastModifyTime</c>).
      ///         A requester then can use <see cref="IVssComponent.DifferencedFiles"/> to obtain candidate files and information 
      ///         about their last modification data. The requester can use this information (along with any records about 
      ///         previous backup operations it maintains) to decide if a file should be included in incremental and differential backups.
      ///     </para>
      ///     <para>
      ///         This scheme does not apply to partial file implementations of incremental and differential backup operations.
      ///     </para>
      ///     <para>
      ///         A writer cannot support this schema if it does not support either incremental or differential backup 
      ///         schemas (<see cref="Differential"/> or <see cref="Incremental"/>.
      ///     </para>
      /// </summary>
      LastModify = 0x00000040,

      /// <summary>
      /// Reserved for system use.
      /// </summary>
      Lsn = 0x00000080,

      /// <summary>
      ///     The writer supports a requester changing the target for file restoration using 
      ///     <see cref="IVssBackupComponents.AddNewTarget"/>.
      /// </summary>       
      WriterSupportsNewTarget = 0x00000100,


      /// <summary>
      ///     <para>
      ///         The writer supports running multiple writer instances with the same class ID, and it supports a 
      ///         requester moving a component to a different writer instance at restore time using 
      ///         <see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetSelectedForRestore"/>.
      ///     </para>    
      ///      <para>
      ///         <b>Windows Server 2003:</b> This value is not supported until Windows Server 2003 SP1.
      ///      </para>
      /// </summary>
      WriterSupportsRestoreWithMove = 0x00000200,

      /// <summary>
      ///     <para>
      ///         The writer supports backing up data that is part of the system state, but that can also 
      ///         be backed up independently of the system state.
      ///     </para>
      ///     <para>
      ///         <b>Windows Server 2003:</b>  This value is not supported until Windows Vista.
      ///     </para>
      /// </summary>
      IndependentSystemState = 0x00000400,

      /// <summary>
      ///     <para>
      ///         The writer supports a requester setting a roll-forward restore point using <see cref="IVssBackupComponents.SetRollForward"/>.
      ///     </para>
      ///     <para>
      ///         <b>Windows Server 2003:</b>  This value is not supported until Windows Vista.
      ///     </para>
      /// </summary>
      RollForwardRestore = 0x00001000,

      /// <summary>
      ///     <para>
      ///         The writer supports a requester setting a restore name using <see cref="IVssBackupComponents.SetRestoreName"/>.
      ///     </para>
      ///     <para>
      ///         <b>Windows Server 2003:</b>  This value is not supported until Windows Vista.
      ///     </para>
      /// </summary>
      RestoreRename = 0x00002000,

      /// <summary>
      ///     <para>
      ///         The writer supports a requester setting authoritative restore using <see cref="IVssBackupComponents.SetAuthoritativeRestore"/>.
      ///     </para>
      ///     <para>
      ///         <b>Windows Server 2003:</b> This value is not supported until Windows Vista.
      ///     </para>
      /// </summary>
      AuthoritativeRestore = 0x00004000,

      /// <summary>
      ///     <para>
      ///         The writer supports multiple unsynchronized restore events.
      ///     </para>
      ///     <para>
      ///         <b>Windows Vista and Windows Server 2003:</b>  This value is not supported until Windows Server 2008.
      ///     </para>
      /// </summary>
      WriterSupportsParallelRestores = 0x00008000
   }
}