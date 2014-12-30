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
   ///     The <see cref="VssFileRestoreStatus" /> enumeration defines the set of statuses of a file restore operation performed on 
   ///     the files managed by a selected component or component set.
   /// </summary>
   /// <remarks>
   ///     See MSDN documentation on <c>VSS_FILE_RESTORE_STATUS</c> for more information.   
   /// </remarks>
   public enum VssFileRestoreStatus
   {
      /// <summary>
      /// 	<para>
      /// 	    The restore state is undefined.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error, or indicates that a restore operation has not yet started.
      /// 	</para>
      /// </summary>
      Undefined = 0,

      /// <summary>
      /// 	<para>
      /// 		No files were restored.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error in restoration that did not leave any restored files on disk.
      /// 	</para>
      /// </summary>
      None = 1,

      /// <summary>
      /// 	All files were restored. This value indicates success and should be set for each component that was restored successfully.
      /// </summary>
      All = 2,

      /// <summary>
      /// 	<para>
      /// 	    The restore process failed.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error in restoration that did leave some restored files on disk. This means the components on disk are now corrupt.
      /// 	</para>
      /// </summary>
      Failed = 3
   }
}
