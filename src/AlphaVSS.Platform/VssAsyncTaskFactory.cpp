#include "pch.h"
#include "VssAsyncTaskFactory.h"

namespace Alphaleonis {
   namespace Win32 {
      namespace Vss {

         VssAsyncTaskFactory::VssAsyncInfo::VssAsyncInfo(::IVssAsync* pVssAsync, System::Threading::CancellationToken cancellationToken)
            : _vssAsync(pVssAsync), _cancellationToken(cancellationToken), _lock(gcnew Object())
         {
         }

         VssAsyncTaskFactory::VssAsyncInfo::~VssAsyncInfo()
         {
            this->!VssAsyncInfo();
         }

         VssAsyncTaskFactory::VssAsyncInfo::!VssAsyncInfo()
         {
            bool taken = false;
            try
            {
               Monitor::Enter(_lock, taken);
               if (_vssAsync != 0)
               {
                  _vssAsync->Release();
                  _vssAsync = 0;
               }
            }
            finally
            {
               if (taken)
                  Monitor::Exit(_lock);
            }
         }

         ::IVssAsync* VssAsyncTaskFactory::VssAsyncInfo::VssAsyncPtr::get()
         {
            return _vssAsync;
         }

         CancellationToken VssAsyncTaskFactory::VssAsyncInfo::CancellationToken::get()
         {
            return _cancellationToken;
         }

         void VssAsyncTaskFactory::VssAsyncInfo::Cancel()
         {
            bool taken = false;
            try
            {
               Monitor::Enter(_lock, taken);
               if (_vssAsync != 0)
               {
                  auto hr = _vssAsync->Cancel();
               }
            }
            finally
            {
               if (taken)
                  Monitor::Exit(_lock);
            }
         }

         Task^ VssAsyncTaskFactory::AsTask(::IVssAsync* vssAsync, CancellationToken cancellationToken)
         {
            // We deliberately pass CancellationToken::None to Task.Factory.StartNew here, because the task continuation is responsible for 
            // cleanup of vssAsync, so we definitely want the task to start either way here. (The cancellation token is used in the WaitWorker
            // method.
            return Task::Factory->StartNew(gcnew Action<Object^>(&WaitWorker), gcnew VssAsyncInfo(vssAsync, cancellationToken), CancellationToken::None, TaskCreationOptions::DenyChildAttach, TaskScheduler::Default);
         }

         void VssAsyncTaskFactory::CancelWorker(Object^ state)
         {
            VssAsyncInfo^ vssAsyncInfo = (VssAsyncInfo^)state;
            vssAsyncInfo->Cancel();
         }

         void VssAsyncTaskFactory::WaitWorker(Object^ state)
         {
            VssAsyncInfo^ vssAsyncInfo = (VssAsyncInfo^)state;
            IVssAsync* vssAsync = vssAsyncInfo->VssAsyncPtr;
            CancellationToken cancellationToken = vssAsyncInfo->CancellationToken;

            if (cancellationToken.CanBeCanceled)
               cancellationToken.Register(gcnew Action<Object^>(&CancelWorker), state);

            HRESULT hrResult;
            hrResult = vssAsync->Wait();

            if (SUCCEEDED(hrResult))
            {
               HRESULT hr = vssAsync->QueryStatus(&hrResult, NULL);
               if (FAILED(hr))
                  hrResult = hr;
            }

            delete vssAsyncInfo;

            if (FAILED(hrResult))
               throw GetExceptionForHr(hrResult);
            else if (hrResult == VSS_S_ASYNC_CANCELLED)
            {
               throw gcnew OperationCanceledException(cancellationToken);
            }
            else if (hrResult == VSS_S_ASYNC_FINISHED)
               return;
            else // this really should not happen
               throw gcnew InvalidOperationException(String::Format("Operation is not complete. HResult is {0}.", hrResult));
         }
      }
   }
}