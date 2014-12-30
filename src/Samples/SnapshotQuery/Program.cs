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
// 
// This is a very simple sample to get started with AlphaVSS. The only thing it does
// is to enumerate any existing snapshots in the system and writing some basic
// information about them.
// 


using System;
using Alphaleonis.Win32.Vss;

namespace SnapshotQuery
{
	class Program
	{
		static void Main(string[] args)
		{
			IVssImplementation vssImplementation = VssUtils.LoadImplementation();
			using (IVssBackupComponents backup = vssImplementation.CreateVssBackupComponents())
			{
				backup.InitializeForBackup(null);

				if (OperatingSystemInfo.IsAtLeast(OSVersionName.WindowsServer2003))
				{
					// This does not work on Windows XP, since the only context supported
					// on Windows XP is VssSnapshotContext.Backup which is the default.
					backup.SetContext(VssSnapshotContext.All);
				}
				
				foreach (VssSnapshotProperties prop in backup.QuerySnapshots())
				{
					Console.WriteLine("Snapshot ID: {0:B}", prop.SnapshotId);
					Console.WriteLine("Snapshot Set ID: {0:B}", prop.SnapshotSetId);
					Console.WriteLine("Original Volume Name: {0}", prop.OriginalVolumeName);
					Console.WriteLine();
				}
			}
		}
	}
}
