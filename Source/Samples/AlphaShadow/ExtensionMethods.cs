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
using System.Text;
using Alphaleonis.Win32.Vss;
using Alphaleonis.Win32.Filesystem;

namespace AlphaShadow
{
   internal static class ExtensionMethods
   {      
      public static string GetExpandedPath(this VssWMFileDescriptor fileDesc)
      {
         return AppendBackslash(Environment.ExpandEnvironmentVariables(fileDesc.Path));
      }

      public static string GetAffectedVolume(this VssWMFileDescriptor fileDesc, IUIHost host)
      {
         string expandedPath = AppendBackslash(Environment.ExpandEnvironmentVariables(fileDesc.Path));

         try
         {
            return Volume.GetUniqueVolumeNameForPath(host, expandedPath, true);
         }
         catch
         {
            return expandedPath;
         }
      }

      public static string GetFullPath(this IVssWMComponent component, IUIHost host)
      {
         string fullPath = AppendBackslash(component.LogicalPath) + component.ComponentName;
         if (!fullPath.StartsWith("\\"))
            fullPath = "\\" + fullPath;
         return fullPath;
      }

      public static string AppendBackslash(this string str)
      {
         if (str == null)
            return "\\";
         else if (str.EndsWith("\\"))
            return str;
         else
            return str + "\\";
      }
   }
}
