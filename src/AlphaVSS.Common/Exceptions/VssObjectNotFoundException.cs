
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     Exception thrown to indicate that the requested object did not exists.
   /// </summary>
   [Serializable]
   public sealed class VssObjectNotFoundException : VssException
   {
      /// <summary>
      ///     Initializes a new instance of the <see cref="VssObjectNotFoundException"/> 
      ///     class with a system-supplied message describing the error.
      /// </summary>
      public VssObjectNotFoundException()
         : base(Resources.LocalizedStrings.TheRequestedObjectDoesNotExist)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssObjectNotFoundException"/> class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssObjectNotFoundException(string message)
         : base(message)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssObjectNotFoundException"/> class with 
      ///     a specified error message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssObjectNotFoundException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssObjectNotFoundException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="SerializationException">The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0). </exception>
      private VssObjectNotFoundException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
