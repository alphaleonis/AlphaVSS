
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Base class for exceptions indicating an error occuring during a VSS operation.
   /// </summary>    
   [Serializable]
   public abstract class VssException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssException"/> class.
      /// </summary>
      protected VssException()
         : base()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssException"/> class with the specified error message.
      /// </summary>
      /// <param name="message">The error message.</param>
      protected VssException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssException"/> class with the specified error message
      /// and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message.</param>
      /// <param name="innerException">The inner exception.</param>
      protected VssException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssException"/> class with serialized data.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="SerializationException">The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0). </exception>
      protected VssException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }

}
