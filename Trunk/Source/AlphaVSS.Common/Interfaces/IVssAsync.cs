using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
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
        /// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="VssAsync"/> object.</para>
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
        /// <para>Additional return values can be returned, but depend on the return codes of the method that initially returned the <see cref="VssAsync"/> object.</para>
        /// </returns>
        VssError QueryStatus();

        /// <summary>
        /// The Wait method waits until an incomplete asynchronous operation finishes.
        /// </summary>
        void Wait();
    }
}
