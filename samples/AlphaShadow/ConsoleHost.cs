
using System;
using System.Collections.Generic;
using System.Linq;
using AlphaShadow.Options;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace AlphaShadow
{
   public class ConsoleHost : IUIHost
   {
      private int m_indent = 0;
      
      public ConsoleHost()
      {
         IsWordWrapEnabled = true;   
      }

      public void WriteHeader(string message, params object[] args)
      {         
         WriteLine(ConsoleColor.Cyan, WordWrap(message, args));
      }   

      public void WriteLine(string message, params object[] args)
      {
         WriteLine(Console.ForegroundColor, WordWrap(message, args));
      }

      public void WriteWarning(string message, params object[] args)
      {
         WriteMessage(ConsoleColor.Red, "Warning:", message, args);
      }

      public void WriteError(string message, params object[] args)
      {
         WriteMessage(ConsoleColor.Red, "Error:", message, args);
      }

      public void WriteVerbose(string message, params object[] args)
      {
         if (VerboseOutputEnabled)
            WriteLine(ConsoleColor.DarkGray, WordWrap(message, args));
      }

      private void WriteMessage(ConsoleColor color, string label, string message, params object[] args)
      {
         if (IsWordWrapEnabled)
         {
            int col1Width = label.Length;
            int col2Width = Math.Max(1, Console.WindowWidth - col1Width - 2);

            string text = StringFormatter.FormatInColumns(m_indent, 1,
               new StringFormatter.ColumnInfo(col1Width, label),
               new StringFormatter.ColumnInfo(col2Width, String.Format(message, args)));

            WriteLine(color, text);
         }
         else
         {
            WriteLine(color, label + " " + String.Format(message, args));
         }
      }

      private static void Write(ConsoleColor color, string message)
      {
         ConsoleColor temp = Console.ForegroundColor;
         Console.ForegroundColor = color;
         Console.Write(message);
         Console.ForegroundColor = temp;
      }

      private static void WriteLine(ConsoleColor color, string message)
      {
         ConsoleColor temp = Console.ForegroundColor;
         Console.ForegroundColor = color;
         Console.WriteLine(message);
         Console.ForegroundColor = temp;
      }

      private string WordWrap(string message, params object[] args)
      {
         if (IsWordWrapEnabled)
         {
            StringBuilder sb = new StringBuilder();
            string wrappedString = StringFormatter.WordWrap((args == null || args.Length == 0) ? message : String.Format(message, args), Console.WindowWidth - 5 - m_indent, StringFormatter.WordWrappingMethod.Greedy);
            IList<string> splitString = StringFormatter.SplitAtLineBreaks(wrappedString);
            for (int i = 0; i < splitString.Count; i++)
            {
               if (i != 0)
                  sb.AppendLine();
               sb.Append(String.Format("{0}{1}", new String(' ', m_indent), splitString[i]));
            }
            return sb.ToString();
         }
         else
         {
            return new String(' ', m_indent) + String.Format(message, args);
         }
      }

      public void WriteLine()
      {
         Console.WriteLine();
      }


      public bool VerboseOutputEnabled
      {
         get;
         set;
      }

      public bool IsWordWrapEnabled { get; set; }

      public void WriteTable(StringTable table, int columnSpacing = 3, bool addVerticalSeparation = false)
      {
         if (table == null)
            throw new ArgumentNullException(nameof(table), "table is null.");

         if (IsWordWrapEnabled)
         {
            int indent = m_indent;
            if (indent >= Console.WindowWidth - columnSpacing - 2)
               indent = 0;

            int maxWidth = Console.WindowWidth - indent;
            int col1Width = Math.Min(table.Labels.Max(text => text.Length), maxWidth / 2);
            int colSpacing = columnSpacing;
            int col2Width = maxWidth - col1Width - colSpacing - 1;

            for (int i = 0; i < table.Count; i++)
            {
               if (i > 0 && addVerticalSeparation)
                  Console.WriteLine();

               Console.WriteLine(
                  StringFormatter.FormatInColumns(indent, colSpacing,
                  new StringFormatter.ColumnInfo(col1Width, table.Labels[i]),
                  new StringFormatter.ColumnInfo(col2Width, table.Values[i])));

            }
         }
         else
         {
            for (int i = 0; i < table.Count; i++)
            {
               Console.WriteLine("{0}{1}{2}{3}", new String(' ', m_indent), table.Labels[i], new String(' ', columnSpacing), table.Values[i]);                  
            }
         }

      }

      public void PushIndent()
      {
         m_indent += 3;
      }

      public void PopIndent()
      {
         m_indent -= 3;
         if (m_indent < 0)
            m_indent = 0;
      }

      public IDisposable GetIndent()
      {
         return new Indenter(this);
      }

      private class Indenter : IDisposable
      {
         IUIHost m_host;
         public Indenter(IUIHost host)
         {
            m_host = host;
            m_host.PushIndent();
         }

         public void Dispose()
         {
            m_host.PopIndent();
         }
      }

      public void ExecCommand(string execCommand, string args)
      {
         WriteLine("- Executing command '{0}' ...", execCommand);
         WriteLine("-----------------------------------------------------");

         ProcessStartInfo ps = new ProcessStartInfo(execCommand, args);
         ps.CreateNoWindow = false;
         ps.UseShellExecute = false;

         Process p = Process.Start(ps);
         p.WaitForExit();
         WriteLine("-----------------------------------------------------");

         if (p.ExitCode != 0)
         {
            WriteError("Command line '{0}' failed!. Aborting the backup...", execCommand);
            WriteError("Returned error code: {0}", p.ExitCode);
            throw new CommandAbortedException();
         }
      }

      public bool ShouldContinue()
      {
         WriteHeader("Continue? [Y/N]");
         string response = Console.ReadLine();
         return response.Equals("y", StringComparison.OrdinalIgnoreCase) || response.Equals("yes", StringComparison.OrdinalIgnoreCase);
      }

      public async Task WaitForTaskAsync(Func<CancellationToken, Task> taskFactory, string description = "Waiting for asynchronous operation...")
      {
         if (taskFactory == null)
            return;


         using (CancellationTokenSource cts = new CancellationTokenSource())
         {
            bool wasCanceled = false;
            void OnCancel(object sender, ConsoleCancelEventArgs args)
            {
               if (wasCanceled == false)
               {
                  wasCanceled = true;
                  WriteLine();
                  WriteLine(ConsoleColor.Red, "Attempting to cancel asynchronous operation...");
                  cts.Cancel();
                  args.Cancel = true;
               }
            }

            Console.CancelKeyPress += OnCancel;
            try
            {
               Task task = taskFactory(cts.Token);
               
               if (description == null)
                  description = "";

               string[] spinner = new string[] { "-", "\\", "|", "/" };
               int spinnerPos = 0;

               do
               {
                  Write(ConsoleColor.Gray, description + "  " + spinner[spinnerPos] + "\r");
                  spinnerPos = (spinnerPos + 1) % spinner.Length;
                  await Task.WhenAny(task, Task.Delay(250)).ConfigureAwait(false);
               }
               while (!task.IsCompleted);

               await task.ConfigureAwait(false);

               WriteLine(description + "   " + " ");
            }
            finally
            {
               Console.CancelKeyPress -= OnCancel;
            }
         }
      }
   }
}

