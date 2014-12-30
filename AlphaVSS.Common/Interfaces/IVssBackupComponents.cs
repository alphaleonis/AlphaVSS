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
using System.Collections.Generic;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// The <see cref="IVssBackupComponents"/> class is used by a requester to poll writers about file status and to run backup/restore operations.
   /// </summary>
   /// <seealso cref="VssUtils"/>
   ///   
   /// <seealso cref="IVssImplementation"/>
   /// <remarks>
   ///   <para>
   /// A <see cref="IVssBackupComponents"/> object can be used for only a single backup, restore, or Query operation.
   ///   </para>
   ///   <para>
   /// After the backup, restore, or Query operation has either successfully finished or been explicitly terminated, a requester must
   /// release the <see cref="IVssBackupComponents"/> object by calling <c>Dispose()</c>.
   /// A <see cref="IVssBackupComponents"/> object must not be reused. For example, you cannot perform a backup or restore operation with the
   /// same <see cref="IVssBackupComponents"/> object that you have already used for a Query operation.
   ///   </para>
   ///   <para>
   /// For information on how to retrieve an instance of <see cref="IVssBackupComponents"/> for the current operating system, see
   ///   <see cref="VssUtils"/> and <see cref="IVssImplementation"/>.
   ///   </para>
   /// </remarks>
   public interface IVssBackupComponents : IDisposable
   {
      #region IVssBackupComponents members 

      /// <summary>
      /// <para>The <b>AbortBackup</b> method notifies VSS that a backup operation was terminated.</para>
      /// <para>
      ///		This method must be called if a backup operation terminates after the creation of a shadow copy set with 
      ///		<see cref="StartSnapshotSet" /> and before <see cref="DoSnapshotSet" />returns.
      /// </para>
      /// <para>
      ///		If AbortBackup is called and no shadow copy or backup operations are underway, it is ignored.
      /// </para>
      /// </summary>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized.</exception>
      void AbortBackup();

      /// <summary>
      ///		The <b>AddAlternativeLocationMapping</b> method is used by a requester to indicate that an alternate location 
      ///     mapping was used to restore all the members of a file set in a given component.
      /// </summary>
      /// <param name="writerId">Globally unique identifier (GUID) of the writer class that exported the component.</param> 
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      ///		<para>String containing the logical path to the component. The logical path can be <see langword="null"/>.</para>
      ///     <para>There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.</para>
      /// </param>
      /// <param name="componentName">The component name.</param>
      /// <param name="path">
      ///		<para>
      ///			The path to the directory that originally contained the 
      ///			file to be relocated. This path must be local to the VSS machine.
      ///		</para>
      ///		<para>
      ///			The path can contain environment variables (for example, %SystemRoot%) but cannot contain wildcard characters. 
      ///			UNC paths are not supported.
      ///		</para>
      ///		<para>
      ///			There is no requirement that the path end with a backslash ("\"). It is up to applications that retrieve 
      ///			this information to check.
      ///		</para>
      ///	</param>
      /// <param name="filespec">String containing the original file specification. A file specification cannot 
      /// contain directory specifications (for example, no backslashes) but can contain the ? and * wildcard characters.</param>
      /// <param name="recursive">
      ///		Boolean indicating whether the path specified by the <paramref name="path" /> parameter identifies only a single 
      ///		directory or if it indicates a hierarchy of directories to be traversed recursively. The Boolean is <c>true</c> if the 
      ///     path is treated as a hierarchy of directories to be traversed recursively and <c>false</c> if not.
      /// </param>
      /// <param name="destination">The name of the directory where the file will be relocated. This path must be local to the VSS machine. 
      /// UNC paths are not supported.</param>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified component does not exist.</exception>
      void AddAlternativeLocationMapping(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string path, string filespec, bool recursive, string destination);

      /// <summary>
      /// The <b>AddComponent</b> method is used to explicitly add to the backup set in the Backup Components Document all required 
      /// components (nonselectable for backup components without a selectable for backup ancestor), and such optional 
      /// (selectable for backup) components as the requester considers necessary. Members of component sets (components with 
      ///  a selectable for backup ancestor) are implicitly included in the backup set, but are not explicitly added to the Backup 
      /// Components Document.
      /// </summary>
      /// <param name="instanceId">Identifies a specific instance of a writer.</param>
      /// <param name="writerId">Writer class identifier.</param>
      /// <param name="componentType">Identifies the type of the component.</param>
      /// <param name="logicalPath">
      ///		<para>String containing the logical path of the selectable for backup component.</para>
      ///		<para>A logical path is not required when adding a component. Therefore, the value of this parameter can be <see langword="null"/>.</para>
      ///		<para>There are no restrictions on the characters that can a logical path.</para>
      ///	</param>
      /// <param name="componentName">
      ///		<para>String containing the name of the selectable for backup component.</para>
      ///		<para>The value of this parameter <b>cannot</b> be <see langword="nulL"/>.</para>
      ///		<para>There are no restrictions on the characters that can appear in a logical path.</para>
      ///	</param>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <exception cref="VssObjectAlreadyExistsException">The object is a duplicate. A component with the same logical path and component name already exists.</exception>
      void AddComponent(Guid instanceId, Guid writerId, VssComponentType componentType, string logicalPath, string componentName);

      /// <summary>
      /// The <b>AddNewTarget</b> method is used by a requester during a restore operation to indicate that the backup application plans 
      /// to restore files to a new location.
      /// </summary>
      /// <param name="writerId">Globally unique identifier (GUID) of the writer class containing the files that are to receive a new target.</param>
      /// <param name="componentType">Identifies the type of the component, see the documentation for <see cref="VssComponentType"/> for more information.</param>
      /// <param name="logicalPath">
      ///		<para>String containing the logical path of the component containing the files that are to receive a new restore target.</para>
      ///     <para>The value of the string containing the logical path used here should be the same as was used when the component was 
      ///           added to the backup set using <see cref="AddComponent"/>.</para>
      ///     <para>The logical path can be <see langword="null"/>.</para>
      ///     <para>There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.</para>
      /// </param>
      /// <param name="componentName">
      ///		<para>The name of the component containing the files that are to receive a new restore target.</para>
      ///		<para>The string should not be <see langword="null"/> and should contain the same component name as was used when the 
      ///			  component was added to the backup set using <see cref="AddComponent"/>.</para>
      ///		<para>There are no restrictions on the characters that can appear in a non-NULL logical path.</para>
      /// </param>
      /// <param name="path">
      ///		<para>The name of the directory or directory hierarchy containing the files to receive a new restore target.</para>
      ///		<para>The path can contain environment variables (for example, %SystemRoot%) but cannot contain wildcard characters.</para>
      ///		<para>There is no requirement that the path end with a backslash ("\"). It is up to applications that retrieve this information to check.</para>
      ///	</param>
      /// <param name="fileName">
      ///		<para>The file specification of the files to receive a new restore target.</para>
      ///		<para>A file specification cannot contain directory specifications (for example, no backslashes) but can contain the ? and * wildcard characters.</para>
      /// </param>
      /// <param name="recursive">Boolean indicating whether only the files in the directory defined by <paramref name="path"/> and matching the file 
      /// specification provided by <paramref name="fileName"/> are to receive a new restore target, or if all files in the hierarchy defined 
      /// by <paramref name="path"/>and matching the file specification provided by <paramref name="fileName" /> are to receive a new restore target. 
      /// </param>
      /// <param name="alternatePath">The fully qualified path of the new restore target directory.</param>
      /// <remarks><note>This method is not supported on Windows XP</note></remarks>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssObjectNotFoundException">The component does not exist or the path and file specification do not match a component and file specification in the component.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <exception cref="NotSupportedException">The operation is not supported on the current operating system.</exception>
      void AddNewTarget(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string path, string fileName, bool recursive, string alternatePath);

      /// <summary>
      /// The <b>AddRestoreSubcomponent</b> method indicates that a Subcomponent member of a component set, which had been marked as 
      /// nonselectable for backup but is marked selectable for restore, is to be restored irrespective of whether any other member 
      /// of the component set will be restored.
      /// </summary>
      /// <param name="writerId">Globally unique identifier (GUID) of the writer class containing the files that are to receive a new target.</param>
      /// <param name="componentType">Identifies the type of the component, see the documentation for <see cref="VssComponentType"/> for more information.</param>
      /// <param name="logicalPath">
      ///		<para>String containing the logical path of the component in the backup document that defines the backup component set containing the Subcomponent to be added for restore.</para>
      ///     <para>The logical path can be <see langword="null"/>.</para>
      ///     <para>There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.</para>
      /// </param>
      /// <param name="componentName">
      ///		<para>The logical path of the component in the backup document that defines the backup component set containing the Subcomponent to be added for restore.</para>
      ///		<para>The string should not be <see langword="null"/> and should contain the same component name as was used when the 
      ///			  component was added to the backup set using <see cref="AddComponent"/>.</para>
      ///		<para>There are no restrictions on the characters that can appear in a non-NULL component name.</para>
      /// </param>
      /// <param name="subcomponentLogicalPath">
      ///		<para>The logical path of the Subcomponent to be added for restore. </para>
      ///		<para>A logical path is required when adding a Subcomponent. Therefore, the value of this parameter cannot be <see langword="null"/>.</para>
      ///		<para>There are no restrictions on the characters that can appear in a non-NULL logical path.</para>
      ///	</param>
      /// <param name="subcomponentName">
      ///		<para>The logical name of the Subcomponent to be added for restore.</para>
      ///		<para>The value of this parameter cannot be <see langword="null"/>.</para>
      ///		<para>There are no restrictions on the characters that can appear in a non-NULL component name.</para>
      ///	</param>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssObjectNotFoundException">The component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void AddRestoreSubcomponent(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string subcomponentLogicalPath, string subcomponentName);

      /// <summary>
      /// The <see cref="AddToSnapshotSet(string, System.Guid)"/> method adds an original volume to the shadow copy set. 
      /// </summary>
      /// <param name="volumeName">String containing the name of the volume to be shadow copied. The name must be in one of the following formats:
      ///		<list type="bullet">
      ///			<item><description>The path of a volume mount point with a backslash (\)</description></item>
      ///			<item><description>A drive letter with backslash (\), for example, D:\</description></item>
      ///			<item><description>A unique volume name of the form "\\?\Volume{GUID}\" (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      ///		</list>
      /// </param>
      /// <param name="providerId">The provider to be used. <see cref="Guid.Empty" /> can be used, in which case the default provider will be used.</param>
      /// <returns>Identifier of the added shadow copy.</returns>
      /// <remarks>
      /// 	<para>
      /// 		The maximum number of shadow copies in a single shadow copy set is 64.
      /// 	</para>
      /// 	<para>If <paramref name="providerId"/> is <see cref="Guid.Empty"/>, the default provider is selected according to the following algorithm:
      /// 		<list type="numbered">
      /// 			<item><description>If any hardware-based provider supports the given volume, it is selected.</description></item>
      /// 			<item><description>If there is no hardware-based provider available, if any software-based provider supports the given volume, it is selected.</description></item>
      /// 			<item><description>If there is no hardware-based provider or software-based provider available, the system provider is selected. (There is only one preinstalled system provider, which must support all nonremovable local volumes.)</description></item>
      /// 		</list>
      /// 	</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">Caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssMaximumNumberOfVolumesReachedException">The maximum number of volumes has been added to the shadow copy set. The specified volume was not added to the shadow copy set.</exception>
      /// <exception cref="VssMaximumNumberOfSnapshotsReachedException">The volume has been added to the maximum number of shadow copy sets. The specified volume was not added to the shadow copy set.</exception>
      /// <exception cref="VssObjectNotFoundException"><paramref name="volumeName" /> does not correspond to an existing volume.</exception>
      /// <exception cref="VssProviderNotRegisteredException"><paramref name="providerId" /> does not correspond to a registered provider.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssVolumeNotSupportedException">The value of the <paramref name="providerId"/> parameter is <see cref="Guid.Empty" /> and no VSS provider indicates that it supports the specified volume.</exception>
      /// <exception cref="VssVolumeNotSupportedByProviderException">The volume is not supported by the specified provider.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">The provider returned an unexpected error code.</exception>
      Guid AddToSnapshotSet(string volumeName, Guid providerId);

      /// <summary>
      /// The <see cref="AddToSnapshotSet(string)"/> method adds an original volume to the shadow copy set using the default provider.
      /// </summary>
      /// <param name="volumeName">String containing the name of the volume to be shadow copied. The name must be in one of the following formats:
      ///		<list type="bullet">
      ///			<item><description>The path of a volume mount point with a backslash (\)</description></item>
      ///			<item><description>A drive letter with backslash (\), for example, D:\</description></item>
      ///			<item><description>A unique volume name of the form "\\?\Volume{GUID}\" (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      ///		</list>
      /// </param>
      /// <returns>Identifier of the added shadow copy.</returns>
      /// <remarks>
      /// 	<para>
      /// 		The maximum number of shadow copies in a single shadow copy set is 64.
      /// 	</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">Caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssMaximumNumberOfVolumesReachedException">The maximum number of volumes has been added to the shadow copy set. The specified volume was not added to the shadow copy set.</exception>
      /// <exception cref="VssMaximumNumberOfSnapshotsReachedException">The volume has been added to the maximum number of shadow copy sets. The specified volume was not added to the shadow copy set.</exception>
      /// <exception cref="VssObjectNotFoundException"><paramref name="volumeName" /> does not correspond to an existing volume.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssVolumeNotSupportedException">No VSS provider indicates that it supports the specified volume.</exception>
      /// <exception cref="VssVolumeNotSupportedByProviderException">The volume is not supported by the specified provider.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">The provider returned an unexpected error code.</exception>        
      Guid AddToSnapshotSet(string volumeName);

      /// <summary>
      /// This method causes VSS to generate a <b>BackupComplete</b> event, which signals writers that the backup 
      /// process has completed. 
      /// </summary>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssUnexpectedWriterErrorException">An unexpected error occurred during communication with writers. The error code is logged in the error log file.</exception>
      void BackupComplete();

      /// <summary>
      /// This method asynchronously causes VSS to generate a <b>BackupComplete</b> event, which signals writers that the backup
      /// process has completed.
      /// </summary>
      /// <remarks>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndBackupComplete"/> method to release operating system resources used for this asynchronous operation. 
      /// <see cref="EndBackupComplete"/> must be called once for every call to <see cref="BeginBackupComplete"/>. You can do this either by using the same code that called <b>BeginBackupComplete</b> or 
      /// in a callback passed to <b>BeginBackupComplete</b>.
      /// </remarks>
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>
      /// An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.
      /// </returns>
      IVssAsyncResult BeginBackupComplete(AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndBackupComplete</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginBackupComplete"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssUnexpectedWriterErrorException">An unexpected error occurred during communication with writers. The error code is logged in the error log file.</exception>
      void EndBackupComplete(IAsyncResult asyncResult);


      /// <overloads>
      /// <summary>
      /// The <c>BreakSnapshotSet</c> method causes the existence of a shadow copy set to be "forgotten" by VSS.
      /// </summary>
      /// </overloads>
      /// <summary>
      /// The <c>BreakSnapshotSet</c> method causes the existence of a shadow copy set to be "forgotten" by VSS.
      /// </summary>
      /// <param name="snapshotSetId">Shadow copy set identifier.</param>
      /// <remarks>BreakSnapshotSet can be used only for shadow copies created by a hardware shadow copy provider. This method makes these shadow copies regular volumes.</remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssProviderVetoException">The shadow copy was created by a software provider and cannot be broken.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      void BreakSnapshotSet(Guid snapshotSetId);

      /// <summary>
      ///		The <c>DeleteSnapshot</c> method deletes a shadow copy.. 
      /// </summary>
      /// <param name="snapshotId">Identifier of the shadow copy to be deleted.</param>
      /// <param name="forceDelete">If the value of this parameter is <see langword="true"/>, the provider will do everything possible to delete the shadow copy. If it is <see langword="false"/>, no additional effort will be made.</param>
      /// <remarks>
      /// 	<para>
      /// 		The requester is responsible for serializing the delete shadow copy operation.
      /// 	</para>
      /// 	<para>
      /// 		During a backup, shadow copies are automatically released as soon as the <see cref="IVssBackupComponents"/> instance is 
      /// 		disposed. In this case, it is not necessary to explicitly delete shadow copies. 
      /// 	</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      void DeleteSnapshot(Guid snapshotId, bool forceDelete);

      /// <summary>
      ///		The <c>DeleteSnapshotSet</c> method deletes a shadow copy set including any shadow copies in that set.
      /// </summary>
      /// <param name="snapshotSetId">Identifier of the shadow copy set to be deleted.</param>
      /// <param name="forceDelete">
      ///     If the value of this parameter is <see langword="true"/>, the provider will do everything possible to 
      ///     delete the shadow copies in a shadow copy set. If it is <see langword="false"/>, no additional effort will be made.
      /// </param>
      /// <remarks>
      /// 	<para>
      /// 		Multiple shadow copies in a shadow copy set are deleted sequentially. If an error occurs during one of these individual 
      /// 		deletions, <b>DeleteSnapshotSet</b> will throw an exception immediately; no attempt will be made to delete any remaining shadow copies. 
      /// 		The identifier of the undeleted shadow copy can be found in the instance of <see cref="VssDeleteSnapshotsFailedException"/> thrown.
      /// 	</para>
      /// 	<para>
      /// 		The requester is responsible for serializing the delete shadow copy operation.
      /// 	</para>
      /// 	<para>
      /// 		During a backup, shadow copies are automatically released as soon as the <see cref="IVssBackupComponents"/> instance is 
      /// 		disposed. In this case, it is not necessary to explicitly delete shadow copies. 
      /// 	</para>
      /// </remarks>
      /// <exception cref="VssDeleteSnapshotsFailedException">The deletion failed. This is the only exception actually thrown by this method. It 
      /// wraps one of the other exceptions listed in this section as its inner exception.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      /// <returns>The total number of snapshots that were deleted</returns>
      int DeleteSnapshotSet(Guid snapshotSetId, bool forceDelete);

      /// <summary>
      /// This method prevents a specific class of writers from receiving any events.
      /// </summary>
      /// <param name="writerClassIds">An array containing one or more writer class identifiers.</param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      void DisableWriterClasses(params Guid[] writerClassIds);

      /// <summary>
      /// This method disables a specified writer instance or instances.
      /// </summary>
      /// <param name="writerInstanceIds">An array containing one or more writer instance identifiers.</param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      void DisableWriterInstances(params Guid[] writerInstanceIds);

      /// <summary>
      /// Commits all shadow copies in this set simultaneously. 
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object has not been initialized or the prerequisite calls for a given shadow copy context have not been made prior to calling <b>DoSnapshotSet</b>. </exception>
      /// <exception cref="VssInsufficientStorageException">The system or provider has insufficient storage space. If possible delete any old or unnecessary persistent shadow copies and try again.</exception>
      /// <exception cref="VssFlushWritesTimeoutException">The system was unable to flush I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssHoldWritesTimeoutException">The system was unable to hold I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssProviderVetoException">The provider was unable to perform the request at this time. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssRebootRequiredException">
      ///		<para>
      ///			The provider encountered an error that requires the user to restart the computer.
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssTransactionFreezeTimeoutException">
      ///		<para>
      ///			The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssTransactionThawTimeoutException">
      ///		<para>
      ///			The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssUnexpectedProviderErrorException">The provider returned an unexpected error code. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception> 
      void DoSnapshotSet();

      /// <summary>
      /// Commits all shadow copies in this set simultaneously as an asynchronous operation.
      /// </summary>
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      /// <remarks>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndDoSnapshotSet"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndDoSnapshotSet"/> must be called once for every call to <see cref="BeginDoSnapshotSet"/>. You can do this either by using the same code that called <b>BeginDoSnapshotSet</b> or
      /// in a callback passed to <b>BeginDoSnapshotSet</b>.
      /// </remarks>
      IVssAsyncResult BeginDoSnapshotSet(AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndDoSnapshotSet</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginDoSnapshotSet"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object has not been initialized or the prerequisite calls for a given shadow copy context have not been made prior to calling <b>DoSnapshotSet</b>. </exception>
      /// <exception cref="VssInsufficientStorageException">The system or provider has insufficient storage space. If possible delete any old or unnecessary persistent shadow copies and try again.</exception>
      /// <exception cref="VssFlushWritesTimeoutException">The system was unable to flush I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssHoldWritesTimeoutException">The system was unable to hold I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssProviderVetoException">The provider was unable to perform the request at this time. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
      /// <exception cref="VssRebootRequiredException">
      ///		<para>
      ///			The provider encountered an error that requires the user to restart the computer.
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssTransactionFreezeTimeoutException">
      ///		<para>
      ///			The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssTransactionThawTimeoutException">
      ///		<para>
      ///			The system was unable to freeze the Distributed Transaction Coordinator (DTC) or the Kernel Transaction Manager (KTM).
      ///		</para>
      ///		<para>
      ///		    <b>Windows Server 2003 and Windows XP:</b>  This value is not supported until Windows Vista.
      ///		</para>
      /// </exception>
      /// <exception cref="VssUnexpectedProviderErrorException">The provider returned an unexpected error code. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception> 
      void EndDoSnapshotSet(IAsyncResult asyncResult);

      /// <summary>
      /// The <b>EnableWriterClasses</b> method enables the specified writers to receive all events.
      /// </summary>
      /// <param name="writerClassIds">An array containing one or more writer class identifiers.</param>
      /// <remarks>
      ///
      ///	<para>
      ///		Once this method is called, only enabled writer classes are subsequently called.
      ///	</para>
      ///	<para>
      ///		<see cref="DisableWriterClasses"/> must be called prior to <see cref="GatherWriterMetadata"/>. To obtain information about the writers 
      ///		currently running on the system, it may be necessary to call <see cref="GatherWriterMetadata"/> from another instance of the 
      ///		<see cref="IVssBackupComponents"/> class.
      ///	</para>
      ///	<para>
      ///		After <see cref="GatherWriterMetadata"/> is called, these calls have no effect.
      ///	</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      void EnableWriterClasses(params Guid[] writerClassIds);

      /// <summary>
      /// The <b>ExposeSnapshot</b> method exposes a shadow copy either by mounting it as a device on a drive letter or mount point, or 
      /// as a file share. 
      /// </summary>
      /// <param name="snapshotId">Shadow copy identifier.</param>
      /// <param name="pathFromRoot">
      ///		<para>The path to the portion of the volume made available when exposing a shadow copy as a file share. The value of this parameter must be NULL when exposing a shadow copy locally; that is, by mounting to a drive letter or a mount point.</para>
      ///     <para>The path cannot contain environment variables (for example, %MyEnv%) or wildcard characters.</para>
      ///		<para>There is no requirement that the path end with a backslash ("\"). It is up to applications that retrieve this information to check.</para>
      /// </param>
      /// <param name="attributes">Attributes of the exposed shadow copy indicating whether it is exposed locally or remotely. The value must 
      /// be either the <see cref="VssVolumeSnapshotAttributes.ExposedLocally" /> or the <see cref="VssVolumeSnapshotAttributes.ExposedRemotely" /> 
      /// value of <see cref="VssVolumeSnapshotAttributes" />.</param>
      /// <param name="expose">When a shadow copy is exposed as a file share, the value of this parameter is the share name. If a shadow copy 
      /// is exposed by mounting it as a device, the parameter value is a drive letter followed by a colon, for example, "X:" or a mount point 
      /// path (for example, "X:\a\b"). If the value of this parameter is <see langword="null"/>, then VSS determines the share name or drive 
      /// letter if the <paramref name="attributes" /> parameter is <see cref="VssVolumeSnapshotAttributes.ExposedRemotely" />. </param>
      /// <returns>The exposed name of the shadow copy. This is either a share name, a drive letter followed by a colon, or a mount point.</returns>
      /// <remarks>
      /// <para>When exposing a persistent shadow copy, it remains exposed through subsequent boots.</para>
      /// <para>When exposing a shadow copy of a volume, the shadow copy may be treated either as a mountable device or as a file system available for file sharing.</para>
      /// <para>When it is exposed as a device, as with other mountable devices, the shadow copy of a volume is exposed at its mount point starting with its root.</para>
      /// <para>When exposed as a file share, subsets (indicated by <paramref name="pathFromRoot" />) of the volume can be shared.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      string ExposeSnapshot(Guid snapshotId, string pathFromRoot, VssVolumeSnapshotAttributes attributes, string expose);

      /// <summary>
      ///     The <c>FreeWriterMetadata</c> method frees system resources allocated when <see cref="GatherWriterMetadata" /> was called.
      /// </summary>
      /// <remarks>
      /// 	<para>
      /// 		This method should never be called prior to the completion of <see cref="IVssBackupComponents.GatherWriterMetadata"/>. 
      /// 		The result of calling the method prior to completion of the metadata gather is undefined.
      /// 	</para>
      /// 	<para>
      /// 		Once writer metadata has been freed, it cannot be recovered by the current instance of the <see cref="IVssBackupComponents"/> class. 
      /// 		It will be necessary to create a new instance of <see cref="IVssBackupComponents"/>, and call the 
      /// 		<see cref="IVssBackupComponents.GatherWriterMetadata"/> method again.
      /// 	</para>
      /// </remarks>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      void FreeWriterMetadata();

      /// <summary>
      ///     The <c>FreeWriterStatus</c> method frees system resources allocated during the call to <see cref="GatherWriterStatus" />.
      /// </summary>
      /// <exception cref="OutOfMemoryException">The caller is out of memory or other system resources.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      void FreeWriterStatus();

      /// <summary>
      /// 	The <see cref="GatherWriterMetadata"/> method prompts each writer to send the metadata they have collected. The method will generate an <c>Identify</c> event to communicate with writers.
      /// </summary>
      /// <remarks><see cref="GatherWriterMetadata"/> should be called only once during the lifetime of a given <see cref="IVssBackupComponents"/> object.</remarks>      
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
      void GatherWriterMetadata();

      /// <summary>
      /// 	The <see cref="BeginGatherWriterMetadata"/> method asynchronously prompts each writer to send the metadata they have collected. 
      /// 	The method will generate an <c>Identify</c> event to communicate with writers.
      /// </summary>
      /// <remarks>
      /// <para><see cref="BeginGatherWriterMetadata"/> should be called only once during the lifetime of a given <see cref="IVssBackupComponents"/> object.</para>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndGatherWriterMetadata"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndGatherWriterMetadata"/> must be called once for every call to <see cref="BeginGatherWriterMetadata"/>. You can do this either by using the same code that called <b>BeginGatherWriterMetadata</b> or
      /// in a callback passed to <b>BeginGatherWriterMetadata</b>.
      /// </para>
      /// </remarks>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginGatherWriterMetadata(AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndGatherWriterMetadata</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginGatherWriterMetadata"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
      void EndGatherWriterMetadata(IAsyncResult asyncResult);

      /// <summary>
      /// 	The <see cref="GatherWriterStatus"/> method prompts each writer to send a status message.
      /// </summary>
      /// <remarks>The caller of this method should also call <see cref="IVssBackupComponents.FreeWriterStatus"/> after receiving the status of each writer.</remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
      void GatherWriterStatus();

      /// <summary>
      /// 	The <see cref="BeginGatherWriterStatus"/> method asynchronously prompts each writer to send a status message.
      /// </summary>
      /// <remarks>
      /// <para>The caller of this method should also call <see cref="IVssBackupComponents.FreeWriterStatus"/> after receiving the status of each writer.</para>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndGatherWriterStatus"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndGatherWriterStatus"/> must be called once for every call to <see cref="BeginGatherWriterStatus"/>. You can do this either by using the same code that called <b>BeginGatherWriterStatus</b> or
      /// in a callback passed to <b>BeginGatherWriterStatus</b>.
      /// </para>
      /// </remarks>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginGatherWriterStatus(AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndGatherWriterStatus</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginGatherWriterStatus"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
      void EndGatherWriterStatus(IAsyncResult asyncResult);

      /// <summary>
      /// 	The <see cref="GetSnapshotProperties"/> method gets the properties of the specified shadow copy. 
      /// </summary>
      /// <param name="snapshotId">The identifier of the shadow copy of a volume as returned by <see cref="AddToSnapshotSet(string, System.Guid)"/>. </param>
      /// <returns>A <see cref="VssSnapshotProperties"/> instance containing the shadow copy properties.</returns>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      VssSnapshotProperties GetSnapshotProperties(Guid snapshotId);

      /// <summary>
      ///     A read-only list containing information about the components of each writer that has been stored in a requester's Backup Components Document.
      /// </summary>
      /// <remarks>
      /// 	<para>
      /// 		<see cref="WriterComponents"/> retrieves component information for a component stored in the Backup Components Document by earlier 
      /// 		calls to <see cref="IVssBackupComponents.AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The information in the components stored in the Backup Components Document is not static. If a writer updates a component during a 
      /// 		restore, that change will be reflected in the component retrieved by <see cref="WriterComponents"/>. This is in contrast with 
      /// 		component information found in the <see cref="IVssWMComponent"/> object returned by <see cref="IVssExamineWriterMetadata.Components"/>. 
      /// 		That information is read-only and comes from the Writer Metadata Document of a writer process.
      /// 	</para>
      /// 	<para>
      /// 		The <see cref="IVssWriterComponents"/> instances that are returned should not be cached, because the following 
      /// 		<see cref="IVssBackupComponents"/> methods cause the instances that are returned by <see cref="WriterComponents"/> to 
      /// 		be no longer valid:
      /// 		<list type="bullet">
      /// 			<item><description><see cref="IVssBackupComponents.PrepareForBackup"/></description></item>
      /// 			<item><description><see cref="IVssBackupComponents.DoSnapshotSet"/></description></item>
      /// 			<item><description><see cref="IVssBackupComponents.BackupComplete"/></description></item>
      /// 			<item><description><see cref="IVssBackupComponents.PreRestore"/></description></item>
      /// 			<item><description><see cref="IVssBackupComponents.PostRestore"/></description></item>
      /// 		</list>
      /// 	</para>
      /// 	<para>
      /// 		If you call one of these methods after you have retrieved a <see cref="IVssWriterComponents"/> instance by calling 
      /// 		<see cref="WriterComponents"/>, you cannot reuse that instance, because it is no longer valid. Instead, you must call 
      /// 		<see cref="WriterComponents"/> again to retrieve a new <see cref="IVssWriterComponents"/> instance.
      /// 	</para>
      /// </remarks>
      /// <value>
      ///     A read-only list containing information about the components of each writer that has been stored in a requester's Backup Components Document.
      ///		<note type="caution">This list must not be accessed after the <see cref="IVssBackupComponents"/> from which it 
      ///     was obtained has been disposed.</note>
      /// </value>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      IList<IVssWriterComponents> WriterComponents { get; }

      /// <summary>
      ///     A read-only list containing metadata for the writers running on the systsem.
      /// </summary>
      /// <value>
      ///     A read-only list containing metadata for the writers running on the system.
      ///		<note type="caution">This list must not be accessed after the <see cref="IVssBackupComponents"/> from which it 
      ///     was obtained has been disposed.</note>
      /// </value>
      /// <remarks>
      /// 	<para>
      /// 		A requester must call the asynchronous operation <see cref="IVssBackupComponents.GatherWriterMetadata"/> and wait for it 
      /// 		to complete prior to using <see cref="WriterMetadata"/>.
      /// 	</para>
      /// 	<para>
      /// 		Although <see cref="IVssBackupComponents.GatherWriterMetadata"/> must be called prior to either a restore or backup operation, 
      /// 		<see cref="WriterMetadata"/> is not typically used for restores.
      /// 	</para>
      /// 	<para>
      /// 		Component information retrieved (during backup operations) using <see cref="IVssExamineWriterMetadata.Components"/>, where the 
      /// 		<see cref="IVssExamineWriterMetadata"/> instance has been returned by <see cref="WriterMetadata"/>, comes from the Writer 
      /// 		Metadata Document of a live writer process.
      /// 	</para>
      /// 	<para>
      /// 		This is in contrast to the information returned by <see cref="WriterComponents"/> (during restore operations), which was 
      /// 		stored in the Backup Components Document by calls to <see cref="AddComponent"/>.
      /// 	</para>
      /// </remarks>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      IList<IVssExamineWriterMetadata> WriterMetadata { get; }

      /// <summary>
      ///     A read-only list containing the status of the writers.
      /// </summary>
      /// <value>
      ///		A read-only list containing <see cref="VssWriterStatusInfo"/> instances representing the returned status for each respective writer.
      ///		<note type="caution">This list must not be accessed after the <see cref="IVssBackupComponents"/> from which it 
      ///     was obtained has been disposed.</note>
      /// </value>
      /// <remarks>
      /// 	<para>
      /// 		A requester must call the asynchronous operation <see cref="IVssBackupComponents.GatherWriterStatus"/> and wait for it to 
      /// 		complete prior to using <see cref="IVssBackupComponents.WriterStatus"/>.
      /// 	</para>
      /// </remarks>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
      IList<VssWriterStatusInfo> WriterStatus { get; }

      /// <summary>
      ///     The ImportSnapshots method imports shadow copies transported from a different machine.
      /// </summary>
      /// <note>This method is supported only on Windows Server operating systems.</note>      
      /// <remarks>
      /// 	<para>Only one shadow copy can be imported at a time.</para>
      /// 	<para>The requester is responsible for serializing the import shadow copy operation.</para>
      ///		<para>For more information see the <see href="http://msdn.microsoft.com/en-us/library/aa382683(VS.85).aspx">MSDN documentation on IIVssBackupComponents::ImportSnapshots Method</see></para>
      ///		<para>Requires Windows Server 2008, Windows Server 2003 SP1, Windows Server 2003, Enterprise Edition, or Windows Server 2003, Datacenter Edition.</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void ImportSnapshots();

      /// <summary>
      ///     The <see cref="BeginImportSnapshots"/> method asynchronously imports shadow copies transported from a different machine.
      /// </summary>
      /// <note>This method is supported only on Windows Server operating systems.</note>      
      /// <remarks>
      /// 	<para>Only one shadow copy can be imported at a time.</para>
      /// 	<para>The requester is responsible for serializing the import shadow copy operation.</para>
      ///		<para>For more information see the <see href="http://msdn.microsoft.com/en-us/library/aa382683(VS.85).aspx">MSDN documentation on IIVssBackupComponents::ImportSnapshots Method</see></para>
      ///		<para>Requires Windows Server 2008, Windows Server 2003 SP1, Windows Server 2003, Enterprise Edition, or Windows Server 2003, Datacenter Edition.</para>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndImportSnapshots"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndImportSnapshots"/> must be called once for every call to <see cref="BeginImportSnapshots"/>. You can do this either by using the same code that called <b>BeginImportSnapshots</b> or
      /// in a callback passed to <b>BeginImportSnapshots</b>.
      /// </para>
      /// </remarks>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginImportSnapshots(AsyncCallback userCallback, object state);



      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndImportSnapshots</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginImportSnapshots"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void EndImportSnapshots(IAsyncResult asyncResult);


      /// <summary>
      /// 	The <see cref="InitializeForBackup"/> method initializes the backup components metadata in preparation for backup.
      /// </summary>
      /// <param name="xml">
      /// 	<para>
      /// 		During imports of transported shadow copies, this parameter must be the original document generated when creating the saved 
      /// 		shadow copy and saved using <see cref="SaveAsXml"/>. 
      /// 	</para>
      /// 	<para>
      /// 		This parameter may be <see langword="null"/>
      /// 	</para>
      /// </param>
      /// <remarks>
      /// 	The XML document supplied to this method initializes the <see cref="IVssBackupComponents"/> object with metadata previously stored by 
      /// 	a call to <see cref="SaveAsXml"/>. Users should not tamper with this metadata document.
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void InitializeForBackup(string xml);

      /// <summary>
      ///		The <see cref="InitializeForRestore"/> method initializes the IIVssBackupComponents interface in preparation for a restore operation.
      /// </summary>
      /// <param name="xml">
      ///		XML string containing the Backup Components Document generated by a backup operation and saved by 
      ///		<see cref="SaveAsXml"/>.
      /// </param>
      /// <remarks>
      /// 	The XML document supplied to this method initializes the <see cref="IVssBackupComponents"/> object with metadata previously stored by a call to 
      /// 	<see cref="SaveAsXml"/>. Users should not tamper with this metadata document.
      /// </remarks>
      /// <exception cref="ArgumentNullException"><paramref name="xml" /> is <see langword="null"/></exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssInvalidXmlDocumentException">The load operation of the specified XML document failed.</exception>
      void InitializeForRestore(string xml);

      /// <summary>
      /// 	The <c>IsVolumeSupported</c> method determines whether the specified provider supports shadow copies on the specified volume.
      /// </summary>
      /// <param name="providerId">
      /// 	Provider identifier. If the value is <see cref="Guid.Empty"/>, <see cref="IsVolumeSupported(string, System.Guid)"/> checks whether any provider 
      /// 	supports the volume.
      /// </param>
      /// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
      /// <list type="bullet">
      /// <item><description>The path of a volume mount point with a backslash (\)</description></item>
      /// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      /// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      /// </list></param>
      /// <returns><see langword="true"/> if shadow copies are supported on the specified volume. If the value is <see langword="false"/>, shadow 
      /// copies are not supported on the specified volume.</returns>
      /// <remarks>
      /// 	<para>
      /// 		<see cref="IsVolumeSupported(string, System.Guid)"/> will return <see langword="true"/> if it is possible to create shadow copies on the given volume, 
      /// 		even if the current configuration does not allow the creation of shadow copies on that volume at the present time.
      /// 	</para>
      /// 	<para>
      /// 		For example, if the maximum number of shadow copies has been reached on a given volume (and therefore no more shadow copies 
      /// 		can be created on that volume), the method will still indicate that the volume can be shadow copied.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException"><paramref name="volumeName" /> is <see langword="null"/></exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found or was not available.</exception>
      bool IsVolumeSupported(string volumeName, Guid providerId);

      /// <summary>
      /// 	The <c>IsVolumeSupported</c> method determines whether any provider supports shadow copies on the specified volume.
      /// </summary>
      /// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
      /// <list type="bullet">
      /// <item><description>The path of a volume mount point with a backslash (\)</description></item>
      /// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      /// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      /// </list></param>
      /// <returns><see langword="true"/> if shadow copies are supported on the specified volume. If the value is <see langword="false"/>, shadow 
      /// copies are not supported on the specified volume.</returns>
      /// <remarks>
      /// 	<para>
      /// 		<see cref="IsVolumeSupported(string, System.Guid)"/> will return <see langword="true"/> if it is possible to create shadow copies on the given volume, 
      /// 		even if the current configuration does not allow the creation of shadow copies on that volume at the present time.
      /// 	</para>
      /// 	<para>
      /// 		For example, if the maximum number of shadow copies has been reached on a given volume (and therefore no more shadow copies 
      /// 		can be created on that volume), the method will still indicate that the volume can be shadow copied.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException"><paramref name="volumeName" /> is <see langword="null"/></exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found or was not available.</exception>
      bool IsVolumeSupported(string volumeName);

      /// <summary>
      ///	The <see cref="PostRestore"/> method will cause VSS to generate a <c>PostRestore</c> event, signaling writers that the current 
      ///	restore operation has finished.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found or was not available.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      void PostRestore();

      /// <summary>
      ///	The <see cref="BeginPostRestore"/> method will asynchronously cause VSS to generate a <c>PostRestore</c> event, signaling writers that the current 
      ///	restore operation has finished.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndPostRestore"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndPostRestore"/> must be called once for every call to <see cref="BeginPostRestore"/>. You can do this either by using the same code that called <b>BeginPostRestore</b> or
      /// in a callback passed to <b>BeginPostRestore</b>.
      /// </para>
      /// </remarks>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginPostRestore(AsyncCallback userCallback, object state);


      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndGatherWriterStatus</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginPostRestore"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified volume was not found or was not available.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      void EndPostRestore(IAsyncResult asyncResult);

      /// <summary>
      /// 	The <see cref="PrepareForBackup"/> method will cause VSS to generate a PrepareForBackup event, signaling writers to prepare for an upcoming 
      /// 	backup operation. This makes a requester's Backup Components Document available to writers.
      /// </summary>
      /// <remarks>
      /// 	<para>
      /// 		<see cref="PrepareForBackup"/> generates a <c>PrepareForBackup</c> event, which is handled by each instance of each writer 
      /// 		through the CVssWriter::OnPrepareBackup method.
      /// 	</para>
      /// 	<para>
      /// 		Before PrepareForBackup can be called, <see cref="SetBackupState"/> must be called.
      /// 	</para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void PrepareForBackup();
      
      /// <summary>
      /// 	The <see cref="BeginPrepareForBackup"/> method will asynchronously cause VSS to generate a PrepareForBackup event, signaling writers to prepare for an upcoming 
      /// 	backup operation. This makes a requester's Backup Components Document available to writers.
      /// </summary>
      /// <remarks>
      /// 	<para>
      /// 		<see cref="BeginPrepareForBackup"/> generates a <c>PrepareForBackup</c> event, which is handled by each instance of each writer 
      /// 		through the CVssWriter::OnPrepareBackup method.
      /// 	</para>
      /// 	<para>
      /// 		Before PrepareForBackup can be called, <see cref="SetBackupState"/> must be called.
      /// 	</para>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndPrepareForBackup"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndPrepareForBackup"/> must be called once for every call to <see cref="BeginPrepareForBackup"/>. You can do this either by using the same code that called <b>BeginPrepareForBackup</b> or
      /// in a callback passed to <b>BeginPrepareForBackup</b>.
      /// </para>
      /// </remarks>
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginPrepareForBackup(AsyncCallback userCallback, object state);


      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndGatherWriterStatus</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginPrepareForBackup"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void EndPrepareForBackup(IAsyncResult asyncResult);

      /// <summary>
      /// The <see cref="PreRestore"/> method will cause VSS to generate a <c>PreRestore</c> event, signaling writers to prepare for a 
      /// coming restore operation.
      /// </summary>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void PreRestore();

      /// <summary>
      /// The <see cref="BeginPreRestore"/> method will asynchronously cause VSS to generate a <c>PreRestore</c> event, signaling writers to prepare for a 
      /// coming restore operation.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndPreRestore"/> method to release operating system resources used for this asynchronous operation.
      /// <see cref="EndPreRestore"/> must be called once for every call to <see cref="BeginPreRestore"/>. You can do this either by using the same code that called <b>BeginPreRestore</b> or
      /// in a callback passed to <b>BeginPreRestore</b>.
      /// </para>
      /// </remarks>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.</returns>
      IVssAsyncResult BeginPreRestore(AsyncCallback userCallback, object state);


      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndGatherWriterStatus</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginPreRestore"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void EndPreRestore(IAsyncResult asyncResult);

      /// <summary>
      /// 	The <see cref="QuerySnapshots"/> method queries the completed shadow copies in the system that reside in the current context. 
      /// 	The method can be called only during backup operations.
      /// </summary>
      /// <returns>A list of <see cref="VssSnapshotProperties"/> objects representing the requested information.</returns>
      /// <remarks>
      /// 	 <para>
      /// 		Because <see cref="QuerySnapshots"/> returns only information on completed shadow copies, the only shadow copy state it can disclose 
      /// 		is <see cref="VssSnapshotState.Created"/>.
      /// 	 </para>
      /// 	 <para>
      /// 		The method may be called only during backup operations and must be preceded by calls to <see cref="InitializeForBackup"/> and 
      /// 		<see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetContext"/>.
      /// 	 </para>
      /// 	 <para>
      /// 		The method will return only information 
      /// 		about shadow copies with the current context (set by <see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetContext"/>). For instance, if the 
      /// 		<see cref="VssSnapshotContext"/> context is set to <see cref="VssSnapshotContext.Backup"/>, <see cref="QuerySnapshots"/> will not 
      /// 		return information on a shadow copy created with a context of <see cref="VssSnapshotContext.FileShareBackup" />.
      /// 	 </para>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller is not an administrator or a backup operator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The queried object is not found.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>		
      IEnumerable<VssSnapshotProperties> QuerySnapshots();

      /// <summary>
      /// 	The <see cref="QueryProviders"/> method queries providers on the system. 
      /// 	The method can be called only during backup operations.
      /// </summary>
      /// <returns>A list of <see cref="VssProviderProperties"/> objects representing the requested information.</returns>
      /// <remarks>
      /// 	 <para>
      /// 		The method may be called only during backup operations and must be preceded by calls to <see cref="InitializeForBackup"/> and 
      /// 		<see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetContext"/>.
      /// 	 </para>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller is not an administrator or a backup operator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The queried object is not found.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      IEnumerable<VssProviderProperties> QueryProviders();

      /// <summary>
      /// The <see cref="BeginQueryRevertStatus"/> method begins an asynchronous operation to determine the status of the revert operation. The 
      /// returned <see cref="IVssAsyncResult"/> can be used to determine the outcome of the operation.
      /// </summary>      
      /// <param name="userCallback">An optional asynchronous callback, to be called when the operation is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>
      /// An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.
      /// </returns>
      /// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
      /// <list type="bullet">
      /// <item><description>The path of a volume mount point with a backslash (\)</description></item>
      /// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
      /// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
      /// </list></param>
      /// <returns>An <see cref="IVssAsyncResult"/> instance that can be used to determine the status of the revert operation.</returns>
      /// <remarks>
      /// 	The revert operation will continue even if the computer is rebooted, and cannot be canceled or undone, except by restoring a 
      /// 	backup created using another method.
      /// <note><b>Windows XP, Windows Server 2003 and Windows Vista:</b> This method requires Windows Server 2008 or Windows Server 2003 SP1</note>
      /// </remarks>
      IVssAsyncResult BeginQueryRevertStatus(string volumeName, AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndQueryRevertStatus</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginQueryRevertStatus"/>.
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The calling process has insufficient privileges.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified parameter is not a valid volume.</exception>
      /// <exception cref="VssVolumeNotSupportedException">Revert is not supported on this volume.</exception>
      /// <exception cref="NotImplementedException">The provider for the volume does not support revert operations.</exception>
      /// <exception cref="NotSupportedException">This operation is not supported on the current operating system.</exception>
      void EndQueryRevertStatus(IAsyncResult asyncResult);

      /// <summary>
      /// 	The <see cref="RevertToSnapshot"/> method reverts a volume to a previous shadow copy. Only shadow copies created with persistent 
      /// 		contexts (<see cref="VssSnapshotContext.AppRollback" />, <see cref="VssSnapshotContext.ClientAccessible" />, 
      /// 		<see cref="VssSnapshotContext.ClientAccessibleWriters" /> or <see cref="VssSnapshotContext.NasRollback" />)
      /// 		are supported.
      /// </summary>
      /// <param name="snapshotId">The identifier of the shadow copy to revert</param>
      /// <param name="forceDismount">If this parameter is <see langword="true"/>, the volume will be dismounted and reverted even if the volume is in use.</param>
      /// <remarks>This operation cannot be canceled, or undone once completed. If the computer is rebooted during the revert operation, the revert process will continue when the system is restarted.
      /// <note><b>Windows XP, Windows Server 2003 and Windows Vista:</b> This method requires Windows Server 2008 or Windows Server 2003 SP1</note>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The calling process has insufficient privileges.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The <paramref name="snapshotId"/> parameter is not a valid shadow copy.</exception>
      /// <exception cref="VssProviderNotRegisteredException">The provider was not found.</exception>
      /// <exception cref="VssRevertInProgressException">The volume already has a revert in process.</exception>
      /// <exception cref="VssUnsupportedContextException">Revert is only supported for persistent shadow copies.</exception>
      /// <exception cref="VssVolumeInUseException">The <paramref name="forceDismount"/> parameter was <see langword="false"/>, and the volume could not be locked.</exception>
      /// <exception cref="VssVolumeNotSupportedException">Revert is not supported on this volume.</exception>
      /// <exception cref="NotImplementedException">The provider for the volume does not support revert operations.</exception>
      /// <exception cref="NotSupportedException">This operation is not supported on the current operating system.</exception>
      void RevertToSnapshot(Guid snapshotId, bool forceDismount);

      /// <summary>
      /// 	The SaveAsXml method saves the Backup Components Document containing a requester's state information to a specified string. 
      /// 	This XML document, which contains the Backup Components Document, should always be securely saved as part of a backup operation.
      /// </summary>
      /// <returns>The Backup Components Document containing a requester's state information.</returns>
      /// <remarks>
      /// 	<para>For a typical backup operation, SaveAsXml should not be called until after both writers and the requester are finished modifying the Backup Components Document.</para>
      /// 	<para>Writers can continue to modify the Backup Components Document until their successful return from handling the PostSnapshot event (CVssWriter::OnPostSnapshot), or equivalently upon the completion of <see cref="DoSnapshotSet"/>.</para>
      /// 	<para>Requesters will need to continue to modify the Backup Components Document as the backup progresses. In particular, a requester will store a component-by-component record of the success or failure of the backup through calls to the <see cref="SetBackupSucceeded"/> method.</para>
      /// 	<para>Once the requester has finished modifying the Backup Components Document, the requester should use <see cref="SaveAsXml"/> to save a copy of the document to the backup media.</para>
      /// 	<para>A Backup Components Document can be saved at earlier points in the life cycle of a backup operation, for instance, to support the generation of transportable shadow copies to be handled on remote machines.</para>
      /// 	<para>However, <see cref="SaveAsXml"/> should never be called prior to <see cref="PrepareForBackup"/>, because the Backup Components Document will not have been filled by the requester and the writers.</para>
      /// </remarks>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      string SaveAsXml();

      /// <summary>
      ///		The <b>SetAdditionalRestores</b> method is used by a requester during incremental or differential restore operations to indicate 
      ///     to writers that a given component will require additional restore operations to completely retrieve it.
      /// </summary>
      /// <param name="writerId">Writer identifier.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component to be added. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="additionalResources">
      /// 	<para>
      /// 		If the value of this parameter is <see langword="true"/>, additional restores of the component will follow this restore. If the 
      /// 		value is <see langword="false"/>, additional restores of the component will not follow this restore.
      /// 	</para>
      /// </param>
      /// <remarks>
      /// 	<para>
      /// 		The information provided by the <see cref="SetAdditionalRestores"/> method is typically used by writers that support an explicit 
      /// 		recovery mechanism as part of their PostRestore event handler (CVssWriter::OnPostRestore)�for instance, the Exchange Server, and 
      /// 		database applications such as SQL Server. For these applications, it is often not possible to perform additional differential, 
      /// 		incremental, or log restores after such a recovery is performed.
      /// 	</para>
      /// 	<para>
      /// 		Therefore, if <see cref="SetAdditionalRestores"/> for a component is set to <see langword="true"/>, this means that such a writer 
      /// 		should not execute its explicit recovery mechanism and should expect that additional differential, incremental, or log restores 
      /// 		will be done.
      /// 	</para>
      /// 	<para>
      /// 		When <see cref="SetAdditionalRestores"/> on a component is set to <see langword="false"/>, then after the component is restored, 
      /// 		the application can complete its recovery operation and be brought back online.
      /// 	</para>
      /// 	<para>
      /// 		This method must be called before <see cref="PreRestore"/>.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetAdditionalRestores(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, bool additionalResources);

      /// <summary>
      /// 	Sets a string of private, or writer-dependent, backup parameters for a component.
      /// </summary>
      /// <param name="writerId">Writer identifier.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component to be added. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="backupOptions">
      /// 	String containing the backup parameters to be set.
      /// </param>
      /// <remarks>
      /// 	<para>
      /// 		The exact syntax and content of the backup options set by the wszBackupOptions parameter of the <see cref="SetBackupOptions"/> method 
      /// 		will depend on the specific writer being contacted.
      /// 	</para>
      /// 	<para>
      /// 		This method must be called before <see cref="PrepareForBackup"/>.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetBackupOptions(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string backupOptions);

      /// <summary>
      ///     Defines an overall configuration for a backup operation.
      /// </summary>
      /// <param name="selectComponents">
      /// 	<para>
      /// 		Indicates whether a backup or restore operation will be in component mode.
      /// 	</para>
      /// 	<para>
      /// 		Operation in component mode supports selectively backing up designated individual components (which can allow their exclusion), 
      /// 		or only supports backing up all files and components on a volume.
      /// 	</para>
      /// 	<para>
      /// 		The Boolean is <see langword="true"/> if the operation will be conducted in component mode and <see langword="false" /> if not.
      /// 	</para>
      /// </param>
      /// <param name="backupBootableSystemState">
      /// 	<para>
      /// 		Indicates whether a bootable system state backup is being performed.
      /// 	</para>
      /// </param>
      /// <param name="backupType">
      /// 	<para>
      /// 		A <see cref="VssBackupType"/> enumeration value indicating the type of backup to be performed.
      /// 	</para>
      /// </param>
      /// <param name="partialFileSupport">
      /// 	<para>
      /// 		If the value of this parameter is <see langword="true"/>, partial file support is enabled. 
      /// 		The default value for this argument is <see langword="false"/>.
      /// 	</para>
      /// </param>
      /// <remarks>
      /// 	Applications must call <see cref="SetBackupState"/> prior to calling <see cref="PrepareForBackup"/>.
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetBackupState(bool selectComponents, bool backupBootableSystemState, VssBackupType backupType, bool partialFileSupport);

      /// <summary>
      /// 	Indicates whether the backup of the specified component of a specific writer was successful.
      /// </summary>
      /// <param name="instanceId">Globally unique identifier of the writer instance.</param>
      /// <param name="writerId">Globally unique identifier of the writer class.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component to be added. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="succeeded">The value of this parameter is <see langword="true"/> if the component was successfully backed up, and <see langword="false"/> if it was not.</param>
      /// <remarks>
      /// 	<para>
      /// 		When working in component mode (when <see cref="SetBackupState"/> is called with its select components argument set to <see langword="true"/>), 
      /// 		writers the state of each components backup using <see cref="IVssComponent.BackupSucceeded"/>.
      /// 	</para>
      /// 	<para>
      /// 		Therefore, a well-behaved backup application (requester) must call <see cref="SetBackupSucceeded"/> after each component has been 
      /// 		processed and prior to calling <see cref="BackupComplete"/>.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetBackupSucceeded(Guid instanceId, Guid writerId, VssComponentType componentType, string logicalPath, string componentName, bool succeeded);

      /// <overloads>
      ///   Sets the context for subsequent  shadow copy-related operations.
      /// </overloads>
      /// <summary>
      /// 	Sets the context for subsequent shadow copy-related operations. 
      /// </summary>
      /// <param name="context">
      /// 	The context to be set. The context must be one of the supported values of <see cref="VssSnapshotContext"/> or a supported bit 
      /// 	mask (or bitwise OR) of <see cref="VssVolumeSnapshotAttributes" /> with a valid <see cref="VssSnapshotContext" />. 
      /// </param>
      /// <remarks>
      /// 	<para>
      /// 		The default context for VSS shadow copies is <see cref="VssSnapshotContext.Backup"/>.
      /// 	</para>
      /// 	<para>
      /// 		<b>Windows XP:</b> The only supported context is the default context, <see cref="VssSnapshotContext.Backup"/>. Therefore, calling 
      /// 		this method under Windows XP throws a <see cref="NotImplementedException"/>. 
      /// 	</para>
      /// 	<para>
      /// 		This method be called only once, and it must be called prior to calling most VSS functions.
      /// 	</para>
      /// 	<para>
      /// 		For details on how the context set by this method affects how a shadow copy is created and managed, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa381653(VS.85).aspx">Implementation Details for Creating Shadow Copies</see>.
      /// 	</para>
      /// 	<para>
      /// 		For a complete discussion of the permitted shadow copy contexts, see <see cref="VssSnapshotContext"/> and <see cref="VssVolumeSnapshotAttributes" />. 
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void SetContext(VssVolumeSnapshotAttributes context);

      /// <summary>
      /// 	Sets the context for subsequent shadow copy-related operations. 
      /// </summary>
      /// <param name="context">
      /// 	The context to be set. The context must be one of the supported values of <see cref="VssSnapshotContext"/> or a supported bit 
      /// 	mask (or bitwise OR) of <see cref="VssVolumeSnapshotAttributes" /> with a valid <see cref="VssSnapshotContext" />. 
      /// </param>
      /// <remarks>
      /// 	<para>
      /// 		The default context for VSS shadow copies is <see cref="VssSnapshotContext.Backup"/>.
      /// 	</para>
      /// 	<para>
      /// 		<b>Windows XP:</b> The only supported context is the default context, <see cref="VssSnapshotContext.Backup"/>. Therefore, calling 
      /// 		this method under Windows XP throws a <see cref="NotImplementedException"/>. 
      /// 	</para>
      /// 	<para>
      /// 		<see cref="SetContext(VssSnapshotContext)"/> can be called only once, and it must be called prior to calling most VSS functions.
      /// 	</para>
      /// 	<para>
      /// 		For details on how the context set by this method affects how a shadow copy is created and managed, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa381653(VS.85).aspx">Implementation Details for Creating Shadow Copies</see>.
      /// 	</para>
      /// 	<para>
      /// 		For a complete discussion of the permitted shadow copy contexts, see <see cref="VssSnapshotContext"/> and <see cref="VssVolumeSnapshotAttributes" />. 
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      void SetContext(VssSnapshotContext context);

      /// <summary>
      /// 	Indicates whether some, all, or no files were successfully restored.
      /// </summary>
      /// <param name="writerId">Globally unique identifier of the writer class.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="status">
      /// 	If all of the files were restored, the value of this parameter is <see cref="VssFileRestoreStatus.All" />. 
      /// 	If some of the files were restored, the value of this parameter is <see cref="VssFileRestoreStatus.Failed" />. If none of the files 
      /// 	were restored, the value of this parameter is <see cref="VssFileRestoreStatus.None" />.
      /// </param>
      /// <remarks>This method should be called between calls to <see cref="PreRestore"/> and <see cref="PostRestore"/>.</remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetFileRestoreStatus(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, VssFileRestoreStatus status);

      /// <summary>
      /// 	<para>
      /// 		Sets the backup stamp of an earlier backup operation, upon which a differential or 
      /// 		incremental backup operation will be based.
      /// 	</para>
      /// 	<para>
      /// 		The method can be called only during a backup operation.
      /// 	</para>
      /// </summary>
      /// <param name="writerId">Writer identifier.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="previousBackupStamp">The backup stamp to be set.</param>
      /// <remarks>
      /// <para>This method should be called before <see cref="PrepareForBackup"/>.</para>
      /// <para>Only requesters can call this method.</para>
      /// <para>The backup stamp set by <see cref="SetPreviousBackupStamp"/> applies to all files in the component and any nonselectable Subcomponents it has.</para>
      /// <para>Requesters merely store the backup stamps in the Backup Components Document. They cannot make direct use of the backup stamps, do not know their format, and do not know how to generate them.</para>
      /// <para>Therefore, the value set with <see cref="SetPreviousBackupStamp"/> should either be retrieved from the stored Backup Components Document of an earlier backup operation (using <see cref="IVssComponent.BackupStamp"/> for the correct component), or from information stored by the requester into its own internal records.</para>
      /// <para>A writer will then obtain this value (using <c>IVssComponent::GetPreviousBackupStamp</c>) and using it will be able to mark the appropriate files for participation in an incremental or differential backup.</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetPreviousBackupStamp(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string previousBackupStamp);

      /// <summary>
      ///     This method is used when a partial file operation requires a ranges file, and that file has been restored to a location other than its original one.
      /// </summary>		
      /// <param name="writerId">Globally unique identifier (GUID) of the writer class containing the files involved in the partial file operation</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component containing the files that are participating in the partial file operation. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component containing the files that are participating in the partial file operation.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="partialFileIndex">
      /// 	Index number of the partial file. The value of this parameter is an integer between <c>0</c> and <c>n-1</c>, 
      /// 	where <c>n</c> is the total number of partial files associated with a given component. The value of <c>n</c> is the number
      ///     of items in <see cref="IVssComponent.PartialFiles"/>.
      /// </param>
      /// <param name="rangesFile">
      /// 	The fully qualified path of a ranges file.
      /// </param>
      /// <remarks>
      /// 	Calling <see cref="SetRangesFilePath"/> is not necessary if ranges files are restored in place.
      /// <note><b>Windows XP and Windows Vista:</b> This method requires Windows Server 2008 or Windows Server 2003</note>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <exception cref="NotSupportedException">The operation is not supported by the current operating system.</exception>
      void SetRangesFilePath(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, int partialFileIndex, string rangesFile);

      /// <summary>
      ///		Sets a string of private, or writer-dependent, restore parameters for a writer component.
      /// </summary>
      /// <param name="writerId">Writer identifier.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="restoreOptions">The private string of restore parameters. For more information see <see href="http://msdn.microsoft.com/en-us/library/aa384610(VS.85).aspx">Setting VSS Restore Options</see>.</param>
      /// <remarks>
      /// 	<para>
      /// 		This method must be called before <see cref="PreRestore"/>.
      /// 	</para>
      /// 	<para>
      /// 		The exact syntax and content of the restore options set by the <paramref name="restoreOptions" /> parameter of the 
      /// 		<see cref="SetRestoreOptions"/> method will depend on the specific writer being contacted.
      /// 	</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetRestoreOptions(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string restoreOptions);

      /// <summary>
      /// 	Defines an overall configuration for a restore operation.
      /// </summary>
      /// <param name="restoreType">The type of restore to be performed.</param>
      /// <remarks>
      /// 	<para>Typically, most restore operations will not need to override the default restore type <see cref="VssRestoreType.Undefined" />.</para>
      /// 	<para>If applications need to call <see cref="SetRestoreState"/>, it should be called prior to calling <see cref="PreRestore"/>.</para>
      /// <note><b>Windows XP:</b> This method requires Windows Vista, Windows Server 2008 or Windows Server 2003</note>
      /// </remarks>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetRestoreState(VssRestoreType restoreType);

      /// <summary>
      ///     Indicates whether the specified selectable component is selected for restoration.
      /// </summary>
      /// <overloads>
      ///     Indicates whether the specified selectable component is selected for restoration. 
      /// </overloads>
      /// <param name="writerId">Writer identifier.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="selectedForRestore">
      ///		If the value of this parameter is <see langword="true"/>, the selected component has been selected for 
      /// 	restoration. If the value is <see langword="false"/>, the selected component has not been selected for restoration.
      /// </param>
      /// <remarks>
      ///		<para>SetSelectedForRestore has meaning only for restores taking place in component mode.</para>
      /// 	<para><see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetSelectedForRestore"/> can only be called for components that were explicitly added to the 
      /// 	backup document at backup time using <see cref="AddComponent"/>. Restoring a component that was implicitly 
      /// 	selected for backup as part of a component set must be done by calling <see cref="O:Alphaleonis.Win32.Vss.IVssBackupComponents.SetSelectedForRestore"/> on the closest 
      /// 	ancestor component that was added to the document. If only this component's data is to be restored, 
      /// 	that should be accomplished through <see cref="AddRestoreSubcomponent"/>; this can only be done if the component 
      /// 	is selectable for restore.</para>
      /// 	<para>This method must be called before <see cref="PreRestore"/>.</para>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      /// <overloads>indicates whether the specified selectable component is selected for restoration. This method has two overloads.</overloads>
      void SetSelectedForRestore(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, bool selectedForRestore);

      /// <summary>
      /// 	Creates a new, empty shadow copy set.
      /// </summary>
      /// <returns>Shadow copy set identifier.</returns>
      /// <remarks>This method must be called before <see cref="PrepareForBackup"/> during backup operations.</remarks>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssSnapshotSetInProgressException">The creation of a shadow copy is in progress, and only one shadow copy creation operation can be in progress at one time. Either wait to try again or return with a failure error code.</exception>
      Guid StartSnapshotSet();

      #endregion

      #region IVssBackupComponentsEx methods

      /// <summary>
      ///     Indicates whether the specified selectable component is selected for restoration to a specified writer instance.
      /// </summary>
      /// <param name="writerId">Globally unique identifier (GUID) of the writer class.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="selectedForRestore">
      ///		If the value of this parameter is <see langword="true"/>, the selected component has been selected for 
      /// 	restoration. If the value is <see langword="false"/>, the selected component has not been selected for restoration.
      /// </param>
      /// <param name="instanceId">
      ///     <para>GUID of the writer instance.</para>
      /// </param>
      /// <remarks>
      /// <para>        
      ///     <c>SetSelectedForRestore</c>, which moves a component to a different writer instance, can be called only for a 
      ///     writer that supports running multiple writer instances with the same class ID and supports a requester moving a 
      ///     component to a different writer instance at restore time. To determine whether a writer provides this support, call 
      ///     the <see cref="IVssExamineWriterMetadata.BackupSchema"/> method.
      /// </para>
      /// <para>
      ///     <c>SetSelectedForRestore</c> has meaning only for restores taking place in component mode.
      /// </para>
      /// <para>
      ///     <c>SetSelectedForRestore</c> can be called only for components that were explicitly added to the backup document at backup 
      ///     time using AddComponent. Restoring a component that was implicitly selected for backup as part of a component set must be 
      ///     done by calling <c>SetSelectedForRestore</c> on the closest ancestor component that was added to the document. If only 
      ///     this component's data is to be restored, that should be accomplished through the <see cref="IVssBackupComponents.AddRestoreSubcomponent"/> method; 
      ///     this can be done only if the component is selectable for restore (see 
      ///     <see href="http://msdn.microsoft.com/en-us/library/aa384988(VS.85).aspx">Working with Selectability and Logical Paths</see>).
      /// </para>
      /// <para>
      ///     This method must be called before the <see cref="IVssBackupComponents.PreRestore"/> method.
      /// </para>
      /// <para>
      ///     The distinction between the <paramref name="instanceId"/> and <paramref name="writerId"/> parameters is necessary because it is 
      ///     possible that multiple instances of the same writer are running on the computer.
      /// </para>
      /// <para>
      ///     If the value of the <paramref name="instanceId"/> parameter is <see cref="Guid.Empty"/>, this is equivalent to calling the 
      ///     <see cref="IVssBackupComponents.SetSelectedForRestore(Guid,VssComponentType,string,string,bool)"/>.
      /// </para>
      /// <para>
      ///     The <paramref name="instanceId"/> parameter is used to specify that the component is to be restored to a different writer 
      ///     instance. If the value of the <paramref name="instanceId"/> parameter is not <see cref="Guid.Empty"/>, it must match the 
      ///     instance ID of a writer instance with the same writer class ID specified in in the <paramref name="writerId"/> parameter.
      /// </para>
      /// <para>
      ///     A writer's class identifier, instance identifier, and instance name can be found 
      ///     in the properties of <see cref="IVssExamineWriterMetadata"/>. 
      /// </para>
      /// <note>
      ///     <b>Windows XP and Windows 2003:</b> This method is not supported until Windows 2003 SP1.
      /// </note>
      /// </remarks>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
      void SetSelectedForRestore(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, bool selectedForRestore, Guid instanceId);

      #endregion

      #region IVssBackupComponentsEx2 methods

      /// <summary>
      /// Breaks a shadow copy set according to requester-specified options.
      /// </summary>
      /// <param name="snapshotSetId">A shadow copy set identifier.</param>
      /// <param name="breakFlags">A bitmask of <see cref="VssHardwareOptions"/> flags that specify how the shadow copy set is broken.</param>
      /// <remarks>
      ///     <para>
      ///         This method is similar to <see cref="BreakSnapshotSet(System.Guid)"/>, except that is has an extra parameter to specify
      ///         how the shadow copy set is broken.
      ///     </para>
      ///     <para>
      ///         Like <see cref="BreakSnapshotSet(System.Guid)"/>, this method can be used only for shadow copies that were created by 
      ///         a hardware shadow copy provider.
      ///     </para>
      ///     <para>
      ///         After this method returns, the shadow copy volume is still a volume, but it is no longer a shadow copy. 
      ///         For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381505(VS.85).aspx">Breaking Shadow Copies</see>.
      ///     </para>
      /// </remarks>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
      void BreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags);


      /// <summary>
      /// Begins an asynchronous operation to break a shadow copy set according to requester-specified options.
      /// </summary>
      /// <param name="snapshotSetId">A shadow copy set identifier.</param>
      /// <param name="breakFlags">A bitmask of <see cref="VssHardwareOptions"/> flags that specify how the shadow copy set is broken.</param>
      /// <remarks>
      ///     <para>
      ///         This method is similar to <see cref="BreakSnapshotSet(System.Guid)"/>, except that is has an extra parameter to specify
      ///         how the shadow copy set is broken.
      ///     </para>
      ///     <para>
      ///         Like <see cref="BreakSnapshotSet(System.Guid)"/>, this method can be used only for shadow copies that were created by 
      ///         a hardware shadow copy provider.
      ///     </para>
      ///     <para>
      ///         After this method returns, the shadow copy volume is still a volume, but it is no longer a shadow copy. 
      ///         For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381505(VS.85).aspx">Breaking Shadow Copies</see>.
      ///     </para>
      /// </remarks>
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>
      /// An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.
      /// </returns>
      IVssAsyncResult BeginBreakSnapshotSet(Guid snapshotSetId, VssHardwareOptions breakFlags, AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for an asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndBreakSnapshotSet</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginBreakSnapshotSet"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>      
      void EndBreakSnapshotSet(IAsyncResult asyncResult);

      /// <summary>
      /// Marks the restore of a component as authoritative for a replicated data store.
      /// </summary>
      /// <param name="writerId">The globally unique identifier (GUID) of the writer class.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component to be added. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="isAuthorative"><see langword="true"/> to indicate that the restore of the component is authoritative; otherwise, <see langword="false"/>.</param>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">This method was not called during a restore operation.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified component was not found.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP and Windows 2003:</b> This method requires Windows Vista or Windows Server 2008.
      ///         </note>
      ///     </para>
      /// </remarks>
      void SetAuthoritativeRestore(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, bool isAuthorative);

      /// <summary>
      /// Assigns a new logical name to a component that is being restored.
      /// </summary>
      /// <param name="writerId">The globally unique identifier (GUID) of the writer class.</param>
      /// <param name="componentType">The type of the component.</param>
      /// <param name="logicalPath">
      ///     <para>
      ///         A string containing the logical path of the component. For more information, see 
      ///         <see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      ///     </para>
      ///     <para>
      ///         The value of the string containing the logical path used here should be the same as the string that was used when 
      ///         the component was added.
      ///     </para>
      ///     <para>
      ///         The logical path can be <see langword="null"/>.
      ///     </para>
      ///     <para>
      ///         There are no restrictions on the characters that can appear in a logical path.
      ///     </para>
      /// </param>
      /// <param name="componentName">
      ///     <para>The name of the component.</para>
      ///     <para>
      ///         The string cannot be <see langword="null"/> and should contain the same component name as was the component name 
      ///         that was used when the component was added to the backup set using the <see cref="IVssBackupComponents.AddComponent"/> method.
      ///     </para>
      ///  </param>
      /// <param name="restoreName">String containing the restore name to be set for the component.</param>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">This method was not called during a restore operation.</exception>		
      /// <exception cref="VssObjectNotFoundException">The specified component was not found.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP and Windows 2003:</b> This method requires Windows Vista or Windows Server 2008.
      ///         </note>
      ///     </para>
      /// </remarks>
      void SetRestoreName(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, string restoreName);

      /// <summary>
      /// Sets the roll-forward operation type for a component and specifies the restore point for a partial roll-forward operation.
      /// </summary>
      /// <param name="writerId">The globally unique identifier (GUID) of the writer class.</param>
      /// <param name="componentType">Type of the component.</param>
      /// <param name="logicalPath">
      /// 	<para>
      /// 		The logical path of the component. For more information, see 
      /// 		<see href="http://msdn.microsoft.com/en-us/library/aa384316(VS.85).aspx">Logical Pathing of Components</see>.
      /// 	</para>
      /// 	<para>
      /// 		The value of the string containing the logical path used here should be the same as was used when the component was 
      /// 		added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// 	<para>
      /// 		The logical path can be <see langword="null"/>.
      /// 	</para>
      /// 	<para>
      /// 		There are no restrictions on the characters that can appear in a non-<c>null</c> logical path.
      /// 	</para>
      /// </param>
      /// <param name="componentName">
      /// 	<para>The name of the component.</para>
      /// 	<para>
      /// 		The value of the string should not be <see langword="null"/>, and should contain the same component as was used when the 
      /// 		component was added to the backup set using <see cref="AddComponent"/>.
      /// 	</para>
      /// </param>
      /// <param name="rollType">A <see cref="VssRollForwardType"/> enumeration value indicating the type of roll-forward operation to be performed.</param>
      /// <param name="rollForwardPoint">
      ///     <para>A null-terminated wide character string specifying the roll-forward restore point.</para>
      ///     <para>The format of this string is defined by the writer, and can be a timestamp, a log sequence number (LSN), or any marker defined by the writer.</para>
      /// </param>
      /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">This method was not called during a restore operation.</exception>		
      /// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP and Windows 2003:</b> This method requires Windows Vista or Windows Server 2008.
      ///         </note>
      ///     </para>
      /// </remarks>
      void SetRollForward(Guid writerId, VssComponentType componentType, string logicalPath, string componentName, VssRollForwardType rollType, string rollForwardPoint);

      /// <summary>
      /// Unexposes a shadow copy either by deleting the file share or by removing the drive letter or mount point.
      /// </summary>
      /// <param name="snapshotId">The shadow copy identifier. The value of this identifier should be the same as the value that was used when the shadow copy was exposed.</param>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist or is not exposed.</exception>
      /// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
      /// <exception cref="VssUnexpectedProviderErrorException">Unexpected provider error. The error code is logged in the error log.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP and Windows 2003:</b> This method requires Windows Vista or Windows Server 2008.
      ///         </note>
      ///     </para>
      /// </remarks>
      void UnexposeSnapshot(Guid snapshotId);

      #endregion

      #region IVssBackupComponentsEx3 methods

      /// <summary>
      ///     Specifies the volumes to be included in a LUN resynchronization operation. This method is supported only on Windows server operating systems.
      /// </summary>
      /// <param name="snapshotId">
      ///     The identifier of the shadow copy that was returned by the <see cref="AddToSnapshotSet(string, System.Guid)"/> method during backup. 
      ///     This parameter is required and cannot be <c>Guid.Empty</c>.
      /// </param>
      /// <param name="destinationVolume">
      ///     This parameter is optional and can be <see langword="null"/>. 
      ///     A value of <see langword="null"/> means that the contents of the shadow copy volume are to be copied back to the original volume. 
      ///     VSS identifies the original volume by the <c>VDS_LUN_INFO</c> information in the Backup Components Document.
      /// </param>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">Either there is no hardware provider that supports the operation, or the requester did not successfully add any volumes to the recovery set.</exception>
      /// <exception cref="VssLegacyProviderException">This version of the hardware provider does not support this operation.</exception>
      /// <exception cref="VssObjectNotFoundException">The <paramref name="snapshotId"/> parameter specifies a shadow copy that the hardware provider does not own.</exception>
      /// <exception cref="VssResyncInProgressException">Another LUN resynchronization operation is already in progress.</exception>
      /// <exception cref="VssSnapshotNotInSetException">The <paramref name="snapshotId"/> parameter specifies a shadow copy that does not exist in the Backup Components Document.</exception>
      /// <exception cref="VssVolumeNotSupportedException">LUN resynchronization is not supported on this volume, because it is a dynamic volume, 
      /// because the destination disk does not have a unique page 83 storage identifier, because the specified volume does not reside on a LUN managed 
      /// by a VSS hardware provider, or because the destination disk is a cluster quorum disk. </exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP, Windows 2003, Windows Vista, Windows 2008, Windows 7:</b> This method requires Windows Server 2008 R2.
      ///         </note>
      ///     </para>
      /// </remarks>
      void AddSnapshotToRecoverySet(Guid snapshotId, string destinationVolume);

      /// <summary>
      /// Gets the requester's session identifier.
      /// </summary>
      /// <remarks>
      /// <para>
      ///    The session identifier is an opaque value that uniquely identifies a backup or restore session. It is used to distinguish 
      ///    the current session among multiple parallel backup or restore sessions.
      /// </para>
      /// <para>
      ///    As a best practice, writers and requesters should include the session ID in all diagnostics messages used for event logging and tracing.
      /// </para>
      ///  <para>
      ///         <note>
      ///             <b>Windows XP, Windows 2003, Windows Vista, Windows 2008:</b> This method requires Windows 7 or Windows Server 2008 R2.
      ///         </note>
      ///     </para>
      /// </remarks>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
      /// <returns>The requester's session identifier.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      Guid GetSessionId();

      /// <summary>
      /// Initiates a LUN resynchronization operation. This method is supported only on Windows server operating systems.
      /// </summary>
      /// <param name="options"><see cref="VssRecoveryOptions"/> flags that specify how the resynchronization is to be performed.</param>
      /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="NotImplementedException">The provider for the volume does not support LUN resynchronization.</exception>
      /// <exception cref="VssBadStateException">Possible reasons for this return value include:
      ///		<list type="bullet">
      ///			<item><description>There is no hardware provider that supports the operation.</description></item>
      ///			<item><description>The requester did not successfully add any volumes to the recovery set.</description></item>
      ///			<item><description>The method was called in WinPE or in Safe mode.</description></item>
      ///			<item><description>he caller did not call the <see cref="InitializeForRestore"/> method before calling this method.</description></item>
      ///		</list>
      /// </exception>
      /// <exception cref="VssLegacyProviderException">This version of the hardware provider does not support this operation.</exception>
      /// <exception cref="VssProviderVetoException">An unexpected provider error occurred. If this error code is returned, the error must be described in an entry in the application event log, giving the user information on how to resolve the problem.</exception>
      /// <exception cref="VssUnselectedVolumeException">The resynchronization destination contained a volume that was not explicitly included.</exception>
      /// <exception cref="VssCannotRevertDiskIdException">The MBR signature or GPT ID for one or more disks could not be set to the intended value. Check the Application event log for more information.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP, Windows 2003, Windows Vista, Windows 2008, Windows 7:</b> This method requires Windows Server 2008 R2.
      ///         </note>
      ///     </para>
      /// </remarks>
      void RecoverSet(VssRecoveryOptions options);


      /// <summary>
      /// Begins an asynchronous operation that initiates a LUN resynchronization operation. This method is supported only on Windows server operating systems.
      /// </summary>
      /// <param name="options"><see cref="VssRecoveryOptions"/> flags that specify how the resynchronization is to be performed.</param>
      /// <param name="userCallback">An optional asynchronous callback, to be called when the read is complete.</param>
      /// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
      /// <returns>
      /// An <see cref="IVssAsyncResult"/> instance that represents this asynchronous operation.
      /// </returns>
      /// <remarks>
      ///   <para>
      /// Pass the <see cref="IVssAsyncResult"/> value returned to the <see cref="EndRecoverSet"/> method to release operating system resources used for this asynchronous operation.
      ///   <see cref="EndRecoverSet"/> must be called once for every call to <see cref="BeginRecoverSet"/>. You can do this either by using the same code that called <b>BeginRecoverSet</b> or
      /// in a callback passed to <b>BeginRecoverSet</b>.
      ///   </para>
      ///   <para>
      ///   <note>
      ///   <b>Windows XP, Windows 2003, Windows Vista, Windows 2008, Windows 7:</b> This method requires Windows Server 2008 R2.
      ///   </note>
      ///   </para>
      /// </remarks>
      IVssAsyncResult BeginRecoverSet(VssRecoveryOptions options, AsyncCallback userCallback, object state);

      /// <summary>
      /// Waits for a pending asynchronous operation to complete.
      /// </summary>
      /// <remarks>
      /// <b>EndRecoverSet</b> can be called once on every <see cref="IVssAsyncResult"/> from <see cref="BeginRecoverSet"/>.
      /// </remarks>
      /// <param name="asyncResult">The reference to the pending asynchronous request to finish. </param>
      /// /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="NotImplementedException">The provider for the volume does not support LUN resynchronization.</exception>
      /// <exception cref="VssBadStateException">Possible reasons for this return value include:
      ///		<list type="bullet">
      ///			<item><description>There is no hardware provider that supports the operation.</description></item>
      ///			<item><description>The requester did not successfully add any volumes to the recovery set.</description></item>
      ///			<item><description>The method was called in WinPE or in Safe mode.</description></item>
      ///			<item><description>he caller did not call the <see cref="InitializeForRestore"/> method before calling this method.</description></item>
      ///		</list>
      /// </exception>
      /// <exception cref="VssLegacyProviderException">This version of the hardware provider does not support this operation.</exception>
      /// <exception cref="VssProviderVetoException">An unexpected provider error occurred. If this error code is returned, the error must be described in an entry in the application event log, giving the user information on how to resolve the problem.</exception>
      /// <exception cref="VssUnselectedVolumeException">The resynchronization destination contained a volume that was not explicitly included.</exception>
      /// <exception cref="VssCannotRevertDiskIdException">The MBR signature or GPT ID for one or more disks could not be set to the intended value. Check the Application event log for more information.</exception>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP, Windows 2003, Windows Vista, Windows 2008, Windows 7:</b> This method requires Windows Server 2008 R2.
      ///         </note>
      ///     </para>
      /// </remarks>
      void EndRecoverSet(IAsyncResult asyncResult);
      #endregion
   }
}
