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
using System.Text;

namespace Alphaleonis.Win32.Vss
{
	/// <summary>
	/// Represents the status of an asynchronous operation performed by the VSS framework.
	/// </summary>
	public interface IVssAsyncResult : IAsyncResult, IDisposable
	{
		/// <summary>
		/// Cancels an incomplete asynchronous operation.
		/// </summary>
		void Cancel();
		/// <summary>Current status of the work</summary>
		UInt32 QueryStatus();
		/// <summary>Wait a period of time.  Result is true only if completed successfully.</summary>
		bool Wait(TimeSpan TimeOut);

	}

	/// <summary>Provides extensions</summary>
	public static partial class Extensions
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
		public static TimeSpan
			TimeOut_Default = new TimeSpan(0, 5, 0);

		/// <summary>Provides an awaitable process while running async operations</summary>
		/// <param name="Async">Since this is an extension, this is the object being extended</param>
		/// <param name="TimeOut">Maximum time to wait (default uses the static field "TimeOut_Default")</param>
		/// <returns>If operation succeded before the timeout occured, true.  Otherwise false.</returns>
		async static public System.Threading.Tasks.Task<bool> WaitAsync(this IVssAsyncResult Async, TimeSpan TimeOut)
		{
			if (Async == null || Async.IsCompleted)
				return true;
			if (TimeOut.Milliseconds <= 0)
				TimeOut = TimeOut_Default;
			System.Threading.Tasks.Task<bool>
				tsk = new System.Threading.Tasks.Task<bool>(() => { return Async.Wait(TimeOut); });
			if (tsk.Status == System.Threading.Tasks.TaskStatus.Created)
				tsk.Start();

			await tsk;

			if (tsk.Exception != null)
				throw tsk.Exception;

			return tsk.Result;
		}
	}
}
