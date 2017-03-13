
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