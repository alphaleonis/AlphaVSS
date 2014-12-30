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

#include <vss.h>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Threading;
namespace Alphaleonis { namespace Win32 { namespace Vss
{
   private ref class VssAsyncResult : IVssAsyncResult, MarshalByRefObject
   {
   private:
      AsyncCallback^ m_asyncCallback;
      Object^ m_asyncState;
      int m_isComplete;
      ManualResetEvent^ m_asyncWaitHandle;
      ::IVssAsync *m_vssAsync;
      Exception^ m_exception;

      VssAsyncResult(::IVssAsync *vssAsync, AsyncCallback^ userCallback, Object^ asyncState);
      void WaitForAsyncCompletion(Object^ state);
   public:
      /// <summary>Destructor.</summary>
      ~VssAsyncResult();

      /// <summary>Releases resources used by the <see cref="VssAsyncResult"/> object.</summary>
      !VssAsyncResult();

      property Object^ AsyncState { virtual Object^ get(); }
      property bool CompletedSynchronously { virtual bool get(); }
      property WaitHandle^ AsyncWaitHandle { virtual WaitHandle^ get(); }
      property bool IsCompleted { virtual bool get(); }
      virtual void Cancel();
   internal:
      void EndInvoke();

      static VssAsyncResult^ Create(::IVssAsync *vssAsync, AsyncCallback^ userCallback, Object^ asyncState);
   };
}}}