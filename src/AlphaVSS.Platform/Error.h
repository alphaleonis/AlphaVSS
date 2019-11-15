
#pragma once


namespace Alphaleonis { namespace Win32 { namespace Vss
{
   Exception ^GetExceptionForHr(HRESULT errorCode);
   void ThrowException(HRESULT errorCode);
   void WaitCheckAndReleaseVssAsyncOperation(::IVssAsync *pAsync);
}	
} }
