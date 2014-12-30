/* Copyright (c) 2008-2009 Peter Palotas
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

using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	private ref class VssWMComponent : IVssWMComponent, IDisposable, MarshalByRefObject
	{
	public:
		~VssWMComponent();
		!VssWMComponent();

		property VssComponentType Type { virtual VssComponentType get(); }
		property String^ LogicalPath { virtual String^ get(); }
		property String^ ComponentName { virtual String^ get(); }
		property String^ Caption { virtual String^ get(); }
		virtual array<byte>^ GetIcon();
		property bool RestoreMetadata { virtual bool get(); }
		property bool NotifyOnBackupComplete { virtual bool get(); }
		property bool Selectable { virtual bool get(); }
		property bool SelectableForRestore { virtual bool get(); }
		property VssComponentFlags ComponentFlags { virtual VssComponentFlags get(); }
		property IList<VssWMFileDescription^>^ Files { virtual IList<VssWMFileDescription^>^ get(); }
		property IList<VssWMFileDescription^>^ DatabaseFiles { virtual IList<VssWMFileDescription^>^ get(); }
		property IList<VssWMFileDescription^>^ DatabaseLogFiles { virtual IList<VssWMFileDescription^>^ get(); }
		property IList<VssWMDependency^>^ Dependencies { virtual IList<VssWMDependency^>^ get(); }
	internal:
		static VssWMComponent^ Adopt(::IVssWMComponent *component);
	private:
		VssWMComponent(::IVssWMComponent *component);
		::IVssWMComponent *mComponent;

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