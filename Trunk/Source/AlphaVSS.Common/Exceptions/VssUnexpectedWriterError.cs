using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///     Exception thrown to indicate that an unexpected error occurred during communication with writers. 
    /// </summary>
    /// <remarks>
    ///     The error code is logged in the error log file.
    /// </remarks>
    [Serializable]
    public sealed class VssUnexpectedWriterError : VssException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VssUnexpectedWriterError"/> 
        ///     class with a system-supplied message describing the error.
        /// </summary>
        public VssUnexpectedWriterError()
            : base(Resources.LocalizedStrings.AnUnexpectedErrorOccurredDuringCommunicationWithWriters)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VssUnexpectedWriterError"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public VssUnexpectedWriterError(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VssUnexpectedWriterError"/> class with 
        ///     a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VssUnexpectedWriterError(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VssUnexpectedWriterError"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        private VssUnexpectedWriterError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
