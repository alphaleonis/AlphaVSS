
using System;

namespace AlphaShadow.Commands
{
   static class ContextOptions
   {
      public static readonly OptionSpec OptPersistent = new OptionSpec("p", OptionType.ValueProhibited, "Manages persistent shadow copies.", false);
      public static readonly OptionSpec OptNoWriters = new OptionSpec("nw", OptionType.ValueProhibited, "Manages no-writer shadow copies.", false);
      public static readonly OptionSpec OptNoAutoRecovery = new OptionSpec("nar", OptionType.ValueProhibited, "Creates shadow copies with no auto-recovery.", false);
      public static readonly OptionSpec OptTxFRecovered = new OptionSpec("tr", OptionType.ValueProhibited, "Creates TxF recovered shadow copies.", false);
      public static readonly OptionSpec OptDifferential = new OptionSpec("ad", OptionType.ValueProhibited, "Creates differential HW shadow copies.", false);
      public static readonly OptionSpec OptPlex = new OptionSpec("ap", OptionType.ValueProhibited, "Creates plex HW shadow copies.", false);
      public static readonly OptionSpec OptSharedFolders = new OptionSpec("scsf", OptionType.ValueProhibited, "Creates shadow copies for Shared Folders (Client Accessible).", false);

      public static OptionSpec[] All = 
		      {
		         OptPersistent,
		         OptNoWriters,
		         OptNoAutoRecovery,
		         OptTxFRecovered,
		         OptDifferential,
		         OptPlex,
		         OptSharedFolders
		      };
   }
}
