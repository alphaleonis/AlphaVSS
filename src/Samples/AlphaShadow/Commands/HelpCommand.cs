/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using AlphaShadow.Options;
using System.Reflection;
using System.Text;

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
            yield return new OptionSpec("", OptionType.SingleValueAllowed, "The command to get help on", false, "command");
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

      private string ApplicationName
      {
         get
         {
            return Assembly.GetEntryAssembly().GetName().Name;
         }
      }

      public override void Run()
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

               if (option.ValueType != OptionType.ValueProhibited)
               {
                  if ((option.ValueType & OptionType.Required) == 0)
                     usageText.Append('[');

                  usageText.AppendFormat(":{0}", option.ValueText);

                  if ((option.ValueType & OptionType.Required) == 0)
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

            Host.WriteTable(new StringTable(command.Options.Select(opt => opt.Name == "" ? opt.ValueText : String.Format("/{0}", opt.Name)),
               command.Options.Select(opt => opt.HelpText).Cast<object>()), 3);
         }
      }
   }
}
