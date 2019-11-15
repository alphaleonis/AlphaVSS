
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     <c>IVssFactory</c> provides an interface to the global methods of the VSS API compiled for a specific 
   ///      platform. 
   /// </summary>
   /// <remarks>
   ///     An instance of <c>IVssFactory</c> can be obtained either by using the <see cref="VssFactoryProvider"/> (recommended), or in 
   ///     advanced scenarios by statically referencing the correct platform-specific assembly and manually creating an instance of <c>VssFactory</c>
   ///     from that assembly.
   /// </remarks>
   /// <seealso cref="VssFactoryProvider"/>
   public interface IVssFactory
   {
      /// <summary>
      /// The <c>CreateVssBackupComponents</c> method creates an <see cref="IVssBackupComponents" /> interface object 
      /// for the current implementation and returns a reference to it.
      /// </summary>
      /// <remarks>
      ///     The calling application is responsible for calling <c>Dispose"</c> to release the 
      ///     resources held by the <see cref="IVssBackupComponents"/> instance when it is no longer needed.
      /// </remarks>
      /// <returns>A reference to the newly created <see cref="IVssBackupComponents"/> instance.</returns>
      /// <exception cref="UnauthorizedAccessException">The caller does not have sufficient backup privileges or is not an administrator.</exception>
      /// <exception cref="OutOfMemoryException">Out of memory or other system resources.</exception>
      /// <exception cref="SystemException">Unexpected VSS system error. The error code is logged in the event log.</exception>
      /// <seealso cref="IVssBackupComponents"/>
      IVssBackupComponents CreateVssBackupComponents();     

      /// <summary>
      /// Gets a snapshot management interface for the current implementation.
      /// </summary>
      /// <returns>A snapshot management interface for the current implementation.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      IVssSnapshotManagement CreateVssSnapshotManagement();

      /// <summary>
      /// 	The <b>CreateVssExamineWriterMetadata</b> function creates a new <see cref="IVssExamineWriterMetadata"/> instance from an 
      /// 	XML document for the current implementation.
      /// </summary>
      /// <param name="xml">A string containing a Writer Metadata Document with which to initialize the returned <see cref="IVssExamineWriterMetadata"/> object.</param>
      /// <remarks>
      /// 	This method attempts to load the returned <see cref="IVssExamineWriterMetadata"/> object with metadata previously stored by a call to 
      /// 	<see cref="IVssExamineWriterMetadata.SaveAsXml"/>. Users should not tamper with this metadata document.
      /// </remarks>
      /// <returns>a <see cref="IVssExamineWriterMetadata"/> instance initialized with the specified XML document.</returns>
      IVssExamineWriterMetadata CreateVssExamineWriterMetadata(string xml);

      /// <summary>
      /// Gets an instance of <see cref="IVssInfoProvider"/>.
      /// </summary>
      /// <returns>An instance of <see cref="IVssInfoProvider"/>.</returns>
      IVssInfoProvider GetInfoProvider();
   }
}
