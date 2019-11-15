
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using AlphaShadow.Commands;
using Alphaleonis.Win32.Vss;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AlphaShadow
{
   class Program
   {
      private Command[] m_nonHelpCommands = 
      { 
         new QueryCommand(),
         new CreateCommand(),
         new ListWriterStatusCommand(),
         new ListWriterMetadataCommand(),
         new DeleteCommand(),
         new RevertCommand(),
         new ExposeCommand(),
         new ImportCommand(), 
         new RestoreCommand(),
      };

      private Command[] m_commands;


      private IUIHost Host { get; set; }

      private void PrintHeader()
      {
         Version version = Assembly.GetExecutingAssembly().GetName().Version;
         string title = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).Single()).Title;
         string description = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).Single()).Description;
         string copyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).Single()).Copyright;
         
         Host.WriteHeader("{0} v{1} - {2}", title, version.ToString(2), description);
         Host.WriteHeader(copyright);
         Host.WriteLine();
      }

      public Program(IUIHost host)
      {
         if (host == null)
            throw new ArgumentNullException(nameof(host), $"{nameof(host)} is null.");

         Host = host;

         m_commands = new Command[] { new HelpCommand(m_nonHelpCommands) }.Concat(m_nonHelpCommands).ToArray();
      }

      private async Task<int> RunAsync(string[] args)
      {
         PrintHeader();

         string commandName;
         if (args == null || args.Length == 0 || 
            new [] { "/?", "-?", "/h", "-h", "/help", "-help" }.Any(h => h.Equals(args[0])))
         {
            commandName = "help";
         }
         else
         {
            commandName = args[0];
         }

         try
         {
            Command command = GetCommand(commandName);
            command.Initialize(Host, args.Skip(1));
            await command.RunAsync().ConfigureAwait(false);
         }
         catch (CommandAbortedException)
         {
            Host.WriteError("Execution aborted.");
         }
         catch (Exception ex)
         {
            Host.WriteError("{0}", ex.Message);
         }

         Host.WriteLine();

         return 0;
      }

      private Command GetCommand(string commandName)
      {
         Command result = m_commands.FirstOrDefault(cmd => cmd.Name.Equals(commandName, StringComparison.OrdinalIgnoreCase));
         if (result == null)
            throw new ArgumentException(String.Format("Unknown command {0}.", commandName));
         return result;
      }

      [MTAThread]
      static async Task<int> Main(string[] args)
      {
         // WARNING: This call will fail if debugging this application using the Visual Studio Hosting process. It may 
         //          also fail under other conditions. It is used to allow communication with all writers. If missing 
         //          for example the System Writer will not show up in the IVssBackupComponents.WriterMetadata collection.
         //          Posts on the internet seems to suggest that the only way to reliably set this is to use a custom CLR host, which 
         //          is out of the scope for this sample.
         Marshal.ThrowExceptionForHR(NativeMethods.CoInitializeSecurity(IntPtr.Zero, -1, IntPtr.Zero, IntPtr.Zero, NativeMethods.RpcAuthnLevel.None, NativeMethods.RpcImpLevel.Impersonate, IntPtr.Zero, NativeMethods.EoAuthnCap.None, IntPtr.Zero));
         
         Program program = new Program(new ConsoleHost());
         return await program.RunAsync(args).ConfigureAwait(false);
      }

   }

   class NativeMethods
   {
      public enum RpcAuthnLevel
      {
         Default = 0,
         None = 1,
         Connect = 2,
         Call = 3,
         Pkt = 4,
         PktIntegrity = 5,
         PktPrivacy = 6
      }

      public enum RpcImpLevel
      {
         Default = 0,
         Anonymous = 1,
         Identify = 2,
         Impersonate = 3,
         Delegate = 4
      }

      public enum EoAuthnCap
      {
         None = 0x00,
         MutualAuth = 0x01,
         StaticCloaking = 0x20,
         DynamicCloaking = 0x40,
         AnyAuthority = 0x80,
         MakeFullSIC = 0x100,
         Default = 0x800,
         SecureRefs = 0x02,
         AccessControl = 0x04,
         AppID = 0x08,
         Dynamic = 0x10,
         RequireFullSIC = 0x200,
         AutoImpersonate = 0x400,
         NoCustomMarshal = 0x2000,
         DisableAAA = 0x1000
      }

      [System.Runtime.InteropServices.DllImport("ole32.dll")]
      public static extern int CoInitializeSecurity(IntPtr pVoid, int
          cAuthSvc, IntPtr asAuthSvc, IntPtr pReserved1, RpcAuthnLevel level,
          RpcImpLevel impers, IntPtr pAuthList, EoAuthnCap dwCapabilities, IntPtr
          pReserved3);
   }
}

