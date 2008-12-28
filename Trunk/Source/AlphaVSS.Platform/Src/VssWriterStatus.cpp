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
#include "StdAfx.h"
#include "VssWriterStatus.h"


namespace Alphaleonis { namespace Win32 { namespace Vss
{
	VssWriterStatus::VssWriterStatus(Guid instanceId, Guid writerId, String^ writerName, VssWriterState state, VssWriterFailure failure)
		: mInstanceId(instanceId), mClassId(writerId), mName(writerName), mState(state), mFailure(failure)
	{
	}

	Guid VssWriterStatus::InstanceId::get()
	{
		return mInstanceId;
	}

	Guid VssWriterStatus::ClassId::get()
	{
		return mClassId;
	}

	String^ VssWriterStatus::Name::get()
	{
		return mName;
	}
	
	VssWriterState VssWriterStatus::State::get()
	{
		return mState;
	}

	VssWriterFailure VssWriterStatus::Failure::get()
	{
		return mFailure;
	}
	
}
} }

