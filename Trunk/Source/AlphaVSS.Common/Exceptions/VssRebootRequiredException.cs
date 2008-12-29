using System;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///		<para>Exception thrown to indicate that the provider encountered an error that requires the user to restart the computer.</para>
    /// </summary>
    /// <remarks>
    ///     <note><b>Windows Server 2003 and Windows XP:</b>This exception is not supported until Windows Vista.</note>
    /// </remarks>
    [Serializable]
    public sealed class VssRebootRequiredException : VssException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VssRebootRequiredException"/> 
        ///     class with a system-supplied message describing the error.
        /// </summary>
        public VssRebootRequiredException()
            : base(Resources.LocalizedStrings.TheProviderEncounteredAnErrorThatRequiresTheUserToRestartTheComputer)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VssRebootRequiredException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public VssRebootRequiredException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VssRebootRequiredException"/> class with 
        ///     a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public VssRebootRequiredException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VssRebootRequiredException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        private VssRebootRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
