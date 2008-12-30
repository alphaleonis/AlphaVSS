using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    ///     <para>
    ///         The <see cref="IVssAsync"/> interface is returned to calling applications by methods that initiate asynchronous operations, 
    ///         which run in the background and typically require a long time to complete.
    ///     </para>
    ///     <para>
    ///         The <see cref="IVssAsync"/> interface permits an application to monitor and control an asynchronous operation by waiting 
    ///         on its completion, querying its status, or canceling it.
    ///     </para>
    ///     <para>
    ///         The calling application is responsible for calling <see cref="M:Alphaleonis.Win32.Vss.IVssAsync.Dispose"/> to release the resources held 
    ///         by the returned <see cref="IVssAsync"/> interface when it is no longer needed.
    ///     </para>
    ///     <para>
    ///         The following methods return an <see cref="IVssAsync"/> interface:
    ///         <list type="bullet">
    ///             <item><description><see cref="IVssBackupComponents.BackupComplete"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.DoSnapshotSet"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.GatherWriterMetadata"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.GatherWriterStatus"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.ImportSnapshots"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.PostRestore"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.PrepareForBackup"/></description></item>
    ///             <item><description><see cref="IVssBackupComponents.PreRestore"/></description></item>
    ///         </list>
    ///     </para>
    /// </summary>
    public interface IVssAsync : IDisposable
    {
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
        /// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="IVssAsync"/> object.</para>
        /// </returns>
        /// <remarks>If an operation has completed unsuccessfully before <see cref="Cancel"/> was called, then <see cref="Cancel"/> throws the error that 
        /// operation encountered.</remarks>
        VssError Cancel();

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
        /// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="IVssAsync"/> object.</para>
        /// </returns>
        VssError QueryStatus();

        /// <summary>
        /// The Wait method waits until an incomplete asynchronous operation finishes.
        /// </summary>
        void Wait();
    }
}
