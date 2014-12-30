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
