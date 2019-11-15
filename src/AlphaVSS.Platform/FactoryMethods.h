
#pragma once

#include "Macros.h"
#include "Error.h"

#include <VsMgmt.h>

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssWMFileDescriptor^ CreateVssWMFileDescriptor(IVssWMFiledesc *vssWMFiledesc);
   VssProviderProperties^ CreateVssProviderProperties(VSS_PROVIDER_PROP *pProp);
   VssWMDependency^ CreateVssWMDependency(IVssWMDependency *dependency);
   VssSnapshotProperties^ CreateVssSnapshotProperties(VSS_SNAPSHOT_PROP *prop);
   VssVolumeProtectionInfo^ CreateVssVolumeProtectionInfo(VSS_VOLUME_PROTECTION_INFO *info);
}
}}