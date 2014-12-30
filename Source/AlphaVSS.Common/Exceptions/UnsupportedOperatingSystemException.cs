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
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Exception indicating that the requested method is not supported on the current operating system, or the loaded
   /// assembly is targeted for a different operating system than the one on which it is running.
   /// </summary>
   [Serializable]
   public sealed class UnsupportedOperatingSystemException : NotSupportedException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
      /// </summary>
      public UnsupportedOperatingSystemException()
         : base(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.UnsupportedOperatingSystemExceptionMessage)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public UnsupportedOperatingSystemException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public UnsupportedOperatingSystemException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
      private UnsupportedOperatingSystemException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
