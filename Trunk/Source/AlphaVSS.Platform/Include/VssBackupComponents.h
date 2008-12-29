/* Copyright (c) 2008 Peter Palotas
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
#pragma once

#include "VssObjectType.h"
#include "VssSnapshotCompatibility.h"
#include "VssComponentType.h"
#include "VssObjectType.h"
#include "VssError.h"
#include "VssVolumeSnapshotAttributes.h"
#include "VssSnapshotProperties.h"
#include "VssProviderProperties.h"
#include "VssWriterComponents.h"
#include "VssExamineWriterMetadata.h"
#include "VssWriterFailure.h"
#include "VssWriterState.h"
#include "VssBackupType.h"
#include "VssRestoreType.h"
#include "VssWriterStatus.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	ref class VssAsync;

	/// <summary>
	/// 	<para>
	/// 		The <see cref="VssBackupComponents"/> class is used by a requester to poll writers about file status and to run backup/restore operations.
	/// 	</para>
	/// 	<para>
	/// 		A <see cref="VssBackupComponents"/> object can be used for only a single backup, restore, or Query operation.
	/// 	</para>
	/// 	<para>
	/// 		After the backup, restore, or Query operation has either successfully finished or been explicitly terminated, a requester must 
	/// 		release the <see cref="VssBackupComponents"/> object by calling <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.Dispose"/>. 
	/// 		A <see cref="VssBackupComponents"/> object must not be reused. For example, you cannot perform a backup or restore operation with the 
	/// 		same <see cref="VssBackupComponents"/> object that you have already used for a Query operation.
	/// 	</para>
	/// </summary>
	public ref class VssBackupComponents : IDisposable
	{
	public:
		/// <summary>
		/// The <see cref="IsVolumeSnapshotted"/> function determines whether any shadow copies exist for the specified volume.
		/// </summary>
		/// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
		/// <list type="bullet">
		/// <item><description>The path of a volume mount point with a backslash (\)</description></item>
		/// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
		/// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
		/// </list></param>
		/// <returns><see langword="true"/> if the volume has a shadow copy, and <see langword="false"/> if the volume does not have a shadow copy.</returns>
		/// <remarks>
		///	If this method returns <see langword="true"/>, use <see cref="GetSnapshotCompatibility"/> to find out the capabilities of that volume.
		/// <note>This method is not supported until Windows Vista.</note>
		/// </remarks>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log. For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381605(VS.85).aspx">Event and Error Handling Under VSS</see>.</exception>
		/// <exception cref="VssObjectNotFoundException">The specified volume was not found.</exception>
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the event log file. For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381605(VS.85).aspx">Event and Error Handling Under VSS</see>.</exception>
		/// <exception cref="NotSupportedException">The operation is not supported on the current operating system.</exception>
		static bool IsVolumeSnapshotted(String^ volumeName);

		/// <summary>
		/// Returns a combination of flags from <see cref="VssSnapshotCompatibility"/> indicating whether certain volume control or file I/O operations 
		/// are disabled for the given volume if a shadow copy of it exists.</summary>
		/// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
		/// <list type="bullet">
		/// <item><description>The path of a volume mount point with a backslash (\)</description></item>
		/// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
		/// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
		/// </list></param>
		/// <returns>A bit mask (or bitwise OR) of <see cref="VssSnapshotCompatibility"/> values that indicates whether certain volume control or file I/O operations are disabled for the given volume if a shadow copy of it exists.</returns>
		/// <remarks>
		///		<note>This method is not supported until Windows Vista.</note>
		/// </remarks>
		/// <exception cref="InvalidOperationException">No snapshot exists for the specified volume.</exception>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log. For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381605(VS.85).aspx">Event and Error Handling Under VSS</see>.</exception>
		/// <exception cref="VssObjectNotFoundException">The specified volume was not found.</exception>
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the event log file. For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa381605(VS.85).aspx">Event and Error Handling Under VSS</see>.</exception>
		/// <exception cref="NotSupportedException">The operation is not supported on the current operating system.</exception>
		static VssSnapshotCompatibility GetSnapshotCompatibility(String^ volumeName);

		/// <summary>Checks the registry for writers that should block revert operations on the specified volume.</summary>
		/// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
		/// <list type="bullet">
		/// <item><description>The path of a volume mount point with a backslash (\)</description></item>
		/// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
		/// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
		/// </list></param>
		/// <returns><see langword="true"/> if the volume contains components from any writers that are listed in the registry as writers that 
		/// should block revert operations; otherwise, <see langword="false"/>.</returns>
		/// <remarks><note>This method requires Windows Server 2008 or Windows Server 2003 SP1</note></remarks>
		/// <exception cref="UnauthorizedAccessException">The caller is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="NotSupportedException">The operation is not supported on the current operating system.</exception>
		static bool ShouldBlockRevert(String^ volumeName);

		/// <summary>Initializes a new instance of the <see cref="VssBackupComponents"/> class.</summary>
		VssBackupComponents();

		/// <summary>Releases any resources aquired by this instance.</summary>
		~VssBackupComponents();

		/// <summary>Releases any resources aquired by this instance.</summary>
		!VssBackupComponents();

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
		///		The <see cref="AddAlternativeLocationMapping"/> method is used by a requester to indicate that an alternate location 
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
		void AddAlternativeLocationMapping(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ path, String^ filespec, bool recursive, String^ destination);

		/// <summary>
		/// The <see cref="AddComponent"/> method is used to explicitly add to the backup set in the Backup Components Document all required 
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
		void AddComponent(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName);

		/// <summary>
		/// The <see cref="AddNewTarget"/> method is used by a requester during a restore operation to indicate that the backup application plans 
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
		void AddNewTarget(Guid writerId, VssComponentType componentType, String ^ logicalPath, String ^ componentName, String ^ path, String ^ fileName, bool recursive, String ^ alternatePath);

		/// <summary>
		/// The <see cref="AddRestoreSubcomponent"/> method indicates that a Subcomponent member of a component set, which had been marked as 
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
		void AddRestoreSubcomponent(Guid writerId, VssComponentType componentType, String^ logicalPath, String ^componentName, String^ subcomponentLogicalPath, String^ subcomponentName);

		/// <summary>
		/// The <see cref="AddToSnapshotSet"/> method adds an original volume to the shadow copy set. 
		/// </summary>
		/// <param name="volumeName">String containing the name of the volume to be shadow copied. The name must be in one of the following formats:
		///		<list type="bullet">
		///			<item><description>The path of a volume mount point with a backslash (\)</description></item>
		///			<item><description>A drive letter with backslash (\), for example, D:\</description></item>
		///			<item><description>A unique volume name of the form "\\?\Volume{GUID}\" (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
		///		</list>
		/// </param>
		/// <param name="providerId">The provider to be used. <see cref="Guid::Empty" /> can be used, in which case the default provider will be used.</param>
		/// <returns>Identifier of the added shadow copy.</returns>
		/// <remarks>
		/// 	<para>
		/// 		The maximum number of shadow copies in a single shadow copy set is 64.
		/// 	</para>
		/// 	<para>If <paramref name="providerId"/> is <see cref="Guid::Empty"/>, the default provider is selected according to the following algorithm:
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
		/// <exception cref="VssVolumeNotSupportedException">The value of the <paramref name="providerId"/> parameter is <see cref="Guid::Empty" /> and no VSS provider indicates that it supports the specified volume.</exception>
		/// <exception cref="VssVolumeNotSupportedByProviderException">The volume is not supported by the specified provider.</exception>
		/// <exception cref="VssUnexpectedProviderError">The provider returned an unexpected error code.</exception>
		Guid AddToSnapshotSet(String^ volumeName, Guid providerId);

		/// <summary>
		/// The <see cref="BackupComplete"/> method causes VSS to generate a <b>BackupComplete</b> event, which signals writers that the backup 
		/// process has completed. 
		/// </summary>
		/// <returns>A <see cref="VssAsync"/> instance representing this operation.</returns>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		/// <exception cref="VssUnexpectedWriterError">An unexpected error occurred during communication with writers. The error code is logged in the error log file.</exception>
		VssAsync^ BackupComplete();

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
		///		The <see cref="DeleteSnapshots"/> method deletes one or more shadow copies or a shadow copy set. 
		/// </summary>
		/// <param name="sourceObjectId">Identifier of the shadow copy or a shadow copy set to be deleted.</param>
		/// <param name="sourceObjectType">Type of the object on which all shadow copies will be deleted. The value of this parameter is <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.Snapshot" /> or <see dref="F:Alphaleonis.Win32.Vss.VssObjectType.SnapshotSet" />.</param>
		/// <param name="forceDelete">If the value of this parameter is <see langword="true"/>, the provider will do everything possible to delete the shadow copy or shadow copies in a shadow copy set. If it is <see langword="false"/>, no additional effort will be made.</param>
		/// <remarks>
		/// 	<para>
		/// 		Multiple shadow copies in a shadow copy set are deleted sequentially. If an error occurs during one of these individual 
		/// 		deletions, <b>DeleteSnapshots</b> will throw an exception immediately; no attempt will be made to delete any remaining shadow copies. 
		/// 		The identifier of the undeleted shadow copy can be found in the instance of <see cref="VssDeleteSnapshotsFailedException"/> thrown.
		/// 	</para>
		/// 	<para>
		/// 		The requester is responsible for serializing the delete shadow copy operation.
		/// 	</para>
		/// 	<para>
		/// 		During a backup, shadow copies are automatically released as soon as the <see cref="VssBackupComponents"/> instance is 
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
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the error log.</exception>
		/// <returns>The total number of snapshots that were deleted</returns>
		int DeleteSnapshots(Guid sourceObjectId, VssObjectType sourceObjectType, bool forceDelete);
		
		/// <summary>
		/// The <see cref="DisableWriterClasses"/> method prevents a specific class of writers from receiving any events.
		/// </summary>
		/// <param name="writerClassIds">An array containing one or more writer class identifiers.</param>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		void DisableWriterClasses(array<Guid> ^ writerClassIds);

		/// <summary>
		/// The <see cref="DisableWriterInstances"/> method disables a specified writer instance or instances.
		/// </summary>
		/// <param name="writerInstanceIds">An array containing one or more writer instance identifiers.</param>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameters is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		void DisableWriterInstances(array<Guid> ^ writerInstanceIds);

		/// <summary>
		/// Commits all shadow copies in this set simultaneously. 
		/// </summary>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object has not been initialized or the prerequisite calls for a given shadow copy context have not been made prior to calling <b>DoSnapshotSet</b>. </exception>
		/// <exception cref="VssInsufficientStorageException">The system or provider has insufficient storage space. If possible delete any old or unnecessary persistent shadow copies and try again. This error code is only returned via the QueryStatus method on the <see cref="VssAsync"/>.</exception>
		/// <exception cref="VssFlushWritesTimeoutException">The system was unable to flush I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
		/// <exception cref="VssHoldWritesTimeoutException">The system was unable to hold I/O writes. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times.</exception>
		/// <exception cref="VssProviderVetoException">The provider was unable to perform the request at this time. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times. This error code is only returned via the <see dref="M:Alphaleonis.Win32.Vss.VssAsync.QueryStatus"/> method on the <see cref="VssAsync"/> instance returned by this method.</exception>
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
		/// <exception cref="VssUnexpectedProviderError">The provider returned an unexpected error code. This can be a transient problem. It is recommended to wait ten minutes and try again, up to three times. This error code is only returned via the <see dref="M:Alphaleonis.Win32.Vss.VssAsync.QueryStatus"/> method on the <see cref="VssAsync"/> instance returned by this method.</exception> 
		VssAsync^ DoSnapshotSet();

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
		///		<see cref="VssBackupComponents"/> class.
		///	</para>
		///	<para>
		///		After <see cref="GatherWriterMetadata"/> is called, these calls have no effect.
		///	</para>
		/// </remarks>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		void EnableWriterClasses(array<Guid> ^ writerClassIds);

		/// <summary>
		/// The <see cref="ExposeSnapshot"/> method exposes a shadow copy either by mounting it as a device on a drive letter or mount point, or 
		/// as a file share. 
		/// </summary>
		/// <param name="snapshotId">Shadow copy identifier.</param>
		/// <param name="pathFromRoot">
		///		<para>The path to the portion of the volume made available when exposing a shadow copy as a file share. The value of this parameter must be NULL when exposing a shadow copy locally; that is, by mounting to a drive letter or a mount point.</para>
		///     <para>The path cannot contain environment variables (for example, %MyEnv%) or wildcard characters.</para>
		///		<para>There is no requirement that the path end with a backslash ("\"). It is up to applications that retrieve this information to check.</para>
		/// </param>
		/// <param name="attributes">Attributes of the exposed shadow copy indicating whether it is exposed locally or remotely. The value must 
		/// be either the <see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.ExposedLocally" /> or the <see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.ExposedRemotely" /> 
		/// value of <see cref="VssVolumeSnapshotAttributes" />.</param>
		/// <param name="expose">When a shadow copy is exposed as a file share, the value of this parameter is the share name. If a shadow copy 
		/// is exposed by mounting it as a device, the parameter value is a drive letter followed by a colon, for example, "X:" or a mount point 
		/// path (for example, "X:\a\b"). If the value of this parameter is <see langword="null"/>, then VSS determines the share name or drive 
		/// letter if the <paramref name="attributes" /> parameter is <see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.ExposedRemotely" />. </param>
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
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the error log.</exception>
		String^ ExposeSnapshot(Guid snapshotId, String ^ pathFromRoot, VssVolumeSnapshotAttributes attributes, String ^ expose);

		/// <summary>The <see cref="FreeWriterMetadata"/> method frees system resources allocated when <see cref="GatherWriterMetadata" /> was called.</summary>
		/// <remarks>
		/// 	<para>
		/// 		This method should never be called prior to the completion of <see cref="VssBackupComponents::GatherWriterMetadata"/>. 
		/// 		The result of calling the method prior to completion of the metadata gather is undefined.
		/// 	</para>
		/// 	<para>
		/// 		Once writer metadata has been freed, it cannot be recovered by the current instance of the <see cref="VssBackupComponents"/> class. 
		/// 		It will be necessary to create a new instance of <see cref="VssBackupComponents"/>, and call the 
		/// 		<see cref="VssBackupComponents::GatherWriterMetadata"/> method again.
		/// 	</para>
		/// </remarks>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		void FreeWriterMetadata();

		/// <summary>The <see cref="FreeWriterStatus" /> method frees system resources allocated during the call to <see cref="GatherWriterStatus" />.</summary>
		/// <exception cref="OutOfMemoryException">The caller is out of memory or other system resources.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>
		void FreeWriterStatus();

		/// <summary>
		/// 	The <see cref="GatherWriterMetadata"/> method prompts each writer to send the metadata they have collected. The method will generate an <c>Identify</c> event to communicate with writers.
		/// </summary>
		/// <remarks><see cref="GatherWriterMetadata"/> should be called only once during the lifetime of a given <see cref="VssBackupComponents"/> object.</remarks>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
		VssAsync^ GatherWriterMetadata();

		/// <summary>
		/// 	The <see cref="GatherWriterStatus"/> method prompts each writer to send a status message.
		/// </summary>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <remarks>The caller of this method should also call <see cref="VssBackupComponents::FreeWriterStatus"/> after receiving the status of each writer.</remarks>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssWriterInfrastructureException">The writer infrastructure is not operating properly. Check that the Event Service and VSS have been started, and check for errors associated with those services in the error log.</exception>
		VssAsync^ GatherWriterStatus();
		
		/// <summary>
		/// 	The <see cref="GetSnapshotProperties"/> method gets the properties of the specified shadow copy. 
		/// </summary>
		/// <param name="snapshotId">The identifier of the shadow copy of a volume as returned by <see cref="AddToSnapshotSet"/>. </param>
		/// <returns>A <see cref="VssSnapshotProperties"/> instance containing the shadow copy properties.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the error log.</exception>
		VssSnapshotProperties^ GetSnapshotProperties(Guid snapshotId);

		/// <summary>A read-only list containing information about the components of each writer that has been stored in a requester's Backup Components Document.</summary>
		/// <remarks>
		/// 	<para>
		/// 		<see cref="WriterComponents"/> retrieves component information for a component stored in the Backup Components Document by earlier 
		/// 		calls to <see cref="VssBackupComponents::AddComponent"/>.
		/// 	</para>
		/// 	<para>
		/// 		The information in the components stored in the Backup Components Document is not static. If a writer updates a component during a 
		/// 		restore, that change will be reflected in the component retrieved by <see cref="WriterComponents"/>. This is in contrast with 
		/// 		component information found in the <see cref="VssWMComponent"/> object returned by <see cref="VssExamineWriterMetadata::Components"/>. 
		/// 		That information is read-only and comes from the Writer Metadata Document of a writer process.
		/// 	</para>
		/// 	<para>
		/// 		The <see cref="VssWriterComponents"/> instances that are returned should not be cached, because the following 
		/// 		<see cref="VssBackupComponents"/> methods cause the instances that are returned by <see cref="WriterComponents"/> to 
		/// 		be no longer valid:
		/// 		<list type="bullet">
		/// 			<item><description><see cref="VssBackupComponents::PrepareForBackup"/></description></item>
		/// 			<item><description><see cref="VssBackupComponents::DoSnapshotSet"/></description></item>
		/// 			<item><description><see cref="VssBackupComponents::BackupComplete"/></description></item>
		/// 			<item><description><see cref="VssBackupComponents::PreRestore"/></description></item>
		/// 			<item><description><see cref="VssBackupComponents::PostRestore"/></description></item>
		/// 		</list>
		/// 	</para>
		/// 	<para>
		/// 		If you call one of these methods after you have retrieved a <see cref="VssWriterComponents"/> instance by calling 
		/// 		<see cref="WriterComponents"/>, you cannot reuse that instance, because it is no longer valid. Instead, you must call 
		/// 		<see cref="WriterComponents"/> again to retrieve a new <see cref="VssWriterComponents"/> instance.
		/// 	</para>
		/// </remarks>
		/// <value>
		///     A read-only list containing information about the components of each writer that has been stored in a requester's Backup Components Document.
		///		<note type="caution">This list must not be accessed after the <see cref="VssBackupComponents"/> from which it 
		///     was obtained has been disposed.</note>
		/// </value>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		property IVssListAdapter<VssWriterComponents^>^ WriterComponents { IVssListAdapter<VssWriterComponents^>^ get(); }


		/// <summary>A read-only list containing metadata for the writers running on the systsem.</summary>
		/// <value>
		///     A read-only list containing metadata for the writers running on the system.
		///		<note type="caution">This list must not be accessed after the <see cref="VssBackupComponents"/> from which it 
		///     was obtained has been disposed.</note>
		/// </value>
		/// <remarks>
		/// 	<para>
		/// 		A requester must call the asynchronous operation <see cref="VssBackupComponents::GatherWriterMetadata"/> and wait for it 
		/// 		to complete prior to using <see cref="WriterMetadata"/>.
		/// 	</para>
		/// 	<para>
		/// 		Although <see cref="VssBackupComponents::GatherWriterMetadata"/> must be called prior to either a restore or backup operation, 
		/// 		<see cref="WriterMetadata"/> is not typically used for restores.
		/// 	</para>
		/// 	<para>
		/// 		Component information retrieved (during backup operations) using <see cref="VssExamineWriterMetadata::Components"/>, where the 
		/// 		<see cref="VssExamineWriterMetadata"/> instance has been returned by <see cref="WriterMetadata"/>, comes from the Writer 
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
		property IVssListAdapter<VssExamineWriterMetadata^>^ WriterMetadata { IVssListAdapter<VssExamineWriterMetadata^>^ get(); }


		/// <summary>A read-only list containing the status of the writers.</summary>
		/// <value>
		///		A read-only list containing <see cref="VssWriterStatus"/> instances representing the returned status for each respective writer.
		///		<note type="caution">This list must not be accessed after the <see cref="VssBackupComponents"/> from which it 
		///     was obtained has been disposed.</note>
		/// </value>
		/// <remarks>
		/// 	<para>
		/// 		A requester must call the asynchronous operation <see cref="VssBackupComponents::GatherWriterStatus"/> and wait for it to 
		/// 		complete prior to using <see cref="VssBackupComponents::WriterStatus"/>.
		/// 	</para>
		/// </remarks>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The specified shadow copy does not exist.</exception>
		property IVssListAdapter<VssWriterStatus^>^ WriterStatus { IVssListAdapter<VssWriterStatus^>^ get(); }


		/// <summary>The ImportSnapshots method imports shadow copies transported from a different machine.</summary>
		/// <note>This method is supported only on Windows Server operating systems.</note>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <remarks>
		/// 	<para>Only one shadow copy can be imported at a time.</para>
		/// 	<para>The requester is responsible for serializing the import shadow copy operation.</para>
		///		<para>For more information see the <see href="http://msdn.microsoft.com/en-us/library/aa382683(VS.85).aspx">MSDN documentation on IVssBackupComponents::ImportSnapshots Method</see></para>
		///		<para>Requires Windows Server 2008, Windows Server 2003 SP1, Windows Server 2003, Enterprise Edition, or Windows Server 2003, Datacenter Edition.</para>
		/// </remarks>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		VssAsync^ ImportSnapshots();

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
		/// 	The XML document supplied to this method initializes the <see cref="VssBackupComponents"/> object with metadata previously stored by 
		/// 	a call to <see cref="SaveAsXml"/>. Users should not tamper with this metadata document.
		/// </remarks>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
		void InitializeForBackup(String^ xml);

		/// <summary>
		///		The <see cref="InitializeForRestore"/> method initializes the IVssBackupComponents interface in preparation for a restore operation.
		/// </summary>
		/// <param name="xml">
		///		XML string containing the Backup Components Document generated by a backup operation and saved by 
		///		<see cref="SaveAsXml"/>.
		/// </param>
		/// <remarks>
		/// 	The XML document supplied to this method initializes the <see cref="VssBackupComponents"/> object with metadata previously stored by a call to 
		/// 	<see cref="SaveAsXml"/>. Users should not tamper with this metadata document.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="xml" /> is <see langword="null"/></exception>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssInvalidXmlDocumentException">The load operation of the specified XML document failed.</exception>
		void InitializeForRestore(String^ xml);

		/// <summary>
		/// 	The <see cref="IsVolumeSupported"/> method determines whether the specified provider supports shadow copies on the specified volume.
		/// </summary>
		/// <param name="providerId">
		/// 	Provider identifier. If the value is <see cref="Guid::Empty"/>, <see cref="IsVolumeSupported"/> checks whether any provider 
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
		/// 		<see cref="IsVolumeSupported"/> will return <see langword="true"/> if it is possible to create shadow copies on the given volume, 
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
		bool IsVolumeSupported(Guid providerId, String^ volumeName);

		/// <summary>
		///	The <see cref="PostRestore"/> method will cause VSS to generate a <c>PostRestore</c> event, signaling writers that the current 
		///	restore operation has finished.
		/// </summary>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The specified volume was not found or was not available.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
		VssAsync^ PostRestore();

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
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		VssAsync^ PrepareForBackup();

		/// <summary>
		/// The <see cref="PreRestore"/> method will cause VSS to generate a <c>PreRestore</c> event, signaling writers to prepare for a 
		/// coming restore operation.
		/// </summary>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		VssAsync^ PreRestore();

		/// <summary>
		/// 	The <see cref="QuerySnapshots"/> method queries the completed shadow copies in the system that reside in the current context. 
		/// 	The method can be called only during backup operations.
		/// </summary>
		/// <returns>A list of <see cref="VssSnapshotProperties"/> objects representing the requested information.</returns>
		/// <remarks>
		/// 	 <para>
		/// 		Because <see cref="QuerySnapshots"/> returns only information on completed shadow copies, the only shadow copy state it can disclose 
		/// 		is <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotState.Created"/>.
		/// 	 </para>
		/// 	 <para>
		/// 		The method may be called only during backup operations and must be preceded by calls to <see cref="InitializeForBackup"/> and 
		/// 		<see cref="SetContext"/>.
		/// 	 </para>
		/// 	 <para>
		/// 		The method will return only information 
		/// 		about shadow copies with the current context (set by <see cref="SetContext"/>). For instance, if the 
		/// 		<see cref="VssSnapshotContext"/> context is set to <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.Backup"/>, <see cref="QuerySnapshots"/> will not 
		/// 		return information on a shadow copy created with a context of <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.FileShareBackup" />.
		/// 	 </para>
		/// </remarks>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="UnauthorizedAccessException">The caller is not an administrator or a backup operator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The queried object is not found.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the error log.</exception>		
		System::Collections::Generic::IEnumerable<VssSnapshotProperties^> ^QuerySnapshots();

		/// <summary>
		/// 	The <see cref="QueryProviders"/> method queries providers on the system. 
		/// 	The method can be called only during backup operations.
		/// </summary>
		/// <returns>A list of <see cref="VssProviderProperties"/> objects representing the requested information.</returns>
		/// <remarks>
		/// 	 <para>
		/// 		The method may be called only during backup operations and must be preceded by calls to <see cref="InitializeForBackup"/> and 
		/// 		<see cref="SetContext"/>.
		/// 	 </para>
		/// </remarks>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="UnauthorizedAccessException">The caller is not an administrator or a backup operator.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The queried object is not found.</exception>
		/// <exception cref="VssProviderVetoException">Expected provider error. The provider logged the error in the event log.</exception>
		/// <exception cref="VssUnexpectedProviderError">Unexpected provider error. The error code is logged in the error log.</exception>
		System::Collections::Generic::IEnumerable<VssProviderProperties^> ^QueryProviders();

		/// <summary>
		/// 	The <see cref="QueryRevertStatus"/> method returns an <see cref="VssAsync"/> instance that can be used to determine the status of 
		/// 	the revert operation.
		/// </summary>
		/// <param name="volumeName">Name of the volume. The name of the volume to be checked must be in one of the following formats:
		/// <list type="bullet">
		/// <item><description>The path of a volume mount point with a backslash (\)</description></item>
		/// <item><description>A drive letter with backslash (\), for example, D:\</description></item>
		/// <item><description>A unique volume name of the form \\?\Volume{GUID}\ (where GUID is the unique global identifier of the volume) with a backslash (\)</description></item>
		/// </list></param>
		/// <returns>A <see cref="VssAsync"/> instance representing this asynchronous operation.</returns>
		/// <remarks>
		/// 	The revert operation will continue even if the computer is rebooted, and cannot be canceled or undone, except by restoring a 
		/// 	backup created using another method.
		/// <note><b>Windows XP, Windows Server 2003 and Windows Vista:</b> This method requires Windows Server 2008 or Windows Server 2003 SP1</note>
		/// </remarks>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="UnauthorizedAccessException">The calling process has insufficient privileges.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The <paramref name="volumeName"/> parameter is not a valid volume.</exception>
		/// <exception cref="VssVolumeNotSupportedException">Revert is not supported on this volume.</exception>
		/// <exception cref="NotImplementedException">The provider for the volume does not support revert operations.</exception>
		/// <exception cref="NotSupportedException">This operation is not supported on the current operating system.</exception>
		VssAsync^ QueryRevertStatus(String^ volumeName);

		/// <summary>
		/// 	The <see cref="RevertToSnapshot"/> method reverts a volume to a previous shadow copy. Only shadow copies created with persistent 
		/// 		contexts (<see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.AppRollback" />, <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.ClientAccessible" />, 
		/// 		<see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.ClientAccessibleWriters" /> or <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.NasRollback" />)
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
		String^ SaveAsXml();

		/// <summary>
		///		The <see cref="SetAdditionalRestores"/> method is used by a requester during incremental or differential restore operations to indicate 
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
		void SetAdditionalRestores(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool additionalResources);

		/// <summary>
		/// 	The <see cref="SetBackupOptions"/> method sets a string of private, or writer-dependent, backup parameters for a component.
		/// </summary>
		/// <param name="writerId">
		/// 	Writer identifier.
		/// </param>
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
		void SetBackupOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ backupOptions);
		
		/// <summary>The <see cref="SetBackupState"/> method defines an overall configuration for a backup operation.</summary>
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
		/// 	The <see cref="SetBackupSucceeded"/> method indicates whether the backup of the specified component of a specific writer was successful.
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
		/// 		writers the state of each components backup using <see cref="VssComponent::BackupSucceeded"/>.
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
		void SetBackupSucceeded(Guid instanceId, Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool succeeded);
		
		/// <summary>
		/// 	The <see cref="SetContext"/> method sets the context for subsequent shadow copy-related operations. 
		/// </summary>
		/// <param name="context">
		/// 	The context to be set. The context must be one of the supported values of <see cref="VssSnapshotContext"/> or a supported bit 
		/// 	mask (or bitwise OR) of <see cref="VssVolumeSnapshotAttributes" /> with a valid <see cref="VssSnapshotContext" />. 
		/// </param>
		/// <remarks>
		/// 	<para>
		/// 		The default context for VSS shadow copies is <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.Backup"/>.
		/// 	</para>
		/// 	<para>
		/// 		<b>Windows XP:</b> The only supported context is the default context, <see dref="F:Alphaleonis.Win32.Vss.VssSnapshotContext.Backup"/>. Therefore, calling 
		/// 		<see cref="SetContext" /> under Windows XP throws a <see cref="NotImplementedException"/>. 
		/// 	</para>
		/// 	<para>
		/// 		<see cref="SetContext"/> can be called only once, and it must be called prior to calling most VSS functions.
		/// 	</para>
		/// 	<para>
		/// 		For details on how the context set by <see cref="SetContext"/> affects how a shadow copy is created and managed, see 
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
		/// 	The <see cref="SetFileRestoreStatus"/> method indicates whether some, all, or no files were successfully restored.
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
		/// 	If all of the files were restored, the value of this parameter is <see dref="F:Alphaleonis.Win32.Vss.VssFileRestoreStatus.All" />. 
		/// 	If some of the files were restored, the value of this parameter is <see dref="F:Alphaleonis.Win32.Vss.VssFileRestoreStatus.Failed" />. If none of the files 
		/// 	were restored, the value of this parameter is <see dref="F:Alphaleonis.Win32.Vss.VssFileRestoreStatus.None" />.
		/// </param>
		/// <remarks>This method should be called between calls to <see cref="PreRestore"/> and <see cref="PostRestore"/>.</remarks>
		/// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
		/// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
		void SetFileRestoreStatus(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, VssFileRestoreStatus status);

		/// <summary>
		/// 	<para>
		/// 		The <see cref="SetPreviousBackupStamp"/> method sets the backup stamp of an earlier backup operation, upon which a differential or 
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
		/// <para>Therefore, the value set with <see cref="SetPreviousBackupStamp"/> should either be retrieved from the stored Backup Components Document of an earlier backup operation (using <see dref="P:Alphaleonis.Win32.Vss.VssComponent.BackupStamp"/> for the correct component), or from information stored by the requester into its own internal records.</para>
		/// <para>A writer will then obtain this value (using <c>IVssComponent::GetPreviousBackupStamp</c>) and using it will be able to mark the appropriate files for participation in an incremental or differential backup.</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
		/// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
		void SetPreviousBackupStamp(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ previousBackupStamp);

		/// <summary>The <see cref="SetRangesFilePath"/> method is used when a partial file operation requires a ranges file, and that file has been restored to a location other than its original one.</summary>		
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
		///     of items in <see dref="P:Alphaleonis.Win32.Vss.VssComponent.PartialFiles"/>.
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
		void SetRangesFilePath(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, int partialFileIndex, String^ rangesFile);

		/// <summary>
		///		The <see cref="SetRestoreOptions"/> method sets a string of private, or writer-dependent, restore parameters for a writer component.
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
		void SetRestoreOptions(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, String^ restoreOptions);
		
		/// <summary>
		/// 	The <see cref="SetRestoreState"/> method defines an overall configuration for a restore operation.
		/// </summary>
		/// <param name="restoreType">The type of restore to be performed.</param>
		/// <remarks>
		/// 	<para>Typically, most restore operations will not need to override the default restore type <see dref="F:Alphaleonis.Win32.Vss.VssRestoreType.Undefined" />.</para>
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

		/// <summary>The <see cref="SetSelectedForRestore"/> method indicates whether the specified selectable component is selected for restoration.</summary>
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
		/// 	<para><see cref="SetSelectedForRestore"/> can only be called for components that were explicitly added to the backup document at backup time using <see cref="AddComponent"/>. Restoring a component that was implicitly selected for backup as part of a component set must be done by calling <see cref="SetSelectedForRestore"/> on the closest ancestor component that was added to the document. If only this component's data is to be restored, that should be accomplished through <see cref="AddRestoreSubcomponent"/>; this can only be done if the component is selectable for restore.</para>
		/// 	<para>This method must be called before <see cref="PreRestore"/>.</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
		/// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssObjectNotFoundException">The backup component does not exist.</exception>
		/// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
		void SetSelectedForRestore(Guid writerId, VssComponentType componentType, String^ logicalPath, String^ componentName, bool selectedForRestore);
		
		/// <summary>
		/// 	The <see cref="StartSnapshotSet"/> method creates a new, empty shadow copy set.
		/// </summary>
		/// <returns>Shadow copy set identifier.</returns>
		/// <remarks>This method must be called before <see cref="PrepareForBackup"/> during backup operations.</remarks>
		/// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
		/// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
		/// <exception cref="VssBadStateException">The backup components object is not initialized, this method has been called during a restore operation, or this method has not been called within the correct sequence.</exception>		
		/// <exception cref="VssSnapshotSetInProgressException">The creation of a shadow copy is in progress, and only one shadow copy creation operation can be in progress at one time. Either wait to try again or return with a failure error code.</exception>
		Guid StartSnapshotSet();
	private:
		IVssBackupComponents *mBackup;

		ref class WriterMetadataList : VssListAdapter<VssExamineWriterMetadata^>
		{
		public:
			WriterMetadataList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property VssExamineWriterMetadata^ default[int] { virtual VssExamineWriterMetadata^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};


		ref class WriterComponentsList : VssListAdapter<VssWriterComponents^>
		{
		public:
			WriterComponentsList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property VssWriterComponents^ default[int] { virtual VssWriterComponents^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};

		ref class WriterStatusList : VssListAdapter<VssWriterStatus^>
		{
		public:
			WriterStatusList(VssBackupComponents^ backupComponents);

			property int Count { virtual int get() override; }
			property VssWriterStatus^ default[int] { virtual VssWriterStatus^ get(int index) override; }
		private:
			VssBackupComponents^ mBackupComponents;
		};


		WriterMetadataList^ mWriterMetadata;
		WriterComponentsList^ mWriterComponents;
		WriterStatusList^ mWriterStatus;
	};


}
} }
