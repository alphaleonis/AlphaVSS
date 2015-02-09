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
	   if (IsCompleted || m_vssAsync == 0)
		   return;

      HRESULT hrResult;
      
      hrResult = m_vssAsync->Wait();

      int prevState = Interlocked::Exchange(m_isComplete, -1);

      if (prevState != 0)
         throw gcnew InvalidOperationException("WaitForAsyncCompletion can only be called once.");

      if (SUCCEEDED(hrResult))
      {
		  // zDougie - something is wrong here.  m_vssAsync becomes null.
		  // I've attempted isolating with if(m_vssAsync==0) after the wait, but it doesn't work
		  // very intermittent
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
	   if (m_vssAsync != 0)
	   {
		   CheckCom(m_vssAsync->Cancel());
	   }
   }

   UInt32 VssAsyncResult::QueryStatus()
   {
	   if (m_vssAsync != 0)
	   {
		   HRESULT
			   hr = 0;
		   Int32
			   resv = 0;
		   CheckCom(m_vssAsync->QueryStatus(&hr, &resv));
		   return hr;
	   }
	   return S_OK;
   }

   bool VssAsyncResult::Wait(TimeSpan TimeOut)
   {
	   if (m_vssAsync != 0)
	   {
		   UInt32
			   ms = (UInt32)TimeOut.TotalMilliseconds;
		   if (ms <= 0)
			   ms = 1000 * 5 * 60;
		   HRESULT
			   result = m_vssAsync->Wait(ms);
		   if (result == VSS_E_UNEXPECTED || result == E_ACCESSDENIED)
			   throw gcnew System::Runtime::InteropServices::COMException("Waiting for Async Operation", result);
		   return (UINT)result == S_OK;
	   }
	   return true;
   }

}}}