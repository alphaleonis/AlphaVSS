using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
	/// <summary>
	/// 	<para>
	/// 		Interface containing methods for examining and modifying information about components contained in a requester's Backup Components Document.
	/// 	</para>
	/// </summary>
	/// <remarks>
	/// 	<para>
	/// 		<see cref="VssComponent"/> objects can be obtained only for those components that have been explicitly added 
	/// 		to the Backup Components Document during a backup operation by the <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.AddComponent(System.Guid,System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String)"/> 
	/// 		method.
	/// 	</para>
	/// 	<para>
	/// 		Information about components explicitly added during a restore operation using 
	/// 		<see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.AddRestoreSubcomponent(System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String,System.String,System.String)"/> are not available through the <see cref="VssComponent"/>
	/// 		interface.
	/// 	</para>
	/// 	<para>
	/// 		For more information, see <see href="http://msdn.microsoft.com/en-us/library/aa382871(VS.85).aspx">the MSDN documentation on 
	/// 		the IVssComponent Interface</see>.
	/// 	</para>
	/// </remarks>    
    public interface IVssComponent : IDisposable
    {
#if false
		// These methods are specific to writers, only supporting requesters at this time, so these are not included.
		void AddDifferencedFilesByLastModifyTime(string path, string fileSpec, bool recursive, DateTime lastModifyTime);
		void AddDifferencedFilesByLastModifyTime(VssDifferencedFileInfo^ differencedFile);

		void AddDirectedTarget(VssDirectedTargetInfo ^directedTarget);
		void AddDirectedTarget(String ^ sourcePath, string sourceFileName, string sourceRangeList, string destinationPath, string destinationFileName, string destinationRangeList);

		void AddParitalFile(VssPartialFileInfo^ partialFile);
		void AddPartialFile(string path, string filename, string ranges, string metaData);

		string RestoreMetadata { string get(); }
		string BackupMetadata { string get(); }
		
		void SetBackupMetadata(string metadata);
		void SetBackupStamp(string metadata);
		void SetPostRestoreFailureMsg(string metadata);
		void SetPreRestoreFailureMsg(string metadata);
		void SetRestoreMetadata(string metadata);
		void SetRestoreTarget(VssRestoreTarget target);

#endif

		/// <summary>
		/// 	The <see cref="AdditionalRestores" /> is used by a writer during incremental or differential restore 
		/// 	operations to determine whether a given component will require additional restore operations to completely retrieve it, 
		/// 	but can also be called by a requester.
		/// </summary>
		/// <value>
		///		If <see langword="true"/>, additional restores will occur for the 
		///		current component. If <see langword="false"/>, additional restores will not occur.
		/// </value>
		bool AdditionalRestores { get; }

		/// <summary>
		/// 	<para>
		/// 		The backup options specified to the writer that manages the currently selected component or component set 
		/// 		by a requester using <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.SetBackupOptions(System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String,System.String)"/>.
		/// 	</para>
		/// </summary>
		/// <value>The backup options for the current writer.</value>
		string BackupOptions { get; }
		
		/// <summary>The backup stamp string stored by a writer for a given component.</summary>
		/// <value>The backup stamp indicating the time at which the component was backed up.</value>
		string BackupStamp { get; }

		/// <summary>
		/// 	The status of a complete attempt at backing up all the files of a selected component or component set as a 
		/// 	<see cref="VssFileRestoreStatus"/> enumeration.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if the backup was successful and <see langword="false"/> if it was not.
		/// </value>
		bool BackupSucceeded { get; }

		/// <summary>The logical name of this component.</summary>
		/// <value>The logical name of this component.</value>
		string ComponentName { get; }

		/// <summary>The type of this component in terms of the <see cref="ComponentType"/> enumeration.</summary>
		/// <value>The type of this component.</value>
		VssComponentType ComponentType { get; }

		/// <summary>
		/// 	The status of a completed attempt to restore all the files of a selected component or component set 
		/// 	as a <see cref="VssFileRestoreStatus" /> enumeration.
		/// </summary>
		/// <value>
		/// 	A value of the <see cref="VssFileRestoreStatus" /> enumeration that specifies whether all files were successfully restored.
		/// </value>		
		VssFileRestoreStatus FileRestoreStatus { get; }

		/// <summary>The logical path of this component.</summary>
		/// <value>The logical path of this component.</value>
		string LogicalPath { get; }

		/// <summary>The failure message generated by a writer while handling the <c>PostRestore</c> event if one was set.</summary>
		/// <value>The failure message that describes an error that occurred while processing the <c>PostRestore</c> event.</value>
		string PostRestoreFailureMsg { get; }

		/// <summary>The failure message generated by a writer while handling the <c>PreRestore</c> event if one was set.</summary>
		/// <value>The failure message that describes an error that occurred while processing the <c>PreRestore</c> event.</value>
		string PreRestoreFailureMsg { get; }

		/// <summary>
		/// 	A previous backup stamp loaded by a requester in the Backup Components Document. The value is used by a writer when 
		/// 	deciding if files should participate in differential or incremental backup operation.
		/// </summary>
		/// <value>
		/// 	The time stamp of a previous backup so that a differential or incremental backup can be correctly implemented.
		/// </value>
		string PreviousBackupStamp { get; }

		/// <summary>The restore options specified to the current writer by a requester using 
		/// <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.SetRestoreOptions(System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String,System.String)"/>.</summary>
		/// <value>The restore options of the writer.</value>
		string RestoreOptions { get; }

		/// <summary>The restore target (in terms of the <see cref="VssRestoreTarget"/> enumeration) for the current component. Can only be called during a restore operation.</summary>
		/// <value>A value from the <see cref="VssRestoreTarget"/> enumeration containing the restore target information.</value>
		VssRestoreTarget RestoreTarget { get; }

		/// <summary>Determines whether the current component has been selected to be restored.</summary>
		/// <value>If the returned value of this parameter is <see langword="true"/>, the component has been selected to be restored. If <see langword="false"/>, it has not been selected.</value>
		bool IsSelectedForRestore { get; }
		
		/// <summary>A collection of mapping information for the file set's alternate location for file restoration.</summary>
		/// <value>A read-only list containing the alternate location to which files were actually restored. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		/// <remarks>See <see href="http://msdn.microsoft.com/en-us/library/aa383473(VS.85).aspx">the MSDN documentation on the IVssComponent::GetAlternateLocationMapping method</see> for more information.</remarks>
		IList<VssWMFileDescription> AlternateLocationMappings { get; }

		/// <summary>
		/// 	Information stored by a writer, at backup time, to the Backup Components Document to indicate that when a file is to be 
		/// 	restored, it (the source file) should be remapped. The file may be restored to a new restore target and/or ranges of its data 
		/// 	restored to different locations with the restore target.
		/// </summary>
		/// <value>A read-only list containing the directed targets of this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		IList<VssDirectedTargetInfo> DirectedTargets { get; }

		/// <summary>
		/// 	The new file restoration locations for the selected component or component set. 
		/// </summary>
		/// <value>A read-only list contianing the new file restoration locations for the selected component or component set. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		IList<VssWMFileDescription> NewTargets { get; }

		/// <summary>
		///		Information about any partial files associated with this component.
		/// </summary>
		/// <value>A read-only list containing information about any partial files associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		IList<VssPartialFileInfo> PartialFiles { get; }

		/// <summary>
		/// 	Information about the file sets (specified file or files) to participate in an incremental or differential backup or restore as a 
		/// 	differenced file — that is, backup and restores associated with it are to be implemented as if entire files are copied to and from 
		/// 	backup media (as opposed to using partial files).
		/// </summary>
		/// <remarks><b>Windows XP:</b> This method requires Windows Server 2003 or later</remarks>
		/// <value>
		/// 	A read only list containing the diffrenced files associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note>
		/// </value>
		IList<VssDifferencedFileInfo> DifferencedFiles { get; }

		/// <summary>The subcomponents associated with this component.</summary>
		/// <value>A read only list containing the subcomponents associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		IList<VssRestoreSubcomponentInfo> RestoreSubcomponents { get; }
    }
}
