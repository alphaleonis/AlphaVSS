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

using System;
namespace Alphaleonis.Win32.Vss
{
	/// <summary>
	/// Represenation of the status for a specific writer.
	/// </summary>
	/// <remarks>This class acts as a container for the information returned from 
	/// <see href="http://msdn.microsoft.com/en-us/library/aa382680(VS.85).aspx">IVssBackupComponents.GetWriterStatus</see> in the original
	/// VSS API</remarks>
	public class VssWriterStatusInfo
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="VssWriterStatusInfo"/> class.
        /// </summary>
        /// <param name="instanceId">The instance id.</param>
        /// <param name="writerId">The writer id.</param>
        /// <param name="writerName">Name of the writer.</param>
        /// <param name="state">The state.</param>
        /// <param name="failure">The failure.</param>
        public VssWriterStatusInfo(Guid instanceId, Guid writerId, string writerName, VssWriterState state, VssWriterFailure failure)
        {
            mInstanceId = instanceId;
            mClassId = writerId;
            mName = writerName;
            mState = state;
            mFailure = failure;
        }

	    /// <summary>
		/// The instance id of the writer.
		/// </summary>
		public Guid InstanceId { get { return mInstanceId; } }
		/// <summary>
		/// The identifier of the writer class.
		/// </summary>
		public Guid ClassId { get { return mClassId; } }
		/// <summary>
		/// The name of the writer.
		/// </summary>
		public string Name { get { return mName; } }
		/// <summary>
		/// A <see cref="VssWriterState"/> value containing the state of the writer.
		/// </summary>
		public VssWriterState State { get { return mState; } }
		/// <summary>
		/// A <see cref="VssWriterFailure"/> value indicating the failure code (if any) of the writer.
		/// </summary>
		public VssWriterFailure Failure { get { return mFailure; } }
	
		private Guid mInstanceId;
        private Guid mClassId;
        private string mName;
        private VssWriterState mState;
        private VssWriterFailure mFailure;
	};
}
