
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