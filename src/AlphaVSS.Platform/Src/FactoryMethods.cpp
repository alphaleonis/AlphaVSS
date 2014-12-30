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
#include "StdAfx.h"

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
#include <VsMgmt.h>
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssWMFileDescriptor^ CreateVssWMFileDescriptor(IVssWMFiledesc *vssWMFiledesc)
   {
      try
      {
         AutoBStr bstrAlternateLocation;
         CheckCom(vssWMFiledesc->GetAlternateLocation(&bstrAlternateLocation));

         DWORD dwTypeMask = 0;
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
         CheckCom(vssWMFiledesc->GetBackupTypeMask(&dwTypeMask));
#endif
         AutoBStr bstrFilespec;
         CheckCom(vssWMFiledesc->GetFilespec(&bstrFilespec));
         AutoBStr bstrPath;
         CheckCom(vssWMFiledesc->GetPath(&bstrPath));
         bool bRecursive;
         CheckCom(vssWMFiledesc->GetRecursive(&bRecursive));

         return gcnew VssWMFileDescriptor(bstrAlternateLocation, (VssFileSpecificationBackupType)dwTypeMask, bstrFilespec, bstrPath, bRecursive);
      }
      finally
      {
         vssWMFiledesc->Release();
      }

   }

   VssProviderProperties^ CreateVssProviderProperties(VSS_PROVIDER_PROP *pProp)
   {
      try
      {
         return gcnew VssProviderProperties(
            ToGuid(pProp->m_ProviderId),
            gcnew String(pProp->m_pwszProviderName),
            (VssProviderType)pProp->m_eProviderType,
            gcnew String(pProp->m_pwszProviderVersion),
            ToGuid(pProp->m_ProviderVersionId),
            ToGuid(pProp->m_ClassId));

      }
      finally
      {
         ::CoTaskMemFree(pProp->m_pwszProviderName);
         ::CoTaskMemFree(pProp->m_pwszProviderVersion);
      }
   }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003
   VssWMDependency^ CreateVssWMDependency(IVssWMDependency *dependency)		
   {
      try
      {
         VSS_ID id;
         CheckCom(dependency->GetWriterId(&id));

         AutoBStr logicalPath;
         CheckCom(dependency->GetLogicalPath(&logicalPath));

         AutoBStr componentName;
         CheckCom(dependency->GetComponentName(&componentName));

         return gcnew VssWMDependency(ToGuid(id), logicalPath, componentName);
      }
      finally 
      {
         dependency->Release();
      }
   }
#endif

   VssSnapshotProperties^ CreateVssSnapshotProperties(VSS_SNAPSHOT_PROP *prop)
   {
      try
      {
         return gcnew VssSnapshotProperties(
            ToGuid(prop->m_SnapshotId),
            ToGuid(prop->m_SnapshotSetId),
            prop->m_lSnapshotsCount,
            gcnew String(prop->m_pwszSnapshotDeviceObject),
            gcnew String(prop->m_pwszOriginalVolumeName),
            gcnew String(prop->m_pwszOriginatingMachine),
            gcnew String(prop->m_pwszServiceMachine),
            gcnew String(prop->m_pwszExposedName),
            gcnew String(prop->m_pwszExposedPath),
            ToGuid(prop->m_ProviderId),
            (VssVolumeSnapshotAttributes)prop->m_lSnapshotAttributes,
            ToDateTime(prop->m_tsCreationTimestamp),
            (VssSnapshotState)prop->m_eStatus
            );
      }
      finally
      {
         ::VssFreeSnapshotProperties(prop);
      }
   }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
   VssVolumeProtectionInfo^ CreateVssVolumeProtectionInfo(VSS_VOLUME_PROTECTION_INFO *info)
   {
      return gcnew VssVolumeProtectionInfo((VssProtectionLevel)info->m_protectionLevel, 
         info->m_volumeIsOfflineForProtection != 0,
         (VssProtectionFault)info->m_protectionFault,
         info->m_failureStatus,
         info->m_volumeHasUnusedDiffArea != 0);
   }
#endif
}
}}