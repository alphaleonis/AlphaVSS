
using System;
using System.Collections.Generic;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Represents the status of an asynchronous operation performed by the VSS framework.
   /// </summary>
   public interface IVssAsyncResult : IAsyncResult, IDisposable
   {
      /// <summary>
      /// Cancels an incomplete asynchronous operation.
      /// </summary>
      void Cancel();
   }
}
