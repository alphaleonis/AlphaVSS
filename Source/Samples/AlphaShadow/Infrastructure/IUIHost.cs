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
using System.Diagnostics;
using System.Collections.Generic;

namespace AlphaShadow
{
   public interface IUIHost
   {
      IDisposable GetIndent();
      void ExecCommand(string execCommand, string args);
      bool VerboseOutputEnabled { get; set; }
      bool IsWordWrapEnabled { get; set; }            
      
      void WriteLine();
      void WriteTable(StringTable table, int columnSpacing = 3, bool addRowSpace = false);

      void PushIndent();
      void PopIndent();
      void WriteHeader(string message, params object[] args);
      void WriteLine(string message, params object[] args);
      void WriteWarning(string message, params object[] args);
      void WriteError(string message, params object[] args);
      void WriteVerbose(string message, params object[] args);

      bool ShouldContinue();
   }
}
