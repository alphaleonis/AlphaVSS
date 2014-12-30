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

#include "Config.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   /// <summary>
   ///		Represents a platform specific implementation of the global methods in the VSS API.
   /// </summary>
   /// <remarks>
   ///		An instance of this class can either be created directly, or by using the factory methods
   ///		in <see cref="VssUtils"/> to obtain an instance of the <see cref="IVssImplementation"/> 
   ///     interface for the current platform.
   /// </remarks>
   public ref class VssImplementation : IVssImplementation, MarshalByRefObject 
   {
   public:
      VssImplementation();
      virtual IVssBackupComponents^ CreateVssBackupComponents();
      virtual bool IsVolumeSnapshotted(String^ volumeName);
      virtual VssSnapshotCompatibility GetSnapshotCompatibility(String^ volumeName);
      virtual bool ShouldBlockRevert(String^ volumeName);
      [System::Security::Permissions::SecurityPermission(System::Security::Permissions::SecurityAction::LinkDemand)]
      virtual IVssExamineWriterMetadata^ CreateVssExamineWriterMetadata(String^ xml);
      virtual IVssSnapshotManagement^ GetSnapshotManagementInterface();
   };

}
}}