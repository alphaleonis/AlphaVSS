#include "stdafx.h"
#include "VssAsyncOperation.h"

using namespace System;
using namespace System::Threading::Tasks;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
    VssAsyncOperation::VssAsyncOperation(IVssAsync *vssAsync, CancellationToken cancellationToken)
        : m_vssAsync(vssAsync), m_cancellationToken(cancellationToken), m_cancellation(nullptr)
    {
        if (cancellationToken != CancellationToken::None)
        {
            m_cancellation = cancellationToken.Register(gcnew Action(this, &VssAsyncOperation::CancelAsync));
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

    Task^ VssAsyncOperation::Start()
    {
        return VssUtils::TaskFactory->StartNew(gcnew Action(this, &VssAsyncOperation::ExecuteOperation), m_cancellationToken);
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
            throw GetExceptionForHr(hrResult);
        else if (hrResult == VSS_S_ASYNC_CANCELLED)
            throw gcnew OperationCanceledException(m_cancellationToken);
        else if (hrResult == VSS_S_ASYNC_FINISHED)
            return;
        else // this really should not happen
            throw gcnew InvalidOperationException(String::Format("Operation is not complete. HResult is {0}.", hrResult));
    }

    void VssAsyncOperation::CancelAsync() 
    {
        m_vssAsync->Cancel();
    }
}}}
