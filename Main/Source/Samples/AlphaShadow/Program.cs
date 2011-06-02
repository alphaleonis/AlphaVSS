using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AlphaShadow
{
   class Program
   {
      static void PrintHeader()
      {
         Version version = Assembly.GetExecutingAssembly().GetName().Version;
         string title = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).Single()).Title;
         string description = ((AssemblyDescriptionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).Single()).Description;
         string copyright = ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false).Single()).Copyright;
         Console.WriteLine("{0} v{1} - {2}", title, version.ToString(2), description);
         Console.WriteLine(copyright);
      }

      static void Main(string[] args)
      {
         PrintHeader();
         Arguments options = new Arguments(Environment.CommandLine);
         
      }
   }
}
