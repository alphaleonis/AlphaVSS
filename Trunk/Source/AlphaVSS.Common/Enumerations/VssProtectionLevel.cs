
namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Defines the set of volume shadow copy protection levels.
    /// </summary>
    public enum VssProtectionLevel
    {
        /// <summary>
        ///     Specifies that I/O to the original volume must be maintained at the expense of shadow copies. 
        ///     This is the default protection level. Shadow copies might be deleted if both of the following conditions occur:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 A write to the original volume occurs.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 The integrity of the shadow copy cannot be maintained for some reason, such as a failure to 
        ///                 write to the shadow copy storage area or a failure to allocate sufficient memory.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        OriginalVolume = 0,
        /// <summary>
        ///     Specifies that shadow copies must be maintained at the expense of I/O to the original volume. 
        ///     All I/O to the original volume will fail if both of the following conditions occur:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 A write to the original volume occurs.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 The corresponding write to the shadow copy storage area cannot be completed for some reason, 
        ///                 such as a failure to write to the shadow copy storage area or a failure to allocate sufficient memory.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        Snapshot = 1,
    }
}
