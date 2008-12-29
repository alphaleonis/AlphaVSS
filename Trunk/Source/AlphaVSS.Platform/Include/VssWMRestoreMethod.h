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

#include "VssWriterRestore.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// Represents information about how a writer wants its data to be restored.
	/// </summary>
	/// <remarks>This class is a container for the data returned by 
	/// <see href="http://msdn.microsoft.com/en-us/library/aa384206(VS.85).aspx">IVssExamineWriterMetadata::GetRestoreMethod</see>
	/// in the VSS API.</remarks>
	public ref class VssWMRestoreMethod
	{
	public:
		/// <summary>Releases any resources aquired by this instance</summary>
		~VssWMRestoreMethod();

		/// <summary>
		/// A <see cref="VssRestoreMethod"/> value that specifies file overwriting, the use of alternate locations specifying the method that 
		/// will be used in the restore operation.
		/// </summary>
		property VssRestoreMethod Method { VssRestoreMethod get(); }

		/// <summary>
		/// If the value of <see cref="Method" /> is <see dref="F:Alphaleonis.Win32.Vss.VssRestoreMethod.StopRestoreStart" />, a pointer to a string containing the name 
		/// of the service that is started and stopped. Otherwise, the value is <see langword="null"/>.
		/// </summary>
		property String^ Service { String^ get(); }

		/// <summary>
		/// Pointer to the URL of an HTML or XML document describing to the user how the restore is to be performed. The value may be <see langword="null" />.
		/// </summary>
		property String^ UserProcedure { String^ get(); }

		/// <summary>
		/// A <see cref="VssWriterRestore" /> value specifying whether the writer will be involved in restoring its data.
		/// </summary>
		property VssWriterRestore WriterRestore { VssWriterRestore get(); }

		/// <summary>A <see langword="bool"/> indicating whether a reboot will be required after the restore operation is complete.</summary>
		/// <value><see langword="true"/> if a reboot will be required and <see langword="false"/> if it will not.</value>
		property bool RebootRequired { bool get(); }

		/// <summary>The number of alternate mappings associated with the writer.</summary>
		property int MappingCount { int get(); }
	internal:
		VssWMRestoreMethod(VssRestoreMethod restoreMethod, String^ service, String^ userProcedure, VssWriterRestore writerRestore, bool rebootRequired, int mappings);
	private:
		VssRestoreMethod mRestoreMethod;
		String^ mService;
		String^ mUserProcedure;
		VssWriterRestore mWriterRestore;
		bool mRebootRequired;
		int mMappingCount;
	};
}
} }