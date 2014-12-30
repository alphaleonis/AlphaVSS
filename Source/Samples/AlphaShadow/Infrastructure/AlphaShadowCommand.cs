/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaShadow
{
   public abstract class AlphaShadowCommand : Command
   {
      protected static OptionSpec OptVerbose = new OptionSpec("verbose", OptionType.ValueProhibited, "Enables verbose tracing output.", false);
      protected static OptionSpec OptNoWrap = new OptionSpec("nowrap", OptionType.ValueProhibited, "Disables wordwrapping output text", false);

      public AlphaShadowCommand(string name, string description)
         : base(name, description)
      {         
      }

      public virtual IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            yield break;
         }
      }

      public override IEnumerable<OptionSpec> Options
      {
         get
         {
            return CommandSpecificOptions.Concat(new [] { OptVerbose, OptNoWrap });
         }
      }

      protected override void ProcessOptions()
      {
         Host.VerboseOutputEnabled = HasOption(OptVerbose);
         Host.IsWordWrapEnabled = !HasOption(OptNoWrap);
      }
   }  
}
