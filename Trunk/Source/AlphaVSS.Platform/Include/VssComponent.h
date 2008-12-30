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

#include "VssListAdapter.h"

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
	private ref class VssComponent : IVssComponent
	{
	public:
		~VssComponent();
		!VssComponent();

		property bool AdditionalRestores { virtual bool get(); }
		property String^ BackupOptions { virtual String^ get(); }
		property String^ BackupStamp { virtual String^ get(); }
		property bool BackupSucceeded { virtual bool get(); }
		property String^ ComponentName { virtual String^ get(); }
		property VssComponentType ComponentType { virtual VssComponentType get(); }
		property VssFileRestoreStatus FileRestoreStatus { virtual VssFileRestoreStatus get(); }
		property String^ LogicalPath { virtual String^ get(); }
		property String^ PostRestoreFailureMsg { virtual String^ get(); }
		property String^ PreRestoreFailureMsg { virtual String^ get(); }
		property String^ PreviousBackupStamp { virtual String^ get(); }
		property String^ RestoreOptions { virtual String^ get(); }
		property VssRestoreTarget RestoreTarget { virtual VssRestoreTarget get(); }
		property bool IsSelectedForRestore { virtual bool get(); }
		property IList<VssWMFileDescription^>^ AlternateLocationMappings { virtual IList<VssWMFileDescription^>^ get(); }
		property IList<VssDirectedTargetInfo^>^ DirectedTargets { virtual IList<VssDirectedTargetInfo^>^ get(); }
		property IList<VssWMFileDescription^>^ NewTargets { virtual IList<VssWMFileDescription^>^ get(); }
		property IList<VssPartialFileInfo^>^ PartialFiles { virtual IList<VssPartialFileInfo^>^ get(); }
		property IList<VssDifferencedFileInfo^>^ DifferencedFiles { virtual IList<VssDifferencedFileInfo^>^ get(); }
		property IList<VssRestoreSubcomponentInfo^>^ RestoreSubcomponents { virtual IList<VssRestoreSubcomponentInfo^>^ get(); }
	internal:
		static VssComponent^ Adopt(::IVssComponent *vssWriterComponents);
	private:
		VssComponent(::IVssComponent *vssWriterComponents);
		::IVssComponent *mVssComponent;

		ref class DirectedTargetList sealed : VssListAdapter<VssDirectedTargetInfo^>
		{
		public:
			DirectedTargetList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssDirectedTargetInfo^ default[int] { virtual VssDirectedTargetInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class NewTargetList sealed : VssListAdapter<VssWMFileDescription^>
		{
		public:
			NewTargetList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssWMFileDescription^ default[int] { virtual VssWMFileDescription^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class AlternateLocationMappingList sealed : VssListAdapter<VssWMFileDescription^>
		{
		public:
			AlternateLocationMappingList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssWMFileDescription^ default[int] { virtual VssWMFileDescription^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

		ref class PartialFileList sealed : VssListAdapter<VssPartialFileInfo^>
		{
		public:
			PartialFileList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssPartialFileInfo^ default[int] { virtual VssPartialFileInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
		ref class DifferencedFileList sealed : VssListAdapter<VssDifferencedFileInfo^>
		{
		public:
			DifferencedFileList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssDifferencedFileInfo^ default[int] { virtual VssDifferencedFileInfo^ get(int index) override; }
		private:
			VssComponent^ mComponent;
		};
#endif

		ref class RestoreSubcomponentList sealed : VssListAdapter<VssRestoreSubcomponentInfo^>
		{
		public:
			RestoreSubcomponentList(VssComponent^ component);

			property int Count { virtual int get() override; }
			property VssRestoreSubcomponentInfo^ default[int] { virtual VssRestoreSubcomponentInfo^ get(int index) override; }
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