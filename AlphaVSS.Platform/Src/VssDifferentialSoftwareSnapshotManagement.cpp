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

#include "stdafx.h"

#include "VssDifferentialSoftwareSnapshotManagement.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WIN2003

   /****************************************************************************************
     Helper methods for creating properties objects and lists
    ****************************************************************************************/

   template <typename IF>
   static IF^ CreatePropertiesObject(VSS_MGMT_OBJECT_PROP &prop)
   {
      throw gcnew NotImplementedException(String::Format(L"Unknown type in VSS_MGMT_OBJECT_PROP: {0}", (Int32)prop.Type));
   }

   
   template <>
   static VssVolumeProperties^ CreatePropertiesObject<VssVolumeProperties>(VSS_MGMT_OBJECT_PROP &prop)
   {
      try
      {
         System::Diagnostics::Debug::Assert(prop.Type == VSS_MGMT_OBJECT_VOLUME, 
            L"Unexpected type in VSS_MGMT_OBJECT_PROP object",
            String::Format(L"Expected type to be VSS_MGMT_OBJECT_VOLUME ({0}), but it was {1}",
            (Int32)VSS_MGMT_OBJECT_VOLUME, (Int32)prop.Type));
         return gcnew VssVolumeProperties(gcnew String(prop.Obj.Vol.m_pwszVolumeName), gcnew String(prop.Obj.Vol.m_pwszVolumeDisplayName));
      }
      finally
      {
         ::CoTaskMemFree(prop.Obj.Vol.m_pwszVolumeName);
         ::CoTaskMemFree(prop.Obj.Vol.m_pwszVolumeDisplayName);
      }
   }

   
   template <>
   static VssDiffVolumeProperties^ CreatePropertiesObject<VssDiffVolumeProperties>(VSS_MGMT_OBJECT_PROP &prop)
   {
      try
      {
         System::Diagnostics::Debug::Assert(prop.Type == VSS_MGMT_OBJECT_DIFF_VOLUME, 
            L"Unexpected type in VSS_MGMT_OBJECT_PROP object",
            String::Format(L"Expected type to be VSS_MGMT_OBJECT_DIFF_VOLUME ({0}), but it was {1}",
            (Int32)VSS_MGMT_OBJECT_DIFF_VOLUME, (Int32)prop.Type));
         return gcnew VssDiffVolumeProperties(gcnew String(prop.Obj.DiffVol.m_pwszVolumeName), 
                                       gcnew String(prop.Obj.DiffVol.m_pwszVolumeDisplayName), 
                                       prop.Obj.DiffVol.m_llVolumeFreeSpace, 
                                       prop.Obj.DiffVol.m_llVolumeTotalSpace);
      }
      finally
      {
         ::CoTaskMemFree(prop.Obj.DiffVol.m_pwszVolumeName);
         ::CoTaskMemFree(prop.Obj.DiffVol.m_pwszVolumeDisplayName);
      }
   }

   
   template <>
   static VssDiffAreaProperties^ CreatePropertiesObject<VssDiffAreaProperties>(VSS_MGMT_OBJECT_PROP &prop)
   {
      try
      {
         System::Diagnostics::Debug::Assert(prop.Type == VSS_MGMT_OBJECT_DIFF_AREA, 
            L"Unexpected type in VSS_MGMT_OBJECT_PROP object",
            String::Format(L"Expected type to be VSS_MGMT_OBJECT_DIFF_AREA ({0}), but it was {1}",
            (Int32)VSS_MGMT_OBJECT_DIFF_AREA, (Int32)prop.Type));
         return gcnew VssDiffAreaProperties(gcnew String(prop.Obj.DiffArea.m_pwszVolumeName),
                                      gcnew String(prop.Obj.DiffArea.m_pwszDiffAreaVolumeName),
                                      prop.Obj.DiffArea.m_llMaximumDiffSpace,
                                      prop.Obj.DiffArea.m_llAllocatedDiffSpace,
                                      prop.Obj.DiffArea.m_llUsedDiffSpace);
      }
      finally
      {
         ::CoTaskMemFree(prop.Obj.DiffArea.m_pwszVolumeName);
         ::CoTaskMemFree(prop.Obj.DiffArea.m_pwszDiffAreaVolumeName);
      }
   }

   
   template <typename IF>
   static IList<IF^>^ CreateListFromEnumMgmtObject(IVssEnumMgmtObject *pEnum)
   {
      try
      {
         List<IF^>^ list = gcnew List<IF^>();

         while (true)
         {
            VSS_MGMT_OBJECT_PROP prop;
            ULONG celtFetched;
            HRESULT hr = pEnum->Next(1, &prop, &celtFetched);
            
            if (FAILED(hr))		// An error occured
               ThrowException(hr);
            
            if (celtFetched < 1) // We are done
               break;

            list->Add(CreatePropertiesObject<IF>(prop));
         }
         return list;
      }
      finally
      {
         pEnum->Release();
      }	
   }

   /****************************************************************************************
     Class members
    ****************************************************************************************/

   VssDifferentialSoftwareSnapshotManagement::~VssDifferentialSoftwareSnapshotManagement()
   {
      this->!VssDifferentialSoftwareSnapshotManagement();
   }

   VssDifferentialSoftwareSnapshotManagement::!VssDifferentialSoftwareSnapshotManagement()
   {
      if (m_mgmt != 0)
      {
         m_mgmt->Release();
         m_mgmt = 0;
      }

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      if (m_IVssDifferentialSoftwareSnapshotMgmt2 != 0)
      {
         m_IVssDifferentialSoftwareSnapshotMgmt2->Release();
         m_IVssDifferentialSoftwareSnapshotMgmt2 = 0;
      }
#endif

#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      if (m_IVssDifferentialSoftwareSnapshotMgmt3 != 0)
      {
         m_IVssDifferentialSoftwareSnapshotMgmt3->Release();
         m_IVssDifferentialSoftwareSnapshotMgmt3 = 0;
      }
#endif
   }

   VssDifferentialSoftwareSnapshotManagement::VssDifferentialSoftwareSnapshotManagement(::IVssDifferentialSoftwareSnapshotMgmt *pMgmt)
   {
      if (pMgmt == 0)
         throw gcnew ArgumentNullException(gcnew String(L"pMgmt"));
      m_mgmt = pMgmt;
   }
        
   void VssDifferentialSoftwareSnapshotManagement::AddDiffArea(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace)
   {
      CheckCom(m_mgmt->AddDiffArea(NoNullAutoMStr(volumeName), NoNullAutoMStr(diffAreaVolumeName), maximumDiffSpace));
   }

    void VssDifferentialSoftwareSnapshotManagement::ChangeDiffAreaMaximumSize(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace)
   {
      CheckCom(m_mgmt->ChangeDiffAreaMaximumSize(NoNullAutoMStr(volumeName), NoNullAutoMStr(diffAreaVolumeName), maximumDiffSpace));
   }

    IList<VssDiffAreaProperties^>^ VssDifferentialSoftwareSnapshotManagement::QueryDiffAreasForSnapshot(Guid snapshotId)
   {
      IVssEnumMgmtObject *pEnum;
      CheckCom(m_mgmt->QueryDiffAreasForSnapshot(ToVssId(snapshotId), &pEnum));
      return CreateListFromEnumMgmtObject<VssDiffAreaProperties>(pEnum);
   }

    IList<VssDiffAreaProperties^>^ VssDifferentialSoftwareSnapshotManagement::QueryDiffAreasForVolume(String^ volumeName)
   {
      IVssEnumMgmtObject *pEnum;
      CheckCom(m_mgmt->QueryDiffAreasForVolume(NoNullAutoMStr(volumeName), &pEnum));
      return CreateListFromEnumMgmtObject<VssDiffAreaProperties>(pEnum);
   }

    IList<VssDiffAreaProperties^>^ VssDifferentialSoftwareSnapshotManagement::QueryDiffAreasOnVolume(String^ volumeName)
   {
      IVssEnumMgmtObject *pEnum;
      CheckCom(m_mgmt->QueryDiffAreasOnVolume(NoNullAutoMStr(volumeName), &pEnum));
      return CreateListFromEnumMgmtObject<VssDiffAreaProperties>(pEnum);
   }

    IList<VssDiffVolumeProperties^>^ VssDifferentialSoftwareSnapshotManagement::QueryVolumesSupportedForDiffAreas(String^ originalVolumeName)
   {
      IVssEnumMgmtObject *pEnum;
      CheckCom(m_mgmt->QueryVolumesSupportedForDiffAreas(NoNullAutoMStr(originalVolumeName), &pEnum));
      return CreateListFromEnumMgmtObject<VssDiffVolumeProperties>(pEnum);
   }




    //
    // From IVssDifferentialSoftwareSnapshotMgmt2
    //
    void VssDifferentialSoftwareSnapshotManagement::ChangeDiffAreaMaximumSize(String^ volumeName, String^ diffAreaVolumeName, Int64 maximumDiffSpace, bool isVolatile)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssDifferentialSoftwareSnapshotMgmt2()->ChangeDiffAreaMaximumSizeEx(NoNullAutoMStr(volumeName), NoNullAutoMStr(diffAreaVolumeName), maximumDiffSpace, isVolatile));
#else
      UnsupportedOs();
#endif
   }

    //
    // From IVssDifferentialSoftwareSnapshotMgmt3
    //
    void VssDifferentialSoftwareSnapshotManagement::ClearVolumeProtectFault(String^ volumeName)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssDifferentialSoftwareSnapshotMgmt3()->ClearVolumeProtectFault(NoNullAutoMStr(volumeName)));
#else
      UnsupportedOs();
#endif
   }

   void VssDifferentialSoftwareSnapshotManagement::DeleteUnusedDiffAreas(String^ diffAreaVolumeName)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssDifferentialSoftwareSnapshotMgmt3()->DeleteUnusedDiffAreas(NoNullAutoMStr(diffAreaVolumeName)));
#else
      UnsupportedOs();
#endif
   }

    VssVolumeProtectionInfo^ VssDifferentialSoftwareSnapshotManagement::GetVolumeProtectionLevel(String^ volumeName)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      VSS_VOLUME_PROTECTION_INFO info;
      CheckCom(RequireIVssDifferentialSoftwareSnapshotMgmt3()->GetVolumeProtectLevel(NoNullAutoMStr(volumeName), &info));
      return CreateVssVolumeProtectionInfo(&info);
#else
      UnsupportedOs();
#endif
   }

    void VssDifferentialSoftwareSnapshotManagement::SetVolumeProtectionLevel(String^ volumeName, VssProtectionLevel protectionLevel)
   {
#if ALPHAVSS_TARGET >= ALPHAVSS_TARGET_WINVISTAORLATER
      CheckCom(RequireIVssDifferentialSoftwareSnapshotMgmt3()->SetVolumeProtectLevel(NoNullAutoMStr(volumeName), (VSS_PROTECTION_LEVEL)protectionLevel));
#else
      UnsupportedOs();
#endif
   }
#endif
}
}}