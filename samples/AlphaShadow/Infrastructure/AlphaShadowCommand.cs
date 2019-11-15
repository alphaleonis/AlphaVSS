
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaShadow
{
   public abstract class AlphaShadowCommand : Command
   {
      protected static readonly OptionSpec OptVerbose = new OptionSpec("verbose", OptionTypes.ValueProhibited, "Enables verbose tracing output.", false);
      protected static readonly OptionSpec OptNoWrap = new OptionSpec("nowrap", OptionTypes.ValueProhibited, "Disables wordwrapping output text", false);

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
