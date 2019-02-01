
using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Globalization;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     The <see cref="VssUtils"/> class is a static utility class for accessing the platform specific
   ///     instances of the various VSS interfaces in a platform-independent manner.
   /// </summary>
   /// <remarks>
   ///     Use the <see cref="M:Alphaleonis.Win32.Vss.VssUtils.LoadImplementation"/> under normal circumstances to load
   ///     the correct assembly and create an instance of <see cref="IVssImplementation"/> from that assembly. If you have 
   ///     specific requirements on how the assembly should be loaded, or the instance created you are not required to use 
   ///     these methods but can use the methods in this class for accessing the suggested assembly name to load, and load it manually.
   ///     In this case you need to create an instance of the class called <c>Alphaleonis.Win32.Vss.VssImplementation</c> from the platform specific
   ///     assembly. This class implements the <see cref="IVssImplementation"/> interface, and has a public parameterless constructor.
   /// </remarks>
   public static class VssUtils
   {
      /// <summary>
      /// Gets the short name of the platform specific assembly for the platform on which the assembly 
      /// is currently executing.
      /// </summary>
      /// <returns>the short name of the platform specific assembly for the platform on which the assembly 
      /// is currently executing.</returns>
      /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      public static string GetPlatformSpecificAssemblyShortName()
      {
         StringBuilder result = new StringBuilder("AlphaVSS.");

         if (IntPtr.Size == 8)
            result.Append("x64");
         else
            result.Append("x86");         

         return result.ToString();
      }

      /// <summary>
      /// Gets the full name of the platform specific assembly for the platform on which the assembly is currently executing.
      /// </summary>
      /// <returns>The full name of the platform specific assembly for the platform on which the assembly is currently executing.</returns>
      /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
      public static AssemblyName GetPlatformSpecificAssemblyName()
      {
         return new AssemblyName(String.Format(CultureInfo.InvariantCulture, "{0}, Version={1}, Culture=neutral, PublicKeyToken=959d3993561034e3", GetPlatformSpecificAssemblyShortName(), Assembly.GetExecutingAssembly().GetName().Version.ToString()));
      }

      /// <summary>
      /// Loads the assembly containing the correct implementation of the <see cref="IVssImplementation"/> interface
      /// for the operating system on which the assembly is currently executing. 
      /// </summary>
      /// <overloads>
      /// Loads the assembly containing the correct implementation of the <see cref="IVssImplementation"/> interface
      /// for the operating system on which the assembly is currently executing, optionally allowing the specification
      /// of an <see cref="AppDomain"/> into which to load the assembly.
      /// </overloads>
      /// <remarks>
      ///     <para>
      ///         The assembly will be loaded into the same <see cref="AppDomain"/> as the calling assembly.
      ///     </para>
      ///     <para>
      ///         The assemblies are loaded using strong name lookup. They need to be present in the code base directory
      ///         of the executing assembly, or installed in the GAC for the lookup to succeed.
      ///     </para>
      /// </remarks>
      /// 
      /// <returns>An newly created instance of <see cref="IVssImplementation"/> suitable for the 
      /// operating system on which the assembly is currently executing.</returns>
      /// <exception cref="UnsupportedOperatingSystemException">The operating system could not be detected or is unsupported.</exception>
      public static IVssImplementation LoadImplementation()
      {
         Assembly assembly = Assembly.Load(GetPlatformSpecificAssemblyName());
         return (IVssImplementation)assembly.CreateInstance("Alphaleonis.Win32.Vss.VssImplementation");
      }
   }
}
