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
   /// Represents information about how a writer wants its data to be restored.
   /// </summary>
   /// <remarks>This class is a container for the data returned by <see cref="IVssExamineWriterMetadata.RestoreMethod"/>.</remarks>
   [Serializable]
   public class VssWMRestoreMethod
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssWMRestoreMethod"/> class.
      /// </summary>
      /// <param name="restoreMethod">The restore method.</param>
      /// <param name="service">The service.</param>
      /// <param name="userProcedure">The user procedure.</param>
      /// <param name="writerRestore">The writer restore.</param>
      /// <param name="rebootRequired">if set to <c>true</c> a reboot is required.</param>
      /// <param name="mappings">The mappings.</param>
      /// <remarks>This constructor is normally not used by application code. Rather instances of <see cref="VssWMRestoreMethod"/> are 
      /// returned by various query methods.</remarks>
      public VssWMRestoreMethod(VssRestoreMethod restoreMethod, string service, string userProcedure, VssWriterRestore writerRestore, bool rebootRequired, int mappings)
      {
         Method = restoreMethod;
         Service = service;
         UserProcedure = userProcedure;
         WriterRestore = writerRestore;
         RebootRequired = rebootRequired;
         MappingCount = mappings;
      }

      #region Public Properties

      /// <summary>
      /// A <see cref="VssRestoreMethod"/> value that specifies file overwriting, the use of alternate locations specifying the method that 
      /// will be used in the restore operation.
      /// </summary>
      public VssRestoreMethod Method { get; private set; }

      /// <summary>
      /// If the value of <see cref="Method" /> is <see cref="VssRestoreMethod.StopRestoreStart" />, a pointer to a string containing the name 
      /// of the service that is started and stopped. Otherwise, the value is <see langword="null"/>.
      /// </summary>
      public string Service { get; private set; }

      /// <summary>
      /// Pointer to the URL of an HTML or XML document describing to the user how the restore is to be performed. The value may be <see langword="null" />.
      /// </summary>
      public string UserProcedure { get; private set; }

      /// <summary>
      /// A <see cref="VssWriterRestore" /> value specifying whether the writer will be involved in restoring its data.
      /// </summary>
      public VssWriterRestore WriterRestore { get; private set; }

      /// <summary>A <see langword="bool"/> indicating whether a reboot will be required after the restore operation is complete.</summary>
      /// <value><see langword="true"/> if a reboot will be required and <see langword="false"/> if it will not.</value>
      public bool RebootRequired { get; private set; }

      /// <summary>The number of alternate mappings associated with the writer.</summary>
      public int MappingCount { get; private set; }

      #endregion
   };
}
