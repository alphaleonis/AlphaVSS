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
#include "Macros.h"
#include "Error.h"

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
#include <VsMgmt.h>
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssWMFileDescriptor^ CreateVssWMFileDescriptor(IVssWMFiledesc *vssWMFiledesc);
   VssProviderProperties^ CreateVssProviderProperties(VSS_PROVIDER_PROP *pProp);
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
   VssWMDependency^ CreateVssWMDependency(IVssWMDependency *dependency);
#endif	
   VssSnapshotProperties^ CreateVssSnapshotProperties(VSS_SNAPSHOT_PROP *prop);

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
   VssVolumeProtectionInfo^ CreateVssVolumeProtectionInfo(VSS_VOLUME_PROTECTION_INFO *info);
#endif
}
}}