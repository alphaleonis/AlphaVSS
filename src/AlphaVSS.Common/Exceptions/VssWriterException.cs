
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Base class for exceptions thrown to indicate errors reported by VSS writers.
   /// </summary>    
   [Serializable]
   public abstract class VssWriterException : VssException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssWriterException"/> class.
      /// </summary>
      protected VssWriterException()
         : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssWriterException"/> class with the specified error message.
      /// </summary>
      /// <param name="message">The error message.</param>
      protected VssWriterException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssWriterException"/> class with the specified error message
      /// and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message.</param>
      /// <param name="innerException">The inner exception.</param>
      protected VssWriterException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssWriterException"/> class with serialized data.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
      protected VssWriterException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
