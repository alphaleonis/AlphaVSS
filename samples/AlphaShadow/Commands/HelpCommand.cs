
using System;
using System.Collections.Generic;
using System.Linq;
using AlphaShadow.Options;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AlphaShadow
{
   public class HelpCommand : Command
   {
      private IEnumerable<Command> m_commands;

      public HelpCommand(IEnumerable<Command> commands)
         : base("help", "Displays help information.")
      {
         m_commands = commands;
      }

      public override IEnumerable<OptionSpec> Options
      {
         get 
         {
            yield return new OptionSpec("", OptionTypes.SingleValueAllowed, "The command to get help on", false, "command");
         }
      }

      protected override void ProcessOptions()
      {
         CommandName = RemainingArguments.FirstOrDefault();
      }

      private string CommandName { get; set; }

      private Command GetCommand(string name)
      {
         return m_commands.FirstOrDefault(cmd => cmd.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
      }

      private static string ApplicationName
      {
         get
         {
            return Assembly.GetEntryAssembly().GetName().Name;
         }
      }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
      public override async Task RunAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
      {
         Host.WriteLine();

         if (CommandName == null)
         {
            Host.WriteTable(new StringTable(m_commands.Select(cmd => String.Format("{0} {1}", ApplicationName, cmd.Name)), m_commands.Select(cmd => cmd.Description).Cast<object>()), 3, true);
         }
         else
         {
            Command command = GetCommand(CommandName);
            if (command == null)
               throw new ArgumentException(String.Format("Unknown command {0}.", CommandName));

            Host.WriteLine(command.Description);

            Host.WriteLine();

            StringBuilder usageText = new StringBuilder();
            foreach (OptionSpec option in command.NamedOptions)
            {
               if (!option.IsRequired)
                  usageText.Append('[');

               usageText.AppendFormat("/{0}", option.Name);

               if (option.ValueType != OptionTypes.ValueProhibited)
               {
                  if ((option.ValueType & OptionTypes.Required) == 0)
                     usageText.Append('[');

                  usageText.AppendFormat(":{0}", option.ValueText);

                  if ((option.ValueType & OptionTypes.Required) == 0)
                     usageText.Append(']');
               }

               if (!option.IsRequired)
                  usageText.Append(']');

               usageText.Append(' ');
            }

            if (command.UnnamedOption != null)
            {
               if (!command.UnnamedOption.IsRequired)
                  usageText.Append('[');

               usageText.Append(command.UnnamedOption.ValueText);

               if (!command.UnnamedOption.IsRequired)
                  usageText.Append(']');
            }

            Host.WriteTable(new StringTable(new[] { String.Format("{0} {1}", ApplicationName, command.Name) }, new string[] { usageText.ToString() }), 1);
            Host.WriteLine();

            Host.WriteTable(new StringTable(command.Options.Select(opt => String.IsNullOrEmpty(opt.Name) ? opt.ValueText : String.Format("/{0}", opt.Name)),
               command.Options.Select(opt => opt.HelpText).Cast<object>()), 3);
         }
      }
   }
}
