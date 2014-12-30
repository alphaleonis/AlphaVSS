/* Copyright (c) 2008-2009 Peter Palotas
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
//#include "VssError.h"

namespace Alphaleonis { namespace Win32 { namespace Vss
{

	/// <summary>
	/// 	<para>
	/// 		The <see cref="VssAsync"/> class is returned to calling applications by methods that initiate asynchronous operations, which run in the 
	/// 		background and typically require a long time to complete.
	/// 	</para>
	/// 	<para>
	/// 		The <see cref="VssAsync"/> class permits an application to monitor and control an asynchronous operation by waiting on its completion, 
	/// 		querying its status, or canceling it.
	/// 	</para>
	/// 	<para>
	/// 		The <see cref="VssAsync"/> object should be disposed as soon as it is no longer needed.
	/// 	</para>
	/// </summary>
	private ref class VssAsync : IVssAsync, MarshalByRefObject
	{
	public:
		/// <summary>Destructor.</summary>
		~VssAsync();

		/// <summary>Releases resources used by the <see cref="VssAsync"/> object.</summary>
		!VssAsync();

		/// <summary>Cancels an incomplete asynchronous operation.</summary>
		/// <returns>
		/// <list type="table">
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.Success" /></term>
		/// 		<description>The asynchronous operation had been successfully cancelled.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.AsyncCancelled" /></term>
		/// 		<description>The asynchronous operation had been canceled prior to calling this method.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.AsyncFinished" /></term>
		/// 		<description>The asynchronous operation had completed prior to calling this method.</description>
		/// 	</item>
		/// </list>
		/// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="VssAsync"/> object.</para>
		/// </returns>
		/// <remarks>If an operation has completed unsuccessfully before <see cref="Cancel"/> was called, then <see cref="Cancel"/> throws the error that 
		/// operation encountered.</remarks>
		virtual VssError Cancel();

		/// <summary>The <see cref="QueryStatus"/> method queries the status of an asynchronous operation.</summary>
		/// <returns>
		/// <list type="table">
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.Success" /></term>
		/// 		<description>The asynchronous operation had been successfully canceled.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.AsyncCancelled" /></term>
		/// 		<description>The asynchronous operation had been canceled prior to calling this method.</description>
		/// 	</item>
		/// 	<item>
		/// 		<term><see dref="F:Alphaleonis.Win32.Vss.VssError.AsyncFinished" /></term>
		/// 		<description>The asynchronous operation had completed prior to calling this method.</description>
		/// 	</item>
		/// </list>
		/// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="VssAsync"/> object.</para>
		/// </returns>
		virtual VssError QueryStatus();

		/// <summary>
		/// The Wait method waits until an incomplete asynchronous operation finishes.
		/// </summary>
		virtual void Wait();
	internal:
		static VssAsync^ Adopt(::IVssAsync *vssAsync);
	private:
		VssAsync(::IVssAsync *vssAsync);
		::IVssAsync *mVssAsync;
	};
}
} }