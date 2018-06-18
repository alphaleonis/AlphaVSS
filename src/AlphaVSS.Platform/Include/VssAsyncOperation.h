#pragma once

#include <vss.h>

using namespace System;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
    private ref class VssAsyncOperation
    {
    private:
        IVssAsync *m_vssAsync;
        CancellationToken m_cancellationToken;
        CancellationTokenRegistration^ m_cancellation;
        
        VssAsyncOperation(IVssAsync *vssAsync, CancellationToken cancellationToken);
        void ExecuteOperation();
        void CancelAsync();
    
    public:
        /// <summary>Destructor.</summary>
        ~VssAsyncOperation();

        /// <summary>Releases resources used by the <see cref="VssAsyncOperation"/> object.</summary>
        !VssAsyncOperation();

        Task^ Start();

    internal:
        static VssAsyncOperation^ Create(IVssAsync *vssAsync, CancellationToken cancellationToken);
    };
}}}