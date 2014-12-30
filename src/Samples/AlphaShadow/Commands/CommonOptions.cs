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

namespace AlphaShadow.Commands
{
   class CommonOptions
   {
      public static readonly OptionSpec OptTransportable = new OptionSpec("t", OptionType.SingleValueRequired, "Transportable shadow set. Also generates the backup components doc.", false, "file.xml");
      public static readonly OptionSpec OptNonTransportableDoc = new OptionSpec("bc", OptionType.SingleValueRequired, "Generates the backup components doc for non-transportable shadow set.", false, "file.xml");
      public static readonly OptionSpec OptVerifyWriterIncluded = new OptionSpec("wi", OptionType.MultipleValuesRequired, "Verify that a writer/component is included", false, "WriterName");
      public static readonly OptionSpec OptExcludeWriter = new OptionSpec("wx", OptionType.MultipleValuesRequired, "Exclude a writer/component from set creation or restore", false, "WriterName");
      public static readonly OptionSpec OptSetVarScript = new OptionSpec("script", OptionType.SingleValueRequired, "Generates a SETVAR script with the specified filename.", false, "file.cmd");
      public static readonly OptionSpec OptExecCommand = new OptionSpec("exec", OptionType.SingleValueRequired, "Custom command executed after shadow creation.", false, "command");
      public static readonly OptionSpec OptExecCommandArgs = new OptionSpec("execArgs", OptionType.SingleValueRequired, "Arguments to send to custom command.", false, "arguments");      
   }
}
