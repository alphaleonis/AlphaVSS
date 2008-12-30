using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///     A class that allows a requester to examine the metadata of a specific writer instance. This metadata may come from a 
    ///     currently executing (live) writer, or it may have been stored as an XML document.
    /// </summary>
    /// <remarks>
    ///     A <see cref="IVssExamineWriterMetadata"/> interface to a live writer's metadata is obtained by a call to 
    ///     <see cref="VssBackupComponents.WriterMetadata" />.
    /// </remarks>
    public interface IVssExamineWriterMetadata
    {
		/// <summary>
		/// The <see cref="LoadFromXML"/> method loads an XML document that contains a writer's metadata document into a
		/// <see cref="VssExamineWriterMetadata"/> instance.
		/// </summary>
		/// <param name="xml">String that contains an XML document that represents a writer's metadata document.</param>
		/// <returns><see langword="true" /> if the XML document was successfully loaded, or <see langword="false"/> if the XML document could not 
		/// be loaded.</returns>
		bool LoadFromXML(string xml);

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
		IList<VssWMFileDescription> AlternateLocationMappings { get; }

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
		IList<VssWMFileDescription> ExcludeFiles { get; }

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

    }
}
