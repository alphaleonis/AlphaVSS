#pragma once
#include "pch.h"

using namespace System::Threading::Tasks;
using namespace System::Threading;

namespace Alphaleonis {
   namespace Win32 {
      namespace Vss {
         
         ref class VssAsyncTaskFactory abstract sealed
         {
         private:
            ref class VssAsyncInfo
            {
            private:
               ::IVssAsync* _vssAsync;
               CancellationToken _cancellationToken;
               initonly Object^ _lock;

            public:
               VssAsyncInfo(::IVssAsync* vssAsync, CancellationToken cancellationToken);
               ~VssAsyncInfo();
               !VssAsyncInfo();

               void Cancel();

               property IVssAsync* VssAsyncPtr {
                     IVssAsync * get();
               }

               property System::Threading::CancellationToken CancellationToken {
                  System::Threading::CancellationToken get();
               }
            };

            static void WaitWorker(Object^ state);
            static void CancelWorker(Object^ state);
         public:
            static Task^ AsTask(::IVssAsync* vssAsync, CancellationToken cancellationToken);
         };

      }
   }
}