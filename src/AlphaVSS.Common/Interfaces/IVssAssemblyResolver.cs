
using System;
using System.Reflection;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// An interface providing methods for <see cref="IVssFactoryProvider"/> to load the platform specific assembly required for instantiating
   /// <see cref="IVssFactory"/>.
   /// </summary>   
   public interface IVssAssemblyResolver
   {
      /// <summary>
      /// Loads the assembly with the specified <paramref name="assemblyName"/>.
      /// </summary>
      /// <param name="assemblyName">The full name of the assembly to load.</param>
      /// <returns>The loaded assembly. Must not return <see langword="null"/>.</returns>
      Assembly LoadAssembly(AssemblyName assemblyName);
   }
}
