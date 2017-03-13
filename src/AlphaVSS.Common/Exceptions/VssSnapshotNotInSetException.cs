
using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Exception thrown to indicate that the specified snapshot specifies a shadow copy that does not exist in the Backup Components Document.
   /// </summary>
   [Serializable]
   public class VssSnapshotNotInSetException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotNotInSetException"/> class.
      /// </summary>
      public VssSnapshotNotInSetException() 
         : base(Alphaleonis.Win32.Vss.Resources.LocalizedStrings.VssSnapshotNotInSetExceptionMessage)
      { 
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotNotInSetException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public VssSnapshotNotInSetException(string message) : base(message) { }
      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotNotInSetException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="inner">The inner.</param>
      public VssSnapshotNotInSetException(string message, Exception inner) : base(message, inner) { }
      /// <summary>
      /// Initializes a new instance of the <see cref="VssSnapshotNotInSetException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// The <paramref name="info"/> parameter is <see langword="null"/>.
      /// </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">
      /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
      /// </exception>
      protected VssSnapshotNotInSetException(
       SerializationInfo info,
       StreamingContext context)
         : base(info, context) { }
   }
}
