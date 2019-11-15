
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaShadow
{
   [Serializable]
   public class CommandAbortedException : Exception
   {
      public CommandAbortedException() { }
      public CommandAbortedException(string message) : base(message) { }
      public CommandAbortedException(string message, Exception inner) : base(message, inner) { }
      protected CommandAbortedException(
       System.Runtime.Serialization.SerializationInfo info,
       System.Runtime.Serialization.StreamingContext context)
         : base(info, context) { }
   }
}
