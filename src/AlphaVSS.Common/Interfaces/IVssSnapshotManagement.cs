

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// The <see cref="IVssSnapshotManagement"/> interface provides a method that returns an interface to further configure a shadow copy provider.
   /// </summary>
   public interface IVssSnapshotManagement
   {
      /// <summary>
      ///     Gets an instance of the differential software snapshot management interface to further configure the system provider.
      /// </summary>
      /// <remarks>
      ///     <para>
      ///         <note>
      ///             <para>
      ///                 <b>Windows XP:</b> This method is not supported until Windows 2003.
      ///             </para>
      ///         </note>
      ///     </para>
      /// </remarks>
      /// <returns>An instance of the differential software snapshot management interface to further configure the system provider.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      IVssDifferentialSoftwareSnapshotManagement GetDifferentialSoftwareSnapshotManagementInterface();

      /// <summary>
      /// Returns the current minimum size of the shadow copy storage area.
      /// </summary>
      /// <remarks>
      ///     <para>
      ///         The shadow copy storage area minimum size is a per-computer setting. Prior to Windows Server 2003 Service Pack 1 (SP1), this 
      ///         was fixed at 100 MB. With Windows Server 2003 SP1, the shadow copy storage area has a minimum size of 300 MB and can be 
      ///         increased in 300 MB increments up to 3000 MB (3 GB). This setting is stored in the <c>MinDiffAreaFileSize</c> value of type 
      ///         <c>REG_DWORD</c> in <c>HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\VolSnap</c>  (the value is the size, in MB).        
      ///     </para>
      ///     <para>
      ///         <note>
      ///             <para>
      ///                 <b>Windows XP and Windows 2003:</b> This method is not supported until Windows 2003 SP1.
      ///             </para>
      ///         </note>
      ///     </para>
      /// </remarks>
      /// <returns>The current minimum size of the shadow copy storage area.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      long GetMinDiffAreaSize();
   }
}
