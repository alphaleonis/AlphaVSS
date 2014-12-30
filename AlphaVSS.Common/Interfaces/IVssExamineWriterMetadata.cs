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
   ///     A class that allows a requester to examine the metadata of a specific writer instance. This metadata may come from a 
   ///     currently executing (live) writer, or it may have been stored as an XML document.
   /// </summary>
   /// <remarks>
   ///     A <see cref="IVssExamineWriterMetadata"/> interface to a live writer's metadata is obtained by a call to 
   ///     <see cref="IVssBackupComponents.WriterMetadata" />.
   /// </remarks>
   public interface IVssExamineWriterMetadata : IDisposable
   {
      /// <summary>
      /// The <see cref="LoadFromXml"/> method loads an XML document that contains a writer's metadata document into a
      /// <see cref="IVssExamineWriterMetadata"/> instance.
      /// </summary>
      /// <param name="xml">String that contains an XML document that represents a writer's metadata document.</param>
      /// <returns><see langword="true" /> if the XML document was successfully loaded, or <see langword="false"/> if the XML document could not 
      /// be loaded.</returns>
      bool LoadFromXml(string xml);

      /// <summary>
      /// The <see cref="SaveAsXml"/> method saves the Writer Metadata Document that contains a writer's state information to a specified string. 
      /// This string can be saved as part of a backup operation.
      /// </summary>
      /// <returns>The Writer Metadata Document that contains a writer's state information.</returns>
      string SaveAsXml();

      /// <summary>
      /// The <see cref="BackupSchema"/> is examined by a requester to determine from the 
      /// Writer Metadata Document the types of backup operations that a given writer can participate in.
      /// </summary>
      VssBackupSchema BackupSchema { get; }

      /// <summary>
      /// The alternate location mappings of the file sets.
      /// </summary>
      /// <value>A read-only list containing the alternate location mappings of the file sets.</value>
      IList<VssWMFileDescriptor> AlternateLocationMappings { get; }

      /// <summary>
      /// Information about how a writer wants its data to be restored.
      /// </summary>
      VssWMRestoreMethod RestoreMethod { get; }

      /// <summary>
      /// Obtains the Writer Metadata Documents the components supported by this writer.
      /// </summary>
      /// <value>the Writer Metadata Documents the components supported by this writer.</value>
      IList<IVssWMComponent> Components { get; }

      /// <summary>Information about files that have been explicitly excluded from backup.</summary>
      /// <value>a read-only list containing information about files that have been explicitly excluded from backup.</value>
      IList<VssWMFileDescriptor> ExcludeFiles { get; }

      /// <summary>
      /// The instance identifier of the writer
      /// </summary>
      /// <value>The instance id.</value>
      Guid InstanceId { get; }

      /// <summary>The class ID of the writer</summary>
      Guid WriterId { get; }

      /// <summary>A string specifying the name of the writer</summary>
      string WriterName { get; }

      /// <summary>A <see cref="VssUsageType"/> enumeration value indicating how the data managed by the writer is used on the host system.</summary>
      VssUsageType Usage { get; }

      /// <summary>A <see cref="VssSourceType"/> enumeration value indicating the type of data managed by the writer.</summary>
      VssSourceType Source { get; }

      /// <summary>
      /// Gets the writer instance name.
      /// </summary>
      /// <remarks>
      ///     <note>
      ///         <para>
      ///             <b>Windows XP and Windows 2003:</b> This property is not supported until Windows 2003 SP1 and will always return 
      ///             <see langword="null"/> on those systems.
      ///         </para>
      ///     </note>
      /// </remarks>
      /// <value>A string specifying the writer instance name.</value>
      string InstanceName { get; }

      /// <summary>
      /// Gets the version information for a writer application.
      /// </summary>
      /// <value>The version information for a writer application.</value>
      /// <remarks>
      ///     <para>
      ///         Only the <see cref="System.Version.Major"/> and <see cref="System.Version.Minor"/> properties of the <see cref="Version"/> instance
      ///         are used by VSS.
      ///     </para>
      ///     <para>
      ///         <note><b>Windows XP and Windows 2003:</b> This property is not supported until Windows Vista, and will always return version 0.0.0.0</note>
      ///     </para>
      /// </remarks>
      Version Version { get; }

      /// <summary>
      /// Obtains information about file sets that have been explicitly excluded from a given shadow copy.
      /// </summary>
      /// <remarks>
      ///     <para>
      ///         The <see cref="ExcludeFromSnapshotFiles"/> property is intended to report information about file sets excluded from a 
      ///         shadow copy. Requesters should not exclude files from backup based on the information returned by this method.
      ///     </para>
      ///     <para>
      ///         <note>
      ///             <para><b>Windows XP and Windows 2003:</b> This property is not supported until Windows Vista and will always return an empty list.</para>
      ///         </note>
      ///     </para>
      /// </remarks>
      /// <value>The exclude from snapshot files.</value>
      IList<VssWMFileDescriptor> ExcludeFromSnapshotFiles { get; }
   }
}
