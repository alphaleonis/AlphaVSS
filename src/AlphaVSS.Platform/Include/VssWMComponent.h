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
      property IList<VssWMFileDescriptor^>^ Files { virtual IList<VssWMFileDescriptor^>^ get(); }
      property IList<VssWMFileDescriptor^>^ DatabaseFiles { virtual IList<VssWMFileDescriptor^>^ get(); }
      property IList<VssWMFileDescriptor^>^ DatabaseLogFiles { virtual IList<VssWMFileDescriptor^>^ get(); }
      property IList<VssWMDependency^>^ Dependencies { virtual IList<VssWMDependency^>^ get(); }
   internal:
      static VssWMComponent^ Adopt(::IVssWMComponent *component);
   private:
      VssWMComponent(::IVssWMComponent *component);
      ::IVssWMComponent *m_component;

      VssComponentType m_type;
      String^ m_logicalPath;
      String^ m_componentName;
      String^ m_caption;
      array<byte>^ m_icon;
      bool m_restoreMetadata;
      bool m_notifyOnBackupComplete;
      bool m_selectable;
      bool m_selectableForRestore;
      VssComponentFlags m_componentFlags;

      UInt32 m_fileCount;
      UInt32 m_databaseFileCount;
      UInt32 m_databaseLogFileCount;
      UInt32 m_dependencyCount;

      IList<VssWMFileDescriptor^>^ m_files;
      IList<VssWMFileDescriptor^>^ m_databaseFiles;
      IList<VssWMFileDescriptor^>^ m_databaseLogFiles;
      IList<VssWMDependency^>^ m_dependencies;
   };
}
} }