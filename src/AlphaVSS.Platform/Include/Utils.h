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

using namespace System;


namespace Alphaleonis { namespace Win32 { namespace Vss
{
   // 
   // GUID conversion functions
   //

   // Convert from VSS_ID to System::Guid
   inline System::Guid ToGuid(const VSS_ID &guid )
   {
      return Guid( guid.Data1, guid.Data2, guid.Data3, 
         guid.Data4[ 0 ], guid.Data4[ 1 ], 
         guid.Data4[ 2 ], guid.Data4[ 3 ], 
         guid.Data4[ 4 ], guid.Data4[ 5 ], 
         guid.Data4[ 6 ], guid.Data4[ 7 ] );
   }

   // Convert from System::Guid to VSS_ID
   inline VSS_ID ToVssId( System::Guid guid ) 
   {
      array<Byte>^ guidData = guid.ToByteArray();
      pin_ptr<Byte> data = &(guidData[ 0 ]);

      return *(_GUID *)data;
   }



   //
   // DateTime conversions
   //

   // Convert from FILETIME to System::DateTime
   inline System::DateTime ToDateTime(FILETIME &ft)
   {
      UInt64 t = (UInt64)( (((UInt64)ft.dwHighDateTime) << 32) | ((UInt64)ft.dwLowDateTime) );
      return DateTime::FromFileTime(t);
   }

   // Convert from VSS_TIMESTAMP to System::DateTime
   inline System::DateTime ToDateTime(const VSS_TIMESTAMP &ft)
   {
      return DateTime::FromFileTime(ft);

   }

   // Convert from System::DateTime to FILETIME
   inline FILETIME ToFileTime(System::DateTime %time)
   {
      __int64 value = time.ToFileTime();
      FILETIME ft;
      ft.dwHighDateTime = (DWORD)(value >> 32);
      ft.dwLowDateTime = (DWORD)(value & 0xFFFFFFFF);
      return ft;		
   }



   //
   // Simple string conversion functions (unmanaged to managed)
   //

   // Convert from BSTR to System::String^
   inline System::String^ FromBStr(BSTR str)
   {
      return (str == 0) ? nullptr : System::Runtime::InteropServices::Marshal::PtrToStringBSTR(static_cast<IntPtr>(str)); 
   }


   // 
   // Classes handling the freeing of memory from various types of strings.
   // Used together with AutoPtr.
   //

   struct SysFreeStringDeleter
   {
      void operator()(BSTR s) { ::SysFreeString(s); }
   };

   struct MarshalFreeBSTRDeleter
   {
      void operator()(BSTR s) { System::Runtime::InteropServices::Marshal::FreeBSTR((IntPtr)s); }
   };

   struct CoTaskMemFreeDeleter
   {
      void operator()(void *s) { ::CoTaskMemFree(s); }
   };

   struct MarshalFreeHGlobalDeleter
   {
      void operator()(void *s) { System::Runtime::InteropServices::Marshal::FreeHGlobal((IntPtr)s); }
   };


   // 
   // Class for managing resources that need to be cleaned up.
   // With template parameter T specifying the pointer type to 
   // contain, and D specifying the Deleter to use for freeing
   // the resource as the object goes out of scope.
   //
   template <typename T, typename D>
   class AutoPtr
   {
   public:
      typedef T PtrT;
      typedef D DeleterT;

      AutoPtr() : mPtr(0) { }
      AutoPtr(T ptr) : mPtr(ptr) { }
      virtual ~AutoPtr() { if (mPtr != 0) { D()(mPtr); } }
      operator T() { return mPtr; }
      T *operator&() { return &mPtr; }
   protected:
      T ptr() { return mPtr; }
   private:
      T mPtr;
   };

   // 
   // Helper class for management of BSTR strings allocated by the system.
   //
   class AutoBStr : public AutoPtr<BSTR, SysFreeStringDeleter>
   {
   public:
      AutoBStr() : AutoPtr() {}
      AutoBStr(BSTR str) : AutoPtr(str) { }
      operator String^() { return (ptr() == 0) ? nullptr : System::Runtime::InteropServices::Marshal::PtrToStringBSTR(static_cast<IntPtr>(ptr())); }
   };

   // 
   // Helper class for management of BSTR strings originating as System::String
   //
   class AutoMBStr : public AutoPtr<BSTR, MarshalFreeBSTRDeleter>
   {
   public:
      AutoMBStr() : AutoPtr() { }
      AutoMBStr(System::String^ str) : AutoPtr((BSTR)System::Runtime::InteropServices::Marshal::StringToBSTR(str).ToPointer()) { }
   };

   // 
   // Helper class for management of VSS_PWSZ strings, allocated by the system.
   //
   class AutoPwsz : public AutoPtr<VSS_PWSZ, CoTaskMemFreeDeleter>
   {
   public:
      AutoPwsz() : AutoPtr() {}
      operator String ^() { return ptr() == 0 ? nullptr : gcnew String(ptr()); }
   };


   // 
   // Helper class for management of wchar_t* strings, originating as managed strings (System::String)
   //
   struct AutoMStr : public AutoPtr<wchar_t *, MarshalFreeHGlobalDeleter>
   {
      AutoMStr(String^ str) : AutoPtr( str == nullptr ? 0 : (wchar_t *)System::Runtime::InteropServices::Marshal::StringToHGlobalUni(str).ToPointer()) { }
      operator VSS_PWSZ(){ return (VSS_PWSZ)ptr(); }
   };

   // 
   // Helper class for forbidding null-pointers to be stored in the AutoPtr classes.
   //
   template <typename T>
   struct NoNull : public T
   {
      NoNull(String^ str) : T(str)
      {
         if (str == nullptr)
            throw gcnew ArgumentNullException();
      }

      NoNull(String^ str, const wchar_t *name) : T(str)
      {
         if (str == nullptr)
            throw gcnew ArgumentNullException(gcnew String(name));
      }
   };


   //
   // Helper class for managing an array of VSS_ID objects, originating from
   // a managed array of Guid objects.
   //
   class VssIds
   {
   public:
      VssIds(array<System::Guid> ^ guids)
         : m_ids(0)
      {
         if (guids == nullptr)
            throw gcnew ArgumentNullException();

         m_ids = new VSS_ID[guids->Length];

         for (int i = 0; i < guids->Length; i++)
         {
            m_ids[i] = ToVssId(guids[i]);
         }
      }

      ~VssIds()
      {
         delete [] m_ids;
      }

      (operator VSS_ID *)()
      {
         return m_ids;
      }

   private:
      VSS_ID *m_ids;
   };


}
} }