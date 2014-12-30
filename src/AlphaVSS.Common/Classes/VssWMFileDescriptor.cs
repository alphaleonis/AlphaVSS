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
   /// The <see cref="VssWMFileDescriptor"/> class is returned to a calling application by a number of query methods. 
   /// It provides detailed information about a file or set of files (a file set).
   /// </summary>
   /// <remarks>
   ///     The following methods return a <see cref="VssWMFileDescriptor"/> instance:
   ///     <list type="bullet">
   ///         <item><description><see cref="IVssComponent.AlternateLocationMappings"/></description></item>
   ///         <item><description><see cref="IVssComponent.NewTargets"/></description></item>
   ///         <item><description><see cref="IVssExamineWriterMetadata.ExcludeFiles"/></description></item>
   ///         <item><description><see cref="IVssExamineWriterMetadata.AlternateLocationMappings"/></description></item>
   ///         <item><description><see cref="IVssWMComponent.Files"/></description></item>
   ///         <item><description><see cref="IVssWMComponent.DatabaseFiles"/></description></item>
   ///         <item><description><see cref="IVssWMComponent.DatabaseLogFiles"/></description></item>
   ///     </list>
   /// </remarks>
   [Serializable]
   public class VssWMFileDescriptor
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssWMFileDescriptor"/> class.
      /// </summary>
      /// <param name="alternateLocation">The alternate location.</param>
      /// <param name="backupTypeMask">The backup type mask.</param>
      /// <param name="fileSpecification">The file specification.</param>
      /// <param name="path">The path.</param>
      /// <param name="isRecursive">if set to <c>true</c> this file description is recursive.</param>
      public VssWMFileDescriptor(string alternateLocation, VssFileSpecificationBackupType backupTypeMask, string fileSpecification, string path, bool isRecursive)
      {
         AlternateLocation = alternateLocation;
         BackupTypeMask = backupTypeMask;
         FileSpecification = fileSpecification;
         Path = path;
         IsRecursive = isRecursive;
      }

      #region Public Properties

      /// <summary>
      /// Obtains the alternate backup location of the component files.
      /// </summary>
      public string AlternateLocation { get; private set; }

      /// <summary>
      /// Obtains the file backup specification for a file or set of files.
      /// </summary>
      /// <remarks><note><b>Windows XP:</b> This value is not supported in Windows XP and will always return <see cref="VssFileSpecificationBackupType.Unknown"/></note></remarks>
      public VssFileSpecificationBackupType BackupTypeMask { get; private set; }

      /// <summary>
      /// Obtains the file specification for the list of files provided.
      /// </summary>
      public string FileSpecification { get; private set; }

      /// <summary>
      /// Obtains the fully qualified directory path for the list of files provided.
      /// </summary>
      public string Path { get; private set; }

      /// <summary>
      /// Determines whether only files in the root directory or files in the entire directory hierarchy are considered for backup.
      /// </summary>
      /// <remarks>VSS API reference: <c>IVssWMFiledesc::GetRecursive()</c></remarks>
      public bool IsRecursive { get; private set; }

      #endregion
   };
}
