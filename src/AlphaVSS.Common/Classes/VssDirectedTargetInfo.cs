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
   ///		Represents information stored by a writer, at backup time, to the Backup Components Document to indicate that when a 
   ///		file is to be restored, it (the source file) should be remapped. The file may be restored to a new restore target 
   ///		and/or ranges of its data restored to different locations with the restore target.
   /// </summary>
   [Serializable]
   public class VssDirectedTargetInfo
   {
      /// <summary>Initializes a new instance of the <see cref="VssDirectedTargetInfo"/> class.</summary>
      /// <param name="sourcePath">The source path.</param>
      /// <param name="sourceFileName">The source file name.</param>
      /// <param name="sourceRangeList">The source range list.</param>
      /// <param name="destinationPath">The destination path.</param>
      /// <param name="destinationFileName">The destination file name.</param>
      /// <param name="destinationRangeList">The destination range list.</param>
      public VssDirectedTargetInfo(string sourcePath, string sourceFileName,
         string sourceRangeList, string destinationPath,
         string destinationFileName, string destinationRangeList)
      {
         SourcePath = sourcePath;
         SourceFileName = sourceFileName;
         SourceRangeList = sourceRangeList;
         DestinationPath = destinationPath;
         DestinationFileName = destinationFileName;
         DestinationRangeList = destinationRangeList;
      }

      #region Properties

      /// <summary>
      /// 	The path to the directory that at backup time contained the file to be restored (the source file). This path should 
      /// 	match or be beneath the path of a file set already in the component or one of its Subcomponents 
      /// 	(if the component defines a component set).
      /// </summary>
      public string SourcePath { get; private set; }

      /// <summary>
      /// 	The name of the file (at backup time) that is to be remapped during a restore (the source file). 
      /// 	The name of this file should not contain any wildcard characters, and must be a member of the same 
      /// 	file set as the source path (<see cref="SourcePath" />).
      /// </summary>
      public string SourceFileName { get; private set; }

      /// <summary>
      /// 	<para>
      /// 		A comma-separated list of file offsets and lengths indicating the source file 
      /// 		support range (the sections of the file to be restored).
      /// 	</para>
      /// 	<para>
      /// 		The number and length of the source file support ranges must match the number and size of destination file support ranges.
      /// 	</para>
      /// </summary>
      public string SourceRangeList { get; private set; }

      /// <summary>
      /// 	The path to which source file data will be remapped at restore time.
      /// </summary>
      public string DestinationPath { get; private set; }

      /// <summary>
      /// 	The name of the file to which source file data will be remapped at restore time.
      /// </summary>
      public string DestinationFileName { get; private set; }

      /// <summary>
      /// 	<para>
      /// 		A comma-separated list of file offsets and lengths indicating the destination file support range (locations to which 
      /// 		the sections of the source file are to be restored).
      /// 	</para>
      /// 	<para>
      /// 		The number and length of the destination file support ranges must match the number and size of source file support ranges.
      /// 	</para>
      /// </summary>
      public string DestinationRangeList { get; private set; }

      #endregion
   };
}