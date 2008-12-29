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
#include <VsWriter.h>
#include <vsBackup.h>

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// An instance of the <see cref="VssWMFileDescription"/> class provides detailed information about a file or set of files (a file set)
	/// </summary>
	/// <remarks>VSS API: <c>IVssWMFiledesc</c></remarks>
	public ref class VssWMFileDescription 
	{
	public:
		/// <summary>
		/// Destructor.
		/// </summary>
		~VssWMFileDescription();

		/// <summary>
		/// Obtains the alternate backup location of the component files.
		/// </summary>
		property String^ AlternateLocation { String^ get(); }		

		/// <summary>
		/// Obtains the file backup specification for a file or set of files.
		/// </summary>
		/// <remarks><note><b>Windows XP:</b> This value is not supported in Windows XP</note></remarks>
		property VssFileSpecificationBackupType BackupTypeMask { VssFileSpecificationBackupType get(); }

		/// <summary>
		/// Obtains the file specification for the list of files provided.
		/// </summary>
		property String^ FileSpecification { String^ get(); };
		/// <summary>
		/// Obtains the fully qualified directory path for the list of files provided.
		/// </summary>
		property String^ Path { String^ get(); };
		/// <summary>
		/// Determines whether only files in the root directory or files in the entire directory hierarchy are considered for backup.
		/// </summary>
		/// <remarks>VSS API reference: <c>IVssWMFiledesc::GetRecursive()</c></remarks>
		property bool IsRecursive { bool get(); };
	
	internal:
		/// <summary>
		/// Adopts the IVssWMFiledesc instance passed to it, initializes the contents of this 
		/// class and releases the IVssWMFiledesc instance.  The IVssWMFiledesc instance is 
		/// released even if an exception is thrown during the creation of this object.
		/// </summary>
		static VssWMFileDescription^ Adopt(IVssWMFiledesc *vssWMFiledesc);
	
	private:
		VssWMFileDescription(IVssWMFiledesc *vssWMFiledesc);

		String^ mAlternateLocation;
		VssFileSpecificationBackupType mBackupTypeMask;
		String^ mFileSpecification;
		String^ mPath;
		bool mRecursive;
	};
}
} }