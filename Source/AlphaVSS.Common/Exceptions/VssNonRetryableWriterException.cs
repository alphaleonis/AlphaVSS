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
   /// Exception indicating that the writer operation failed because of an error that might recur if another shadow copy is created. 
   /// For more information, see <see href="http://msdn.microsoft.com/en-us/library/cc325736(VS.85).aspx">Event and Error Handling Under VSS</see>.
   /// </summary>
   [Serializable]
   public class VssNonRetryableWriterException : VssWriterException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssNonRetryableWriterException"/> class.
      /// </summary>
      public VssNonRetryableWriterException()
         : base(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.VssNonRetryableWriterExceptionMessage)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssNonRetryableWriterException"/> class.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssNonRetryableWriterException(string message) : base(message) { }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssNonRetryableWriterException"/> class.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssNonRetryableWriterException(string message, Exception innerException) : base(message, innerException) { }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssNonRetryableWriterException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// The <paramref name="info"/> parameter is <see langword="null"/>.
      /// </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">
      /// The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0).
      /// </exception>
      protected VssNonRetryableWriterException(
       System.Runtime.Serialization.SerializationInfo info,
       System.Runtime.Serialization.StreamingContext context)
         : base(info, context) { }
   }
}
