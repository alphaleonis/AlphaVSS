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
   /// 	Information about a file set (a specified file or files) to participate in an incremental or differential backup 
   /// 	or restore as a differenced file, that is, backup and restores associated with it are to be implemented as if 
   /// 	entire files are copied to and from backup media (as opposed to using partial files).
   /// </summary>
   [Serializable]
   public class VssDifferencedFileInfo
   {
      /// <summary>Initializes a new instance of the <see cref="VssDifferencedFileInfo"/> class.</summary>
      /// <param name="path">The path to the differenced files.</param>
      /// <param name="fileSpecification">The file specification of the differenced files.</param>
      /// <param name="lastModifyTime">The time of last modification for the difference files.</param>
      /// <param name="isRecursive"><see langword="true"/> if the filespec for the differenced files should be interpreted recursively, <see langword="false"/> otherwise.</param>
      public VssDifferencedFileInfo(string path, string fileSpecification, bool isRecursive, DateTime lastModifyTime)
      {
         Path = path;
         FileSpecification = fileSpecification;
         IsRecursive = isRecursive;
         LastModifyTime = lastModifyTime;
      }

      #region Properties

      /// <summary>
      /// 	<para>
      /// 		The path to the differenced files.
      /// 	</para>
      /// 	<para>
      /// 		Users of this method need to check to determine whether this path ends with a backslash (\).
      /// 	</para>
      /// </summary>
      public string Path { get; private set; }

      /// <summary>The file specification of the differenced files.</summary>
      public string FileSpecification { get; private set; }

      /// <summary>
      /// 	Boolean specifying whether the file specification for the differenced files should be interpreted recursively. 
      /// 	If <see langword="true"/>, then the entire directory hierarchy will need to be searched for files matching the 
      /// 	file specification <see cref="FileSpecification"/> to find files to be handled as differenced files during incremental 
      /// 	or differential backups. If <see langword="false"/>, only the root directory needs to be searched.
      /// </summary>
      public bool IsRecursive { get; private set; }

      /// <summary>
      /// 	The writer specification of the time of last modification for the difference files.
      /// </summary>
      public DateTime LastModifyTime { get; private set; }

      #endregion
   };
}
