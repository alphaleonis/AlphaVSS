
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{

   /// <summary>
   ///     Exception thrown to indicate that the volume has been added to the maximum number of shadow copy sets.
   ///     The specified volume was not added to the shadow copy set.
   /// </summary>
   [Serializable]
   public sealed class VssMaximumNumberOfVolumesReachedException : VssException
   {
      /// <summary>
      ///     Initializes a new instance of the <see cref="VssMaximumNumberOfVolumesReachedException"/> 
      ///     class with a system-supplied message describing the error.
      /// </summary>
      public VssMaximumNumberOfVolumesReachedException()
         : base(Resources.LocalizedStrings.TheVolumeHasAlreadyBeenAddedToTheMaximumNumberOfShadowCopySets)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssMaximumNumberOfVolumesReachedException"/> class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      public VssMaximumNumberOfVolumesReachedException(string message)
         : base(message)
      {
      }

      /// <summary>
      ///     Initializes a new instance of the <see cref="VssMaximumNumberOfVolumesReachedException"/> class with 
      ///     a specified error message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The message that describes the error</param>
      /// <param name="innerException">The exception that is the cause of the current exception.</param>
      public VssMaximumNumberOfVolumesReachedException(string message, Exception innerException)
         : base(message, innerException)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssMaximumNumberOfVolumesReachedException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null"/>. </exception>
      /// <exception cref="SerializationException">The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0). </exception>
      private VssMaximumNumberOfVolumesReachedException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }
   }
}
