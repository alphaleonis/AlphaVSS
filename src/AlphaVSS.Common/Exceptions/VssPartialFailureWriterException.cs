
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Exception indicating the writer is reporting one or more component-level errors. 
   /// To retrieve the errors, the requester must use the <see cref="IVssComponent.Failure"/> property.
   /// </summary>
   [Serializable]
   public class VssPartialFailureWriterException : VssWriterException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssPartialFailureWriterException"/> class.
      /// </summary>
      public VssPartialFailureWriterException()
         : base(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.VssPartialFailureWriterExceptionMessage)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssPartialFailureWriterException"/> class.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssPartialFailureWriterException(string message) : base(message) { }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssPartialFailureWriterException"/> class.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssPartialFailureWriterException(string message, Exception innerException) : base(message, innerException) { }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssPartialFailureWriterException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="ArgumentNullException">
      /// The <paramref name="info"/> parameter is <see langword="null"/>.
      /// </exception>
      /// <exception cref="SerializationException">
      /// The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0).
      /// </exception>
      protected VssPartialFailureWriterException(SerializationInfo info, StreamingContext context)
         : base(info, context) { }
   }
}

