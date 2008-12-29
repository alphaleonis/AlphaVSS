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
#include "VssWMFileDescription.h"
#include "VssWMDependency.h"

using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// <see cref="VssWMComponent"/> is a class that allows access to component information stored in a Writer Metadata Document.
	/// Instances of <see cref="VssWMComponent"/> are obtained by enumerating <cee dref="VssExamineWriterMetadata::Components"/>.
	/// </summary>
	public ref class VssWMComponent : IDisposable
	{
	public:
		/// <summary>Releases any resources aquired by this instance</summary>
		~VssWMComponent();
		/// <summary>Releases any resources aquired by this instance</summary>
		!VssWMComponent();

		/// <summary>
		/// The component type.
		/// </summary>
		property VssComponentType Type { VssComponentType get(); }
		/// <summary>
		/// The logical path of the component.
		/// </summary>
		/// <value>A string containing the logical path of the component, which may be <see langword="null"/>.
		/// <remarks>There are no restrictions on the characters that can appear in a non-NULL logical path.</remarks></value>
		property String^ LogicalPath { String^ get(); }
		/// <summary>
		/// The name of the component. A component name string cannot be <see langword="null"/>.
		/// </summary>
		property String^ ComponentName { String^ get(); }
		/// <summary>
		/// The description of the component. A caption string can be <see langword="null" />.
		/// </summary>
		property String^ Caption { String^ get(); }
		/// <summary>
		/// A buffer containing the binary data for a displayable icon representing the component. 
		/// </summary>
		/// <remarks>The buffer contents should use the same format as the standard icon (.ico) files. If the writer that created 
		/// the component did not choose to specify an icon, the value will be <see langword="null"/>.</remarks>
		property array<byte>^ Icon { array<byte>^ get(); }
		/// <summary>
		/// Boolean that indicates whether there is private metadata associated with the restoration of the component.
		/// </summary>
		/// <value>The Boolean is <see langword="true"/> if there is private metadata associated with the restoration
		/// of the components, and <see langword="false"/> if there is not. </value>
		property bool RestoreMetadata { bool get(); }
		/// <summary>Reserved for future use.</summary>
		property bool NotifyOnBackupComplete { bool get(); }
		/// <summary>Boolean that indicates (for component mode operations) if the component is selectable for backup.</summary>
		/// <remarks>The value of <see cref="Selectable"/> helps determine whether a requester has the option of including or excluding 
		/// a given component in backup operations. </remarks>
		/// <value><see langword="true"/> if the component is selectable for backup and <see langword="false"/> if it is not. </value>
		/// <seealso href="http://msdn.microsoft.com/en-us/library/aa384680(VS.85).aspx">VSS_COMPONENTINFO structure on MSDN</seealso>
		property bool Selectable { bool get(); }

		/// <summary>
		/// Boolean that indicates (for component-mode operations) whether the component is selectable for restore.
		/// </summary>
		/// <remarks><see cref="SelectableForRestore"/> allows the requester to determine whether this component can be individually selected 
		/// for restore if it had earlier been implicitly included in the backup.
		/// <note><b>Windows XP:</b> This property requires Windows Server 2003 or later</note></remarks>
		/// <value>The Boolean is <see langword="true"/> if the component is selectable for restore and <see langword="false"/> if it is not.</value>
		property bool SelectableForRestore { bool get(); }

		/// <summary><para>A bit mask (or bitwise OR) of values of the <see cref="VssComponentFlags"/> enumeration, indicating the 
		/// features this component supports.</para>
		/// <para><b>Windows Server 2003 and Windows XP:</b>  Before Windows Server 2003 SP1, this member is reserved for system use.</para>
		/// </summary>
		CA_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId="Flags")
		property VssComponentFlags ComponentFlags { VssComponentFlags get(); }

		/// <summary>
		/// The file descriptors associated with this component.
		/// </summary>
		/// <remarks>This collection represents the method <c>GetFile()</c> of <c>IVssWMComponent</c> in the VSS API</remarks>
		property IList<VssWMFileDescription^>^ Files { IList<VssWMFileDescription^>^ get(); }

		/// <summary>
		/// A list of <see cref="VssWMFileDescription"/> instances containing information about the database backup component files.
		/// </summary>
		property IList<VssWMFileDescription^>^ DatabaseFiles { IList<VssWMFileDescription^>^ get(); }
		
		/// <summary>
		/// A list of file descriptors for the log files associated with the specified database backup component.
		/// </summary>
		property IList<VssWMFileDescription^>^ DatabaseLogFiles { IList<VssWMFileDescription^>^ get(); }

		/// <summary>
		/// A list of  <see cref="VssWMDependency"/> instances containing accessors for obtaining information about explicit writer-component 
		/// dependencies of one of the current components.
		/// </summary>
		property IList<VssWMDependency^>^ Dependencies { IList<VssWMDependency^>^ get(); }


	internal:
		static VssWMComponent^ Adopt(IVssWMComponent *component);
	private:
		VssWMComponent(IVssWMComponent *component);
		IVssWMComponent *mComponent;

		VssComponentType mType;
		String^ mLogicalPath;
		String^ mComponentName;
		String^ mCaption;
		array<byte>^ mIcon;
		bool mRestoreMetadata;
		bool mNotifyOnBackupComplete;
		bool mSelectable;
		bool mSelectableForRestore;
		VssComponentFlags mComponentFlags;

		UInt32 mFileCount;
		UInt32 mDatabaseFileCount;
		UInt32 mDatabaseLogFileCount;
		UInt32 mDependencyCount;

		IList<VssWMFileDescription^>^ mFiles;
		IList<VssWMFileDescription^>^ mDatabaseFiles;
		IList<VssWMFileDescription^>^ mDatabaseLogFiles;
		IList<VssWMDependency^>^ mDependencies;
	};
}
} }