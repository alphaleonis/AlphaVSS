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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///		The <see cref="VssObjectType"/> enumeration is used by requesters to identify an object as 
   ///		a shadow copy set, shadow copy, or provider.
   /// </summary>
   public enum VssObjectType
   {
      /// <summary>
      /// 	<para>
      /// 	    The object type is not known.
      /// 	</para>
      /// 	<para>
      /// 		This indicates an application error.
      /// 	</para>
      /// </summary>
      Unknown = 0,

      /// <summary>
      /// 	<para>
      /// 		The interpretation of this value depends on whether it is used as an 
      /// 		input to a VSS method or returned as an output from a VSS method.
      /// 	</para>
      /// 	<para>
      /// 		When used as an input to a VSS method, it indicates that the method is 
      /// 		not restricted to any particular object type, but should act on all 
      /// 		appropriate objects. In this sense, <see cref="None"/> can be thought 
      /// 		of as a wildcard input.
      /// 	</para>
      /// 	<para>
      /// 		When returned as an output, the object type is not known and means that 
      /// 		there has been an application error.
      /// 	</para>
      /// </summary>
      None = 1,

      /// <summary>Shadow copy set.</summary>
      SnapshotSet = 2,

      /// <summary>Shadow copy.</summary>
      Snapshot = 3,

      /// <summary>Shadow copy provider.</summary>		
      Provider = 4,
   };
}