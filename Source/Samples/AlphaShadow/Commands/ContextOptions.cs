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
   static class ContextOptions
   {
      public static readonly OptionSpec OptPersistent = new OptionSpec("p", OptionType.ValueProhibited, "Manages persistent shadow copies.", false);
      public static readonly OptionSpec OptNoWriters = new OptionSpec("nw", OptionType.ValueProhibited, "Manages no-writer shadow copies.", false);
      public static readonly OptionSpec OptNoAutoRecovery = new OptionSpec("nar", OptionType.ValueProhibited, "Creates shadow copies with no auto-recovery.", false);
      public static readonly OptionSpec OptTxFRecovered = new OptionSpec("tr", OptionType.ValueProhibited, "Creates TxF recovered shadow copies.", false);
      public static readonly OptionSpec OptDifferential = new OptionSpec("ad", OptionType.ValueProhibited, "Creates differential HW shadow copies.", false);
      public static readonly OptionSpec OptPlex = new OptionSpec("ap", OptionType.ValueProhibited, "Creates plex HW shadow copies.", false);
      public static readonly OptionSpec OptSharedFolders = new OptionSpec("scsf", OptionType.ValueProhibited, "Creates shadow copies for Shared Folders (Client Accessible).", false);

      public static OptionSpec[] All = 
		      {
		         OptPersistent,
		         OptNoWriters,
		         OptNoAutoRecovery,
		         OptTxFRecovered,
		         OptDifferential,
		         OptPlex,
		         OptSharedFolders
		      };
   }
}
