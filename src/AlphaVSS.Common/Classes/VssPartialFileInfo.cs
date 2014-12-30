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
   ///		Representation of information on a partial file associated with a component.
   /// </summary>
   /// <remarks>See <see href="http://msdn.microsoft.com/en-us/library/aa383529(VS.85).aspx">MSDN documentation on IVssComponent::GetPartialFile Method</see> for more information.</remarks>
   [Serializable]
   public class VssPartialFileInfo
   {
      /// <summary>Initializes a new instance of the <see cref="VssPartialFileInfo"/> class</summary>
      /// <param name="path">The path of the partial file.</param>
      /// <param name="fileName">The name of the partial file.</param>
      /// <param name="range">Either a listing of file offsets and lengths that make up the partial file support range 
      /// 	(the sections of the file that were backed up), or the name of a file containing such a list.</param>
      /// <param name="metadata">Any additional metadata required by a writer to validate a partial file restore operation. The information in this 
      /// 		metadata string will be opaque to requesters. May be <see langword="null"/></param>
      public VssPartialFileInfo(string path, string fileName, string range, string metadata)
      {
         if (path == null)
            throw new ArgumentNullException("path");

         if (fileName == null)
            throw new ArgumentNullException("fileName");

         Path = path;
         FileName = fileName;
         Range = range;
         Metadata = metadata;
      }

      #region Public Properties

      /// <summary>
      /// 	<para>
      /// 		The path of the partial file.
      /// 	</para>
      /// 	<para>
      /// 		Users of this public need to check to determine whether this path ends with a backslash ("\").
      /// 	</para>
      /// </summary>
      public string Path { get; private set; }

      /// <summary>The name of the partial file.</summary>
      public string FileName { get; private set; }

      /// <summary>
      /// 	Either a listing of file offsets and lengths that make up the partial file support range 
      /// 	(the sections of the file that were backed up), or the name of a file containing such a list.
      /// </summary>
      public string Range { get; private set; }

      /// <summary>
      /// 	<para>
      /// 		Any additional metadata required by a writer to validate a partial file restore operation. The information in this 
      /// 		metadata string will be opaque to requesters.
      /// 	</para>
      /// 	<para>
      /// 		Additional metadata is not required, so <see cref="Metadata"/> may also be empty (zero length).
      /// 	</para>
      /// </summary>
      public string Metadata { get; private set; }

      #endregion
   };
}