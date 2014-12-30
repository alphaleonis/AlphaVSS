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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// 	The <see cref="IVssWriterComponents"/> interface contains methods used to obtain and modify component information 
   /// 	(in the form of <see cref="IVssComponent"/> instances) associated with a given writer but stored in a 
   /// 	requester's Backup Components Document.
   /// </summary>
   public interface IVssWriterComponents
   {
      /// <summary>
      /// 	A read-only collection of <see cref="IVssComponent"/> instances to the a given writer's 
      /// 	components explicitly stored in the Backup Components Document. 
      /// </summary>
      /// <value>A read-only collection of <see cref="IVssComponent"/> instances to the a given writer's 
      /// 	components explicitly stored in the Backup Components Document. <note type="caution">This list 
      /// 	must not be accessed after the <see cref="IVssComponent"/> from which it was obtained has been disposed.</note>
      /// </value>
      IList<IVssComponent> Components { get; }

      /// <summary>
      ///     Identifier of the writer instance responsible for the components.
      /// </summary>
      Guid InstanceId { get; }

      /// <summary>
      ///     Identifier of the writer class responsible for the components.
      /// </summary>
      Guid WriterId { get; }
   }
}
