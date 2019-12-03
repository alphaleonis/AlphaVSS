
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///		Exception indicating that the creation of a shadow copy is in progress, and only one shadow copy creation 
   ///		operation can be in progress at one time. Either wait to try again or return with a failure.
   /// </summary>
   [Serializable]
   public sealed class VssSnapshotSetInProgressException : VssException
   {
      /// <summary>
      ///     Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> 
      ///     class with a system-supplied message describing the error.
      /// </summary>
      public VssSnapshotSetInProgressException()
         : base(Resources.LocalizedStrings.TheCreationOfAShadowCopyIsAlreadyInProgress)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssSnapshotSetInProgressException(string message)
         : base(message)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class with 
      ///     a specified error message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssSnapshotSetInProgressException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotSetInProgressException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="SerializationException">The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0). </exception>
      private VssSnapshotSetInProgressException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
