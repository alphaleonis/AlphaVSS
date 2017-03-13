
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     Exception thrown to indicate that the volume does not support the requested operation, or that no provider supports it.
   /// </summary>
   [Serializable]
   public sealed class VssVolumeNotSupportedException : VssException
   {
      /// <summary>
      ///     Initializes a new instance of the <see cref="VssVolumeNotSupportedException"/> 
      ///     class with a system-supplied message describing the error.
      /// </summary>
      public VssVolumeNotSupportedException()
         : base(Resources.LocalizedStrings.TheVolumeDoesNotSupportTheRequestedOperation)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssVolumeNotSupportedException"/> class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssVolumeNotSupportedException(string message)
         : base(message)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssVolumeNotSupportedException"/> class with 
      ///     a specified error message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssVolumeNotSupportedException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssVolumeNotSupportedException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null"/> or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
      private VssVolumeNotSupportedException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
