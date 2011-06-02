using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AlphaShadow
{
   static class ArgumentExtensions
   {
      public static bool ArgNameIs(this Match match, string optionName)
      {
         return match.Groups["name"].Value.Equals(optionName, StringComparison.OrdinalIgnoreCase);
      }

      public static string GetArgName(this Match match)
      {
         return match.Groups["name"].Value;
      }

      public static string GetArgValue(this Match match)
      {
         return match.Groups["value"].Value.Replace("\\\\", "\\").Replace("\\\"", "\"");
      }

      public static string[] GetArgValues(this Match match)
      {
         return match.Groups["value"].Captures.Cast<Capture>().Select(capture => capture.Value.Replace("\\\\", "\\").Replace("\\\"", "\"")).ToArray();
      }

      public static int GetArgValueCount(this Match match)
      {
         return match.Groups["value"].Captures.Count;
      }

      public static bool HarArgValue(this Match match)
      {
         return match.Groups["value"].Success;
      }

   }

   class Arguments
   {
      #region Private fields

      public static Regex s_argumentRegex = new Regex(
      "(?<=\\s*(/|-))(?<name>[^:=\\s]+)\r\n( ((\\s*(:|=)\\s*)|(\\s+(?" +
      "=[^-/]|\")))\r\n(\r\n\"(?<value>([^\"\\\\]*(\\\\.[^\"\\\\]*)*))\"\r\n" +
      "|\r\n(?<value>[^\"]([^,\\s])*)\r\n)\r\n(\\s*,\\s*\r\n(\r\n\"(?<value>(" +
      "[^\"\\\\]*(\\\\.[^\"\\\\]*)*))\"\r\n|\r\n(?<value>[^\"]([^,\\s])" +
      "*)\r\n)\r\n)*\r\n)?\r\n",
          RegexOptions.IgnoreCase
          | RegexOptions.ExplicitCapture
          | RegexOptions.CultureInvariant
          | RegexOptions.IgnorePatternWhitespace
    );
      #endregion

      #region Constructor

      public Arguments(string commandLine)
      {
         Parse(commandLine);
      }

      #endregion

      public void Parse(string commandLine)
      {         
         MatchCollection s_argumentRegexMatches = s_argumentRegex.Matches(commandLine);

         foreach (Match match in s_argumentRegexMatches)
         {
            Console.WriteLine("[{0}]=[{1}] HasValue={2}", match.GetArgName(), String.Join(" | ", match.GetArgValues()), match.HarArgValue());
         }
      }
   }
}
