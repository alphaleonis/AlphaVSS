
using System;
using System.Collections.Generic;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Represents a component-level error reported by writers.
   /// </summary>
   [Serializable]
   public class VssComponentFailure
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssComponentFailure"/> class.
      /// </summary>
      /// <param name="errorCode">The error code.</param>
      /// <param name="applicationErrorCode">The application error code.</param>
      /// <param name="applicationMessage">The application message.</param>
      public VssComponentFailure(int errorCode, int applicationErrorCode, string applicationMessage)
      {
         ErrorCode = errorCode;
         ApplicationErrorCode = applicationErrorCode;
         ApplicationMessage = applicationMessage;
      }

      #region Properties

      /// <summary>
      /// Gets the HRESULT failure code that the writer passed for the hr parameter of the IVssComponentEx2::SetFailure method. 
      /// </summary>
      public int ErrorCode { get; private set; }

      /// <summary>
      /// Gets the additional error code if provided by the writer.
      /// </summary>
      public int ApplicationErrorCode { get; private set; }

      /// <summary>
      /// Gets an error message for the requester to display to the end user. The writer is responsible for localizing this string if necessary before using it in this method. This parameter is optional and can be <see langword="null"/> or an empty string.
      /// </summary>
      public string ApplicationMessage { get; private set; }

      #endregion
   }
}
