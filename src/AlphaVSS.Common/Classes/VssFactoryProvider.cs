using System;
using System.Reflection;
using System.Text;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Linq;
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
      private static AssemblyName PlatformSpecificAssemblyName => new AssemblyName($"{PlatformSpecificAssemblyShortName}, Version={Assembly.GetExecutingAssembly().GetName().Version}, Culture=neutral, PublicKeyToken=959d3993561034e3");

      /// <summary>
      /// The default instance of <see cref="IVssFactoryProvider"/>. This attempts to load the platform specific AlphaVSS assembly from the same 
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
      /// Creates a new instance of <see cref="IVssFactory"/> corresponding to the current platform by loading the appropriate platform specific assembly, 
      /// and instantiating the correct implementation of <see cref="IVssFactory"/>.
      /// </summary>
      /// <returns>An instance of <see cref="IVssFactory"/>.</returns>
      /// <exception cref="UnsupportedOperatingSystemException">This exception is thrown if running as a 32-bit process on a 64-bit operating system.</exception>
      public IVssFactory GetVssFactory()
      {         
         return (IVssFactory)m_assembly.Value.CreateInstance("Alphaleonis.Win32.Vss.VssFactory");
      }

      private Assembly LoadAssembly()
      {
         Log($"Request loading AlphaVSS platform specific assembly...");
         Log($"OS Version: {Environment.OSVersion}");
         Log($"Is64BitOperatingSystem: {Environment.Is64BitOperatingSystem}");
         Log($"Is64BitProcess: {Environment.Is64BitProcess}");
         if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            throw new UnsupportedOperatingSystemException($"Request to load AlphaVSS from a 32-bit process on a 64-bit operating system. This combination is not supported.");

         return m_resolver.LoadAssembly(PlatformSpecificAssemblyName);
      }

      private static void Log(string message)
      {
         Trace.WriteLine(message, "AlphaVSS");
      }

      class DefaultVssAssemblyResolver : IVssAssemblyResolver
      {
         public Assembly LoadAssembly(AssemblyName assemblyName)
         {
            if (assemblyName == null)
               throw new ArgumentNullException(nameof(assemblyName), $"{nameof(assemblyName)} is null.");

#if NETCOREAPP
            Log($"Attempting to locate \"{assemblyName.FullName}\"");
            // In .NET Core the native binaries are placed under a runtimes\{rid}\native folder, which is where the platform 
            // specific binaries are placed in AlphaVSS nuget packages. However the location after building is different if
            // building a self-contained executable.  So we use the NATIVE_DLL_SEARCH_DIRECTORIES property from the
            // AppContext to determine which directories to search.  This property is documented here:
            // https://docs.microsoft.com/en-us/dotnet/core/dependency-loading/default-probing
            string nativeSearchDirectories = AppContext.GetData("NATIVE_DLL_SEARCH_DIRECTORIES") as string;

            string[] searchDirectories;
            if (nativeSearchDirectories == null)
            {
               // If no such property was found, just search the directory of this assembly.
               searchDirectories = new string[] { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };
               Log($"Found no native search directories in AppContext, searching current directory \"{searchDirectories}\" only.");
            }
            else
            {
               // On non-windows systems, the ':' separator is used, but since we only run on windows we don't need to 
               // take that into consideration.
               searchDirectories = nativeSearchDirectories.Split(';', StringSplitOptions.RemoveEmptyEntries);
               Log($"Using native search directories: {String.Join(", ", searchDirectories.Select(s => $"\"{s}\""))}");
            }

            string assemblyFileName = assemblyName.Name + ".dll";

            // Attempt to locate a dll-file with the same short name in each of the search directories, and load the
            // first one found. 
            foreach (var searchDirectory in searchDirectories)
            {
               string fullAssemblyFileName = Path.Combine(searchDirectory, assemblyFileName);
               if (File.Exists(fullAssemblyFileName))
               {
                  Log($"Found \"{fullAssemblyFileName}\"");
                  try
                  {
                     Log($"Loading \"{fullAssemblyFileName}\".");
                     var result = AssemblyLoadContext.Default.LoadFromAssemblyPath(fullAssemblyFileName);
                     Log($"Loaded \"{result.FullName}\" from \"{result.Location}\".");
                     return result;
                  }
                  catch (Exception ex)
                  {
                     Log($"Error loading \"{fullAssemblyFileName}\"; {ex}");
                     throw;
                  }
               }
               else
               {
                  Log($"File not found \"{fullAssemblyFileName}\"");
               }
            }

            throw new FileNotFoundException($"Failed to find assembly \"{assemblyName.FullName}\"");
#elif NETFRAMEWORK
            Log($"Attempting to load assembly \"{assemblyName.FullName}\"");
            try
            {
               var result = Assembly.Load(assemblyName);
               Log($"Loaded \"{result.FullName}\" from \"{result.Location}\".");
               return result;
            }
            catch (Exception ex)
            {
               Log($"Error loading \"{assemblyName.FullName}\"; {ex}");
               throw;
            }
#endif
         }
      }
   }
}
