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

#include <vss.h>

#include "VssWMFiledesc.h"
#include "VssComponentType.h"
#include "VssFileRestoreStatus.h"
#include "VssWMFiledesc.h"
#include "VssRestoreTarget.h"
#include "DirectedTargetInfo.h"
#include "VssListAdapter.h"
#include "PartialFileInfo.h"
#include "DifferencedFileInfo.h"
#include "RestoreSubcomponentInfo.h"

using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	<para>
	/// 		Class containing methods for examining and modifying information about components contained in a requester's Backup Components Document.
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
	public ref class VssComponent : IDisposable
	{
	public:
		/// <summary>Releases any resources aquired by this instance</summary>
		~VssComponent();

		/// <summary>Releases any resources aquired by this instance</summary>
		!VssComponent();

		#if 0
		// These methods are specific to writers, only supporting requesters at this time, so these are not included.
		void AddDifferencedFilesByLastModifyTime(String^ path, String^ fileSpec, bool recursive, DateTime lastModifyTime);
		void AddDifferencedFilesByLastModifyTime(DifferencedFileInfo^ differencedFile);

		void AddDirectedTarget(DirectedTargetInfo ^directedTarget);
		void AddDirectedTarget(String ^ sourcePath, String^ sourceFileName, String^ sourceRangeList, String^ destinationPath, String^ destinationFileName, String^ destinationRangeList);

		void AddParitalFile(PartialFileInfo^ partialFile);
		void AddPartialFile(String^ path, String^ filename, String^ ranges, String^ metaData);

		property String^ RestoreMetadata { String^ get(); }
		property String^ BackupMetadata { String^ get(); }
		
		void SetBackupMetadata(String^ metadata);
		void SetBackupStamp(String^ metadata);
		void SetPostRestoreFailureMsg(String^ metadata);
		void SetPreRestoreFailureMsg(String^ metadata);
		void SetRestoreMetadata(String^ metadata);
		void SetRestoreTarget(VssRestoreTarget target);

#endif

		/// <summary>
		/// 	The <see cref="AdditionalRestores" /> property is used by a writer during incremental or differential restore 
		/// 	operations to determine whether a given component will require additional restore operations to completely retrieve it, 
		/// 	but can also be called by a requester.
		/// </summary>
		/// <value>
		///		If <see langword="true"/>, additional restores will occur for the 
		///		current component. If <see langword="false"/>, additional restores will not occur.
		/// </value>
		property bool AdditionalRestores { bool get(); }

		/// <summary>
		/// 	<para>
		/// 		The backup options specified to the writer that manages the currently selected component or component set 
		/// 		by a requester using <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.SetBackupOptions(System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String,System.String)"/>.
		/// 	</para>
		/// </summary>
		/// <value>The backup options for the current writer.</value>
		property String^ BackupOptions { String^ get(); }
		
		/// <summary>The backup stamp string stored by a writer for a given component.</summary>
		/// <value>The backup stamp indicating the time at which the component was backed up.</value>
		property String^ BackupStamp { String^ get(); }

		/// <summary>
		/// 	The status of a complete attempt at backing up all the files of a selected component or component set as a 
		/// 	<see cref="VssFileRestoreStatus"/> enumeration.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if the backup was successful and <see langword="false"/> if it was not.
		/// </value>
		property bool BackupSucceeded { bool get(); }

		/// <summary>The logical name of this component.</summary>
		/// <value>The logical name of this component.</value>
		property String^ ComponentName { String^ get(); }

		/// <summary>The type of this component in terms of the <see cref="ComponentType"/> enumeration.</summary>
		/// <value>The type of this component.</value>
		property VssComponentType ComponentType { VssComponentType get(); }

		/// <summary>
		/// 	The status of a completed attempt to restore all the files of a selected component or component set 
		/// 	as a <see cref="VssFileRestoreStatus" /> enumeration.
		/// </summary>
		/// <value>
		/// 	A value of the <see cref="VssFileRestoreStatus" /> enumeration that specifies whether all files were successfully restored.
		/// </value>		
		property VssFileRestoreStatus FileRestoreStatus { VssFileRestoreStatus get(); }

		/// <summary>The logical path of this component.</summary>
		/// <value>The logical path of this component.</value>
		property String^ LogicalPath { String^ get(); }

		/// <summary>The failure message generated by a writer while handling the <c>PostRestore</c> event if one was set.</summary>
		/// <value>The failure message that describes an error that occurred while processing the <c>PostRestore</c> event.</value>
		property String^ PostRestoreFailureMsg { String^ get(); }

		/// <summary>The failure message generated by a writer while handling the <c>PreRestore</c> event if one was set.</summary>
		/// <value>The failure message that describes an error that occurred while processing the <c>PreRestore</c> event.</value>
		property String^ PreRestoreFailureMsg { String^ get(); }

		/// <summary>
		/// 	A previous backup stamp loaded by a requester in the Backup Components Document. The value is used by a writer when 
		/// 	deciding if files should participate in differential or incremental backup operation.
		/// </summary>
		/// <value>
		/// 	The time stamp of a previous backup so that a differential or incremental backup can be correctly implemented.
		/// </value>
		property String^ PreviousBackupStamp { String^ get(); }

		/// <summary>The restore options specified to the current writer by a requester using 
		/// <see dref="M:Alphaleonis.Win32.Vss.VssBackupComponents.SetRestoreOptions(System.Guid,Alphaleonis.Win32.Vss.VssComponentType,System.String,System.String,System.String)"/>.</summary>
		/// <value>The restore options of the writer.</value>
		property String^ RestoreOptions { String^ get(); }

		/// <summary>The restore target (in terms of the <see cref="VssRestoreTarget"/> enumeration) for the current component. Can only be called during a restore operation.</summary>
		/// <value>A value from the <see cref="VssRestoreTarget"/> enumeration containing the restore target information.</value>
		property VssRestoreTarget RestoreTarget { VssRestoreTarget get(); }

		/// <summary>Determines whether the current component has been selected to be restored.</summary>
		/// <value>If the returned value of this parameter is <see langword="true"/>, the component has been selected to be restored. If <see langword="false"/>, it has not been selected.</value>
		property bool IsSelectedForRestore { bool get(); }
		
		/// <summary>A collection of mapping information for the file set's alternate location for file restoration.</summary>
		/// <value>A read-only list containing the alternate location to which files were actually restored. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		/// <remarks>See <see href="http://msdn.microsoft.com/en-us/library/aa383473(VS.85).aspx">the MSDN documentation on the IVssComponent::GetAlternateLocationMapping method</see> for more information.</remarks>
		property IVssListAdapter<VssWMFiledesc^>^ AlternateLocationMappings { IVssListAdapter<VssWMFiledesc^>^ get(); }


		/// <summary>
		/// 	Information stored by a writer, at backup time, to the Backup Components Document to indicate that when a file is to be 
		/// 	restored, it (the source file) should be remapped. The file may be restored to a new restore target and/or ranges of its data 
		/// 	restored to different locations with the restore target.
		/// </summary>
		/// <value>A read-only list containing the directed targets of this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		property IVssListAdapter<DirectedTargetInfo^>^ DirectedTargets { IVssListAdapter<DirectedTargetInfo^>^ get(); }

		/// <summary>
		/// 	The new file restoration locations for the selected component or component set. 
		/// </summary>
		/// <value>A read-only list contianing the new file restoration locations for the selected component or component set. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		property IVssListAdapter<VssWMFiledesc^>^ NewTargets { IVssListAdapter<VssWMFiledesc^>^ get(); }

		/// <summary>
		///		Information about any partial files associated with this component.
		/// </summary>
		/// <value>A read-only list containing information about any partial files associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		property IVssListAdapter<PartialFileInfo^>^ PartialFiles { IVssListAdapter<PartialFileInfo^>^ get(); }

		/// <summary>
		/// 	Information about the file sets (specified file or files) to participate in an incremental or differential backup or restore as a 
		/// 	differenced file — that is, backup and restores associated with it are to be implemented as if entire files are copied to and from 
		/// 	backup media (as opposed to using partial files).
		/// </summary>
		/// <remarks><b>Windows XP:</b> This method requires Windows Server 2003 or later</remarks>
		/// <value>
		/// 	A read only list containing the diffrenced files associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note>
		/// </value>
		property IVssListAdapter<DifferencedFileInfo^>^ DifferencedFiles { IVssListAdapter<DifferencedFileInfo^>^ get(); }

		/// <summary>The subcomponents associated with this component.</summary>
		/// <value>A read only list containing the subcomponents associated with this component. <note type="caution">This list must not be accessed after the <see cref="VssComponent"/> from which it was obtained has been disposed.</note></value>
		property IVssListAdapter<RestoreSubcomponentInfo^>^ RestoreSubcomponents { IVssListAdapter<RestoreSubcomponentInfo^>^ get(); }
	internal:
		static VssComponent^ Adopt(IVssComponent *vssWriterComponents);
	private:
		VssComponent(IVssComponent *vssWriterComponents);
		IVssComponent *mVssComponent;

		ref class DirectedTargetList sealed : VssListAdapter<DirectedTargetInfo^>
		{
		public:
			DirectedTargetList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property DirectedTargetInfo^ default[int] { virtual DirectedTargetInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class NewTargetList sealed : VssListAdapter<VssWMFiledesc^>
		{
		public:
			NewTargetList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssWMFiledesc^ default[int] { virtual VssWMFiledesc^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class AlternateLocationMappingList sealed : VssListAdapter<VssWMFiledesc^>
		{
		public:
			AlternateLocationMappingList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssWMFiledesc^ default[int] { virtual VssWMFiledesc^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class PartialFileList sealed : VssListAdapter<PartialFileInfo^>
		{
		public:
			PartialFileList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property PartialFileInfo^ default[int] { virtual PartialFileInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		ref class DifferencedFileList sealed : VssListAdapter<DifferencedFileInfo^>
		{
		public:
			DifferencedFileList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property DifferencedFileInfo^ default[int] { virtual DifferencedFileInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};
#endif

		ref class RestoreSubcomponentList sealed : VssListAdapter<RestoreSubcomponentInfo^>
		{
		public:
			RestoreSubcomponentList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property RestoreSubcomponentInfo^ default[int] { virtual RestoreSubcomponentInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		AlternateLocationMappingList^ mAlternateLocationMappings;
		DirectedTargetList^ mDirectedTargets;
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		DifferencedFileList^ mDifferencedFiles;
#endif
		RestoreSubcomponentList^ mRestoreSubcomponents;
		PartialFileList^ mPartialFiles;
		NewTargetList^ mNewTargets;
	};
}
} }