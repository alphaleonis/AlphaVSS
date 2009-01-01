using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Alphaleonis.Win32.Vss
{
    public sealed class UnsupportedOperatingSystemException : Exception
    {
        public UnsupportedOperatingSystemException()
            : base()
        {
        }

        public UnsupportedOperatingSystemException(string message)
            : base(message)
        {
        }

        public UnsupportedOperatingSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private UnsupportedOperatingSystemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
