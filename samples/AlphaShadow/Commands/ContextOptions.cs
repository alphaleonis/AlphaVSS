
using System;

namespace AlphaShadow.Commands
{
   static class ContextOptions
   {
      public static readonly OptionSpec OptPersistent = new OptionSpec("p", OptionTypes.ValueProhibited, "Manages persistent shadow copies.", false);
      public static readonly OptionSpec OptNoWriters = new OptionSpec("nw", OptionTypes.ValueProhibited, "Manages no-writer shadow copies.", false);
      public static readonly OptionSpec OptNoAutoRecovery = new OptionSpec("nar", OptionTypes.ValueProhibited, "Creates shadow copies with no auto-recovery.", false);
      public static readonly OptionSpec OptTxFRecovered = new OptionSpec("tr", OptionTypes.ValueProhibited, "Creates TxF recovered shadow copies.", false);
      public static readonly OptionSpec OptDifferential = new OptionSpec("ad", OptionTypes.ValueProhibited, "Creates differential HW shadow copies.", false);
      public static readonly OptionSpec OptPlex = new OptionSpec("ap", OptionTypes.ValueProhibited, "Creates plex HW shadow copies.", false);
      public static readonly OptionSpec OptSharedFolders = new OptionSpec("scsf", OptionTypes.ValueProhibited, "Creates shadow copies for Shared Folders (Client Accessible).", false);

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
