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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>This enumeration is used by a writer at backup time to specify through its Writer Metadata Document the default file restore 
   /// method to be used with all the files in all the components it manages.</summary>
   public enum VssRestoreMethod
   {
      /// <summary>
      /// <para>No restore method is defined.</para>
      /// <para>This indicates an error on the part of the writer.</para>
      /// </summary>
      Undefined = 0,

      /// <summary>
      /// A requester will restore files of a selected component or component set only if there are no versions of those files currently on the disk.
      /// </summary>
      /// <remarks>
      /// <para>Unless alternate location mappings are defined for file restoration, if a version of any file managed by a selected component or component set is currently on the disk, none of the files managed by the selected component or component set will be restored.</para>
      /// <para>If a file's alternate location mapping is defined, and a version of the files is present on disk at the original location, files will be written to the alternate location as long as no version of the file exists at the alternate location.</para>
      /// </remarks>
      RestoreIfNotThere = 1,

      /// <summary>
      /// A requester will restore files of a selected component or component set only if there are no versions of those files currently on the disk that cannot be overwritten.
      /// </summary>
      /// <remarks>
      /// <para>Unless alternate location mappings are defined for file restoration, if there is a version of any file that cannot be overwritten of the selected component or component set on the disk, none of the files managed by the component or component set will be restored.</para>
      /// <para>If a file's alternate location mapping is defined, files will be written to the alternate location.</para>
      /// </remarks>
      RestoreIfCanReplace = 2,

      /// <summary>
      /// This value is used by a writer to indicates that a given service must be stopped prior to the start of the restore. After the restore operation, the service will be restarted.
      /// </summary>
      /// <remarks>
      /// The service to be stopped is specified by an argument to <c>IVssCreateWriterMetadata.SetRestoreMethod</c>.
      /// </remarks>
      StopRestoreStart = 3,

      /// <summary>
      /// A requester must restore the files of a selected component or component set to the location specified by the alternate location mapping specified in the writer component metadata file.
      /// </summary>
      RestoreToAlternateLocation = 4,

      /// <summary>
      /// A requester will restore the files of a selected component or component set following a reboot of the system.
      /// <para>
      /// Files to be restored should be copied to a temporary location, and the requester should use <c>File.Move</c> with the <c>DelayUntilReboot</c> flag 
      /// to complete the restoration of these files to their proper location following reboot. (Using AlphaFS for file operations).
      /// </para>
      /// </summary>
      RestoreAtReboot = 5,

      /// <summary>
      /// If possible, a requester will restore the files of a selected component or component set to their correct location immediately.
      /// </summary>
      /// <remarks>
      /// <para>If there are versions of any of the files managed by the selected component or component set on the disk that cannot be overwritten, then all the files managed by the selected component or component set will be restored following the reboot of the system.</para>
      /// <para>In this case, files to be restored should be copied to a temporary location, and the requester should use <c>File.Move</c> with the <c>DelayUntilReboot</c> flag 
      /// to complete the restoration of these files to their proper location following reboot. (Using AlphaFS for file operations). </para>
      /// <note><b>Windows XP:</b> This value is not supported until Windows Server 2003</note>
      /// </remarks>
      RestoreAtRebootIfCannotReplace = 6,

      /// <summary>
      /// This value indicates that a custom restore method will be used to restore the files managed by the selected component or component set. 
      /// </summary>
      Custom = 7,

      /// <summary>
      /// The requester should perform the restore operation as follows:
      /// <list type="number">
      /// <item><description>Send the PreRestore event and wait for all writers to process it.</description></item>
      /// <item><description>Restore the files to their original locations.</description></item>
      /// <item><description>Send the PostRestore event and wait for all writers to process it.</description></item>
      /// <item><description>Stop the service.</description></item>
      /// <item><description>Restart the service.</description></item>
      /// </list>
      /// <para>The service to be stopped is specified by the writer beforehand when it calls the IVssCreateWriterMetadata::SetRestoreMethod method. 
      /// The requester can obtain the name of the service by examining the <see cref="IVssExamineWriterMetadata.RestoreMethod"/> property.</para>
      /// </summary>
      RestoreStopStart = 8
   };
}
