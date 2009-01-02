using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Exception indicating that the requested method is not supported on the current operating system, or the loaded
    /// assembly is targeted for a different operating system than the one on which it is running.
    /// </summary>
    [Serializable]
    public sealed class UnsupportedOperatingSystemException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
        /// </summary>
        public UnsupportedOperatingSystemException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnsupportedOperatingSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnsupportedOperatingSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedOperatingSystemException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        private UnsupportedOperatingSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
