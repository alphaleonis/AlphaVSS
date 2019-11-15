
using System;

namespace AlphaShadow
{
   public class OptionSpec
   {

      public OptionSpec(string name, OptionTypes valueType, string helpText, bool isRequired, string valueText = null)
      {
         Name = name;
         ValueType = valueType;
         HelpText = helpText;
         IsRequired = isRequired;
         ValueText = valueText;
      }

      public string Name { get; private set; }
      public OptionTypes ValueType { get; private set; }
      public string HelpText { get; private set; }
      public bool IsRequired { get; private set; }
      public string ValueText { get; private set; }

      public OptionSpec AsRequired()
      {
         return new OptionSpec(Name, ValueType, HelpText, true, ValueText);
      }

      public OptionSpec AsOptional()
      {
         return new OptionSpec(Name, ValueType, HelpText, false, ValueText);
      }

      public OptionSpec WithHelpText(string helpText)
      {
         return new OptionSpec(Name, ValueType, helpText, IsRequired, ValueText);
      }

      public override string ToString()
      {
         return Name;
      }
   }
}
