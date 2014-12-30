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
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope="member", Target="Alphaleonis.Win32.Vss.VssListAdapter`1+Enumerator.#!Enumerator()");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="XPSP", Scope="member", Target="Alphaleonis.Win32.Vss.OSVersions.#WinXPSP1");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="XPSP", Scope="member", Target="Alphaleonis.Win32.Vss.OSVersions.#WinXPSP2");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="XPSP", Scope="member", Target="Alphaleonis.Win32.Vss.OSVersions.#WinXPSP3");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId="XPSP", Scope="member", Target="Alphaleonis.Win32.Vss.OSVersions.#WinXPSP4");
CA_GLOBAL_SUPPRESS_MESSAGE("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope="member", Target="Alphaleonis.Win32.Vss.VssExamineWriterMetadata.#RequireIVssExamineWriterMetadataEx()");
