// This file is used by Code Analysis to maintain 
// CA_GLOBAL_SUPPRESS_MESSAGE macros that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="VSS");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="x");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="Db", Scope="member", Target="Alphaleonis.Win32.Vss.VssSourceType.#NonTransactedDb");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="Tx", Scope="member", Target="Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.#TxFRecovery");
