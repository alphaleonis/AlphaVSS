
using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Interface for a provider that provides the correct implementation of <see cref="IVssFactory"/> which is used as the entry point to accessing the various VSS functions.
   /// </summary>
   /// <remarks>
   /// <para>The default implementation of this is <see cref="VssFactoryProvider"/>. </para>
   /// <para><note type="note">There is no known scenario in which a custom implementation of this interface is required.</note></para>
   /// </remarks>
   /// <seealso cref="VssFactoryProvider.Default"/>
   public interface IVssFactoryProvider
   {
      /// <summary>
      /// Gets the correct implementation of <see cref="IVssFactory"/> for the current platform.
      /// </summary>
      /// <returns>An instance of <see cref="IVssFactory"/>.</returns>
      /// <exception cref="UnsupportedOperatingSystemException">This exception is thrown if running as a 32-bit process on a 64-bit operating system.</exception>
      IVssFactory GetVssFactory();
   }
}
