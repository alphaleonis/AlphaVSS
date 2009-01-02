
namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Enumeration used to discriminate between the named windows versions.
    /// </summary>
    /// <remarks>
    ///     The values of the enumeration are ordered so a later released operating system version 
    ///     has a higher number, so comparisons between named versions are meaningful.
    /// </remarks>
    public enum OSVersionName
    {
        /// <summary>
        /// Windows 2000 (Server or Professional)
        /// </summary>
        Windows2000 = 0,
        /// <summary>
        /// Windows XP
        /// </summary>
        WindowsXP = 1,
        /// <summary>
        /// Windows Server 2003
        /// </summary>
        WindowsServer2003 = 2,
        /// <summary>
        /// Windows Vista
        /// </summary>
        WindowsVista = 3,
        /// <summary>
        /// Windows Server 2008
        /// </summary>
        WindowsServer2008 = 4,
        /// <summary>
        /// Unknown operating system
        /// </summary>
        Unknown = 0xffff
    }
}
