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
        public VssWriterStatusInfo(Guid instanceId, Guid writerId, string writerName, VssWriterState state, VssError failure)
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
		/// A <see cref="VssError"/> value indicating the failure code (if any) of the writer.
		/// </summary>
        /// <remarks>
        ///     <para>
        ///         The following are the supported values for <see cref="Failure"/>:
        ///         <list type="table">
        ///             <listheader>
        ///                 <term>Value</term>
        ///                 <description>Meaning</description>
        ///             </listheader>
        ///             <item>
        ///                 <term><see cref="VssError.Success"/></term>
        ///                 <description>The writer was successful.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterErrorInconsistentSnapshot"/></term>
        ///                 <description>The shadow copy contains only a subset of the volumes needed by the writer to correctly back up the application component.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterOutOfResources"/></term>
        ///                 <description>The writer ran out of memory or other system resources. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterTimeout"/></term>
        ///                 <description>The writer operation failed because of a time-out between the Freeze and Thaw events. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterErrorRetryable"/></term>
        ///                 <description>The writer failed due to an error that would likely not occur if the entire backup, restore, or shadow copy creation process was restarted. The recommended way to handle this error code is to wait ten minutes and then repeat the operation, up to three times.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterErrorNonRetryable"/></term>
        ///                 <description>The writer operation failed because of an error that might recur if another shadow copy is created.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterNotResponding"/></term>
        ///                 <description>The writer is not responding.</description>
        ///             </item>
        ///             <item>
        ///                 <term><see cref="VssError.WriterStatusNotAvailable"/></term>
        ///                 <description>
        ///                     <para>
        ///                         The writer status is not available for one or more writers. A writer may have reached the maximum number of available backup and restore sessions.
        ///                     </para>                    
        ///                     <para>
        ///                         <b>Windows Vista, Windows Server 2003 and Windows XP:</b> This value is not supported.
        ///                     </para>
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
		public VssError Failure { get { return mFailure; } }
	
		private Guid mInstanceId;
        private Guid mClassId;
        private string mName;
        private VssWriterState mState;
        private VssError mFailure;
	};
}
