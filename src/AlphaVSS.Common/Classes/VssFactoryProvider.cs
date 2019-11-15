
using System;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.IO;
using System.Threading;
#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Class that provides instances of <see cref="IVssFactory"/> which in turn is used as the entry point to accessing the various VSS functions.
   /// </summary>
   /// <remarks>
   /// <para>This class has a default implementation in <see cref="VssFactoryProvider.Default"/> which is normally used. It will load the correct platform specific dll, assuming it is located in the same
   /// directory as the <c>AlphaVSS.Common</c> assembly.</para>
   /// <para>To modify this dynamic assembly loading, you can implement <see cref="IVssAssemblyResolver"/> and pass that as a constructor argument to this class.</para>
   /// </remarks>
   public class VssFactoryProvider : IVssFactoryProvider
   {
      private readonly IVssAssemblyResolver m_resolver;
      private readonly Lazy<Assembly> m_assembly;

      private static string PlatformSpecificAssemblyShortName => $"AlphaVSS.{(Environment.Is64BitOperatingSystem ? "x64" : "x86")}";
      private static AssemblyName PlatformSpecificAssemblyName => new AssemblyName(String.Format(CultureInfo.InvariantCulture, "{0}, Version={1}, Culture=neutral, PublicKeyToken=959d3993561034e3", PlatformSpecificAssemblyShortName, Assembly.GetExecutingAssembly().GetName().Version.ToString()));

      /// <summary>
      /// The default instance of <see cref="IVssFactoryProvider"/>. This attempts to load the platform specifiec AlphaVSS assembly from the same 
      /// directory that the AlphaVSS.Common assembly is located in.
      /// </summary>
      public static readonly IVssFactoryProvider Default = new VssFactoryProvider(new DefaultVssAssemblyResolver());

      /// <summary>
      /// Creates a new instance of <see cref="VssFactoryProvider"/>.
      /// </summary>
      /// <param name="resolver">The assembly resolver which will be used to load the platform specific AlphaVSS assembly.</param>
      public VssFactoryProvider(IVssAssemblyResolver resolver)
      {
         m_resolver = resolver ?? throw new ArgumentNullException(nameof(resolver), $"{nameof(resolver)} is null.");
         m_assembly = new Lazy<Assembly>(LoadAssembly, LazyThreadSafetyMode.ExecutionAndPublication);
      }

      /// <summary>
      /// Creates a new instance of <see cref="IVssFactory"/> corresponding to the current platform by loading the appropriate platform specifiec assembly, 
      /// and instantiating the correct implementation of <see cref="IVssFactory"/>.
      /// </summary>
      /// <returns>An instance of <see cref="IVssFactory"/>.</returns>
      public IVssFactory GetVssFactory()
      {
         return (IVssFactory)m_assembly.Value.CreateInstance("Alphaleonis.Win32.Vss.VssFactory");
      }

      private Assembly LoadAssembly() => m_resolver.LoadAssembly(PlatformSpecificAssemblyName);

      private class DefaultVssAssemblyResolver : IVssAssemblyResolver
      {
         public Assembly LoadAssembly(AssemblyName assemblyName)
         {
            if (assemblyName == null)
               throw new ArgumentNullException(nameof(assemblyName), $"{nameof(assemblyName)} is null.");

#if NETCOREAPP
            string assemblyFileName = assemblyName.Name + ".dll";
            string fullAssemblyFileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assemblyFileName);
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(fullAssemblyFileName);
#elif NETFRAMEWORK
         return Assembly.Load(assemblyName);
#endif
         }
      }
   }
}
