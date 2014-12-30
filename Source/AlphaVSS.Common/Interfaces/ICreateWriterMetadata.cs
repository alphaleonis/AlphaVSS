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

namespace Alphaleonis.Win32.Vss.Interfaces
{
#if false // This interface is not yet supported
    class ICreateWriterMetadata
    {
        /// <summary>
        ///     <para>Reports any file sets that will be explicitly excluded by the writer when a shadow copy is created.</para>
        ///     <para>
        ///         Calling this method does not cause the files to be excluded. The writer is responsible for 
        ///         deleting the files from the shadow copy in its <c>CVssWriter::OnPostSnapshot</c> method.</para>
        /// </summary>
        /// <param name="path">
        ///     <para>A string containing the root directory under which files are to be excluded.</para>
        ///     <para>The path can contain environment variables (for example, %SystemRoot%) but cannot contain wildcard characters.</para>
        ///     <para>
        ///         There is no requirement that the path end with a backslash ("\"). It is up to applications that retrieve 
        ///         this information to check whether the path ends with a backslash.
        ///     </para>
        /// </param>
        /// <param name="fileSpecification">
        ///     <para>A string containing the file specification of the files to be excluded.</para>
        ///     <para>
        ///         A file specification cannot contain directory specifications (for example, no backslashes) 
        ///         but can contain the ? and * wildcard characters.
        ///     </para>
        /// </param>
        /// <exception cref="ArgumentNullException">One of the arguments that cannot be <see langword="null"/> was <see langword="null"/></exception>
        /// <exception cref="ArgumentException">One of the parameter values is not valid.</exception>
        /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
        /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
        /// <exception cref="VssInvalidXmlDocumentException">The XML document is not valid. Check the event log for details.</exception>
        /// <remarks>
        ///     <para>
        ///         The use of the <see cref="AddExcludeFilesFromSnapshot"/> method is optional. Writers should use this method 
        ///         only for large files that change significantly between shadow copy operations.
        ///     </para>
        ///     <para>
        ///         This method is not a substitute for the <see cref="IVssCreateWriterMetadata.AddExcludeFiles"/> method. Writers 
        ///         should continue to use the <c>AddExcludeFiles"</c> method to report which file sets are excluded from backup.
        ///     </para>
        ///     <note>
        ///         <para>
        ///             <b>Windows XP and Windows Server 2003 (SP1):</b> This method is not supported until Windows Vista
        ///         </para>
        ///     </note>
        /// </remarks>
        void AddExcludeFilesFromSnapshot(string path, string fileSpecification);
    }
#endif
}
