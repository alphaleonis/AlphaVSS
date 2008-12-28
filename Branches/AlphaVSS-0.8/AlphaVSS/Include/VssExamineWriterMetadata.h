/* Copyright (c) 2008 Peter Palotas
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
#pragma once

#include <vss.h>
#include "VssUsageType.h"
#include "VssSourceType.h"
#include "VssBackupSchema.h"
#include "VssWMFileDescription.h"
#include "VssWMComponent.h"
#include "VssRestoreMethod.h"
#include "VssWriterRestore.h"
#include "VssWMRestoreMethod.h"

using namespace System::Text;
using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>A class that allows a requester to examine the metadata of a specific writer instance. This metadata may come from a 
	/// currently executing (live) writer, or it may have been stored as an XML document.</summary>
	/// <remarks>
	/// A <see cref="VssExamineWriterMetadata"/> interface to a live writer's metadata is obtained by a call to 
	/// <see dref="P:Alphaleonis.Win32.Vss.VssBackupComponents.WriterMetadata" />.
	/// </remarks>
	public ref class VssExamineWriterMetadata : IDisposable
	{
	public:
		/// <summary>Releases any resources aquired by this instance</summary>
		~VssExamineWriterMetadata();
		/// <summary>Releases any resources aquired by this instance</summary>
		!VssExamineWriterMetadata();

		/// <summary>
		/// 	The <b>CreateVssExamineWriterMetadata</b> function creates a new <see cref="VssExamineWriterMetadata"/> instance from an 
		/// 	XML document.
		/// </summary>
		/// <param name="xml">A string containing a Writer Metadata Document with which to initialize the returned <see cref="VssExamineWriterMetadata"/> object.</param>
		/// <remarks>
		/// 	This method attempts to load the returned <see cref="VssExamineWriterMetadata"/> object with metadata previously stored by a call to 
		/// 	<see cref="VssExamineWriterMetadata::SaveAsXML"/>. Users should not tamper with this metadata document.
		/// </remarks>
		/// <returns>a <see cref="VssExamineWriterMetadata"/> instance initialized with the specified XML document.</returns>
		static VssExamineWriterMetadata^ Create(String^ xml);

		/// <summary>
		/// The <see cref="LoadFromXML"/> method loads an XML document that contains a writer's metadata document into a
		/// <see cref="VssExamineWriterMetadata"/> instance.
		/// </summary>
		/// <param name="xml">String that contains an XML document that represents a writer's metadata document.</param>
		/// <returns><see langword="true" /> if the XML document was successfully loaded, or <see langword="false"/> if the XML document could not 
		/// be loaded.</returns>
		bool LoadFromXML(String^ xml);

		/// <summary>
		/// The <see cref="SaveAsXML"/> method saves the Writer Metadata Document that contains a writer's state information to a specified string. 
		/// This string can be saved as part of a backup operation.
		/// </summary>
		/// <returns>The Writer Metadata Document that contains a writer's state information.</returns>
		String^ SaveAsXML();

		/// <summary>
		/// The <see cref="BackupSchema"/> property is examined by a requester to determine from the 
		/// Writer Metadata Document the types of backup operations that a given writer can participate in.
		/// </summary>
		property VssBackupSchema BackupSchema { VssBackupSchema get(); }

		/// <summary>
		/// The alternate location mappings of the file sets.
		/// </summary>
		/// <value>A read-only list containing the alternate location mappings of the file sets.</value>
		property IList<VssWMFileDescription^>^ AlternateLocationMappings { IList<VssWMFileDescription^>^ get(); }

		/// <summary>
		/// Information about how a writer wants its data to be restored.
		/// </summary>
		property VssWMRestoreMethod^ RestoreMethod { VssWMRestoreMethod^ get(); }
		
		/// <summary>
		/// Obtains the Writer Metadata Documents the components supported by this writer.
		/// </summary>
		/// <value>the Writer Metadata Documents the components supported by this writer.</value>
		property IList<VssWMComponent^>^ Components { IList<VssWMComponent^>^ get(); }

		/// <summary>Information about files that have been explicitly excluded from backup.</summary>
		/// <value>a read-only list containing information about files that have been explicitly excluded from backup.</value>
		property IList<VssWMFileDescription^>^ ExcludeFiles { IList<VssWMFileDescription^>^ get(); }

		/// <summary>The instance identifier of the writer</summary>
		property Guid InstanceId { Guid get(); }

		/// <summary>The class ID of the writer</summary>
		property Guid WriterId { Guid get(); }

		/// <summary>A string specifying the name of the writer</summary>
		property String^ WriterName { String^ get(); }

		/// <summary>A <see cref="VssUsageType"/> enumeration value indicating how the data managed by the writer is used on the host system.</summary>
		property VssUsageType Usage { VssUsageType get(); }

		/// <summary>A <see cref="VssSourceType"/> enumeration value indicating the type of data managed by the writer.</summary>
		property VssSourceType Source { VssSourceType get(); }
	internal:
		static VssExamineWriterMetadata^ Adopt(IVssExamineWriterMetadata *ewm);
	private:
		VssExamineWriterMetadata(IVssExamineWriterMetadata *examineWriterMetadata);
		IVssExamineWriterMetadata *mExamineWriterMetadata;
		void Initialize();

		Guid mInstanceId;
		Guid mWriterId;
		String^ mWriterName;
		VssUsageType mUsage;
		VssSourceType mSource;
		IList<VssWMFileDescription^> ^mExcludeFiles;
		IList<VssWMComponent^> ^mComponents;
		VssWMRestoreMethod^ mRestoreMethod;
		IList<VssWMFileDescription^>^ mAlternateLocationMappings;
	};
}
} }