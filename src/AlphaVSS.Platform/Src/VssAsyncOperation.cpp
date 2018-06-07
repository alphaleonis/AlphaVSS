#include "stdafx.h"
#include "VssAsyncOperation.h"

using namespace System;
using namespace System::Threading::Tasks;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
    VssAsyncOperation::VssAsyncOperation(IVssAsync *vssAsync, CancellationToken cancellationToken)
        : m_vssAsync(vssAsync), m_cancellationToken(cancellationToken), m_taskCompletionSource(gcnew TaskCompletionSource<Object^>()), m_cancellation(nullptr)
    {
        if(cancellationToken != CancellationToken::None)
            m_cancellation = cancellationToken.Register(gcnew Action(this, &VssAsyncOperation::CancelAsync));
        try 
        {
            ScheduleOperation();
        }
        catch (...)
        {
            if (m_cancellation != nullptr) delete m_cancellation;
            throw;
        }
    }

    VssAsyncOperation::~VssAsyncOperation() 
    {
        this->!VssAsyncOperation();
    }

    VssAsyncOperation::!VssAsyncOperation()
    {
        if (m_cancellation != nullptr)
        {
            delete m_cancellation;
            m_cancellation = nullptr;
        }
        if (m_vssAsync != 0)
        {
            m_vssAsync->Release();
            m_vssAsync = 0;
        }
    }

    VssAsyncOperation^ VssAsyncOperation::Create(IVssAsync *vssAsync, CancellationToken cancellationToken)
    {
        try
        {
            return gcnew VssAsyncOperation(vssAsync, cancellationToken);
        }
        catch (...)
        {
            vssAsync->Release();
            throw;
        }
    }

    Task^ VssAsyncOperation::Task::get()
    {
        return m_taskCompletionSource->Task;
    }

    void VssAsyncOperation::ScheduleOperation()
    {
        VssUtils::TaskFactory->StartNew(gcnew Action(this, &VssAsyncOperation::ExecuteOperation)); 
        // note: do not pass the cancellation token on StartNew. Even if cancellation was requested at this time, the operation needs to execute so that the result is VSS_S_ASYNC_CANCELLED.
    }

    void VssAsyncOperation::ExecuteOperation()
    {
        HRESULT hrResult;

        hrResult = m_vssAsync->Wait();

        if (SUCCEEDED(hrResult))
        {
            HRESULT hr = m_vssAsync->QueryStatus(&hrResult, NULL);
            if (FAILED(hr))
                hrResult = hr;
        }

        if (FAILED(hrResult))
            m_taskCompletionSource->SetException(GetExceptionForHr(hrResult));
        else if (hrResult == VSS_S_ASYNC_CANCELLED)
            m_taskCompletionSource->SetCanceled();
        else if (hrResult == VSS_S_ASYNC_FINISHED)
            m_taskCompletionSource->SetResult(nullptr);
        else // this really should not happen
            m_taskCompletionSource->SetException(gcnew InvalidOperationException(String::Format("Operation is not complete. HResult is {0}.", hrResult)));
    }

    void VssAsyncOperation::CancelAsync() 
    {
        m_vssAsync->Cancel();
    }
}}}
