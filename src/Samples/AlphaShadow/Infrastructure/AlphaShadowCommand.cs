
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaShadow
{
   public abstract class AlphaShadowCommand : Command
   {
      protected static OptionSpec OptVerbose = new OptionSpec("verbose", OptionType.ValueProhibited, "Enables verbose tracing output.", false);
      protected static OptionSpec OptNoWrap = new OptionSpec("nowrap", OptionType.ValueProhibited, "Disables wordwrapping output text", false);

      public AlphaShadowCommand(string name, string description)
         : base(name, description)
      {         
      }

      public virtual IEnumerable<OptionSpec> CommandSpecificOptions
      {
         get
         {
            yield break;
         }
      }

      public override IEnumerable<OptionSpec> Options
      {
         get
         {
            return CommandSpecificOptions.Concat(new [] { OptVerbose, OptNoWrap });
         }
      }

      protected override void ProcessOptions()
      {
         Host.VerboseOutputEnabled = HasOption(OptVerbose);
         Host.IsWordWrapEnabled = !HasOption(OptNoWrap);
      }
   }  
}
