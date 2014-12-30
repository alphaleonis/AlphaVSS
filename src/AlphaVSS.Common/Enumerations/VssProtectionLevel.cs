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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Defines the set of volume shadow copy protection levels.
   /// </summary>
   public enum VssProtectionLevel
   {
      /// <summary>
      ///     Specifies that I/O to the original volume must be maintained at the expense of shadow copies. 
      ///     This is the default protection level. Shadow copies might be deleted if both of the following conditions occur:
      ///     <list type="bullet">
      ///         <item>
      ///             <description>
      ///                 A write to the original volume occurs.
      ///             </description>
      ///         </item>
      ///         <item>
      ///             <description>
      ///                 The integrity of the shadow copy cannot be maintained for some reason, such as a failure to 
      ///                 write to the shadow copy storage area or a failure to allocate sufficient memory.
      ///             </description>
      ///         </item>
      ///     </list>
      /// </summary>
      OriginalVolume = 0,
      /// <summary>
      ///     Specifies that shadow copies must be maintained at the expense of I/O to the original volume. 
      ///     All I/O to the original volume will fail if both of the following conditions occur:
      ///     <list type="bullet">
      ///         <item>
      ///             <description>
      ///                 A write to the original volume occurs.
      ///             </description>
      ///         </item>
      ///         <item>
      ///             <description>
      ///                 The corresponding write to the shadow copy storage area cannot be completed for some reason, 
      ///                 such as a failure to write to the shadow copy storage area or a failure to allocate sufficient memory.
      ///             </description>
      ///         </item>
      ///     </list>
      /// </summary>
      Snapshot = 1,
   }
}
