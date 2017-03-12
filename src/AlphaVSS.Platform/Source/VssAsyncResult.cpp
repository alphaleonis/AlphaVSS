

#include <StdAfx.h>
#include "VssAsyncResult.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
   VssAsyncResult::VssAsyncResult(::IVssAsync *vssAsync, AsyncCallback^ userCallback, Object^ asyncState)
      : m_isComplete(0), m_asyncCallback(userCallback), m_asyncState(asyncState), m_asyncWaitHandle(nullptr), m_vssAsync(vssAsync), m_exception(nullptr)
   {
      if (!ThreadPool::QueueUserWorkItem(gcnew WaitCallback(this, &VssAsyncResult::WaitForAsyncCompletion)))
      {
         throw gcnew Exception(L"ThreadPool::QueueUserWorkItem failed.");
      }
   }

   void VssAsyncResult::WaitForAsyncCompletion(Object^ state)
   {
      HRESULT hrResult;
      
      hrResult = m_vssAsync->Wait();

      int prevState = Interlocked::Exchange(m_isComplete, -1);

      if (prevState != 0)
         throw gcnew InvalidOperationException("WaitForAsyncCompletion can only be called once.");

      if (SUCCEEDED(hrResult))
      {
         HRESULT hr = m_vssAsync->QueryStatus(&hrResult, NULL);
         if (FAILED(hr))
            hrResult = hr;
      }

      if (FAILED(hrResult))
         m_exception = GetExceptionForHr(hrResult);
      else if (hrResult == VSS_S_ASYNC_CANCELLED)
         m_exception = gcnew OperationCanceledException();

      if (m_asyncWaitHandle != nullptr)
         m_asyncWaitHandle->Set();

      if (m_asyncCallback != nullptr)
         m_asyncCallback(this);
   }

   void VssAsyncResult::EndInvoke()
   {
         // This method assumes that only 1 thread calls EndInvoke 
         // for this object
         if (!IsCompleted)
         {
            AsyncWaitHandle->WaitOne();
            AsyncWaitHandle->Close();
            m_asyncWaitHandle = nullptr;
         }

         if (m_exception != nullptr)
            throw m_exception;

   }
   
   Object^ VssAsyncResult::AsyncState::get()
   {
      return m_asyncState;
   }

   bool VssAsyncResult::CompletedSynchronously::get()
   {
      return false;
   }

   WaitHandle^ VssAsyncResult::AsyncWaitHandle::get()   
   {
      if (m_asyncWaitHandle == nullptr)
      {
         bool done = IsCompleted;
         ManualResetEvent^ ev = gcnew ManualResetEvent(done);
         if (Interlocked::CompareExchange<ManualResetEvent^>(m_asyncWaitHandle, ev, nullptr) != nullptr)
         {
            ev->Close();
         }
         else
         {
            if (!done && IsCompleted)
            {
               m_asyncWaitHandle->Set();
            }
         }
      }
      return m_asyncWaitHandle;
   }

   bool VssAsyncResult::IsCompleted::get()
   {
      return Thread::VolatileRead(m_isComplete) != 0;
   }

   VssAsyncResult^ VssAsyncResult::Create(::IVssAsync *vssAsync, AsyncCallback^ userCallback, Object^ asyncState)
   {
      try
      {
         return gcnew VssAsyncResult(vssAsync, userCallback, asyncState);
      }
      catch (...)
      {
         vssAsync->Release();
         throw;
      }
   }

   VssAsyncResult::~VssAsyncResult()
   {
      this->!VssAsyncResult();
   }

   VssAsyncResult::!VssAsyncResult()
   {
      if (m_vssAsync != 0)
      {
         m_vssAsync->Release();
         m_vssAsync = 0;
      }
   }

   void VssAsyncResult::Cancel()
   {
   }
}}}