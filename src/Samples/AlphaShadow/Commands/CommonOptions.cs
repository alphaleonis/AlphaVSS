
using System;

namespace AlphaShadow.Commands
{
   class CommonOptions
   {
      public static readonly OptionSpec OptTransportable = new OptionSpec("t", OptionType.SingleValueRequired, "Transportable shadow set. Also generates the backup components doc.", false, "file.xml");
      public static readonly OptionSpec OptNonTransportableDoc = new OptionSpec("bc", OptionType.SingleValueRequired, "Generates the backup components doc for non-transportable shadow set.", false, "file.xml");
      public static readonly OptionSpec OptVerifyWriterIncluded = new OptionSpec("wi", OptionType.MultipleValuesRequired, "Verify that a writer/component is included", false, "WriterName");
      public static readonly OptionSpec OptExcludeWriter = new OptionSpec("wx", OptionType.MultipleValuesRequired, "Exclude a writer/component from set creation or restore", false, "WriterName");
      public static readonly OptionSpec OptSetVarScript = new OptionSpec("script", OptionType.SingleValueRequired, "Generates a SETVAR script with the specified filename.", false, "file.cmd");
      public static readonly OptionSpec OptExecCommand = new OptionSpec("exec", OptionType.SingleValueRequired, "Custom command executed after shadow creation.", false, "command");
      public static readonly OptionSpec OptExecCommandArgs = new OptionSpec("execArgs", OptionType.SingleValueRequired, "Arguments to send to custom command.", false, "arguments");      
   }
}
