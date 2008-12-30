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
#include "VssWMComponent.h"

using namespace System::Text;
using namespace System::Collections::Generic;

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	private ref class VssExamineWriterMetadata : IDisposable, IVssExamineWriterMetadata
	{
	public:
		~VssExamineWriterMetadata();
		!VssExamineWriterMetadata();

		virtual bool LoadFromXML(String^ xml);
		virtual String^ SaveAsXml();
		property VssBackupSchema BackupSchema { virtual VssBackupSchema get(); }

		property IList<VssWMFileDescription^>^ AlternateLocationMappings { virtual IList<VssWMFileDescription^>^ get(); }

		property VssWMRestoreMethod^ RestoreMethod { virtual VssWMRestoreMethod^ get(); }
		property IList<IVssWMComponent^>^ Components { virtual IList<IVssWMComponent^>^ get(); }

		property IList<VssWMFileDescription^>^ ExcludeFiles { virtual IList<VssWMFileDescription^>^ get(); }

		property Guid InstanceId { virtual Guid get(); }

		property Guid WriterId { virtual Guid get(); }

		property String^ WriterName { virtual String^ get(); }

		property VssUsageType Usage { virtual VssUsageType get(); }

		property VssSourceType Source { virtual VssSourceType get(); }
	internal:
		static IVssExamineWriterMetadata^ Adopt(::IVssExamineWriterMetadata *ewm);
	private:
		VssExamineWriterMetadata(::IVssExamineWriterMetadata *examineWriterMetadata);
		::IVssExamineWriterMetadata *mExamineWriterMetadata;
		void Initialize();

		Guid mInstanceId;
		Guid mWriterId;
		String^ mWriterName;
		VssUsageType mUsage;
		VssSourceType mSource;
		IList<VssWMFileDescription^> ^mExcludeFiles;
		IList<IVssWMComponent^> ^mComponents;
		VssWMRestoreMethod^ mRestoreMethod;
		IList<VssWMFileDescription^>^ mAlternateLocationMappings;
	};
}
} }