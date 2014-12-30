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
   /// <summary>The <see cref="VssRestoreType"/> enumeration is used by a requester to indicate the type of restore operation it is about to perform.</summary>
   /// <remarks>
   ///     <para>A requester sets the type of a restore operation using <see cref="IVssBackupComponents.SetRestoreState"/>.</para>
   ///     <!-- <para>A writer can retrieve the type of a restore operation by calling CVssWriter::GetRestoreType.</para> -->
   /// </remarks>
   public enum VssRestoreType
   {
      /// <summary>
      /// 	<para>No restore type is defined.</para>
      /// 	<para>This indicates an error on the part of the requester.</para>
      /// </summary>
      Undefined = 0,

      /// <summary>The default restore type: A requester restores backed-up data to the original volume from a backup medium.</summary>
      ByCopy = 1,

      /// <summary>
      /// 	<para>
      /// 		A requester does not copy data from a backup medium, but imports a transportable shadow copy 
      /// 		and uses this imported volume for operations such as data mining.
      /// 	</para>
      /// 	<para>
      /// 		<b>Windows Server 2003, Standard Edition and Windows Server 2003, Web Edition:</b> This value is not supported. All editions of Windows Server 2003 SP1 support this value.
      /// 	</para>
      /// </summary>
      Import = 2,

      /// <summary>A restore type not currently enumerated. This value indicates an application error.</summary>
      Other = 3

   };
}
