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

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003

#include <VsMgmt.h>

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   public ref class VssSnapshotManagement : IVssSnapshotManagement, MarshalByRefObject
   {
   public:
      ~VssSnapshotManagement();
      !VssSnapshotManagement();

      virtual IVssDifferentialSoftwareSnapshotManagement^ GetDifferentialSoftwareSnapshotManagementInterface();
      virtual Int64 GetMinDiffAreaSize();
   internal:
      VssSnapshotManagement();
   private:
      ::IVssSnapshotMgmt *m_snapshotMgmt;

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      DEFINE_EX_INTERFACE_ACCESSOR(IVssSnapshotMgmt2, m_snapshotMgmt)
#endif
   };

}
}}

#endif
