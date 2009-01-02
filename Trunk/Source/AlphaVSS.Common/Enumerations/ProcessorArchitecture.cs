
namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// Enumeration used by <see cref="OperatingSystemInfo"/> to indicate the current
    /// processor architecture for which the operating system is targeted and running.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ProcessorArchitecture : ushort
    {
        /// <summary>
        /// The system is running a 32-bit version of Windows.
        /// </summary>
        X86 = 0x00,
        /// <summary>
        /// The system is running an Itanium processor.
        /// </summary>
        IA64 = 0x06,
        /// <summary>
        /// The system is running a 64-bit version of Windows.
        /// </summary>
        X64 = 0x09,
        /// <summary>
        /// Unknown architecture.
        /// </summary>
        Unknown = 0xFFFF,
    }
}
