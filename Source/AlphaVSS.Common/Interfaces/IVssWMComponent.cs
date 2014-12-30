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
using System;
using System.Collections.Generic;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     <see cref="IVssWMComponent"/> is a class that allows access to component information stored in a Writer Metadata Document.
   ///     Instances of <see cref="IVssWMComponent"/> are obtained by enumerating <cee cref="IVssExamineWriterMetadata.Components"/>.
   /// </summary>
   public interface IVssWMComponent : IDisposable
   {
      /// <summary>
      ///     The component type.
      /// </summary>
      VssComponentType Type { get; }

      /// <summary>
      ///     The logical path of the component.
      /// </summary>
      /// <value>A string containing the logical path of the component, which may be <see langword="null"/>.
      /// <remarks>There are no restrictions on the characters that can appear in a non-NULL logical path.</remarks></value>
      string LogicalPath { get; }

      /// <summary>
      ///     The name of the component. A component name string cannot be <see langword="null"/>.
      /// </summary>
      string ComponentName { get; }

      /// <summary>
      ///     The description of the component. A caption string can be <see langword="null" />.
      /// </summary>
      string Caption { get; }

      /// <summary>
      ///     Gets a buffer containing the binary data for a displayable icon representing the component. 
      /// </summary>
      /// <remarks>
      ///     The buffer contents should use the same format as the standard icon (.ico) files. If the writer that created 
      ///     the component did not choose to specify an icon, the value will be <see langword="null"/>.
      /// </remarks>
      /// <returns>A buffer containing the binary data for a displayable icon representing the component. </returns>
      byte[] GetIcon();

      /// <summary>
      ///     Boolean that indicates whether there is private metadata associated with the restoration of the component.
      /// </summary>
      /// <value>
      ///     The Boolean is <see langword="true"/> if there is private metadata associated with the restoration
      ///     of the components, and <see langword="false"/> if there is not. 
      /// </value>
      bool RestoreMetadata { get; }

      /// <summary>Reserved for future use.</summary>
      bool NotifyOnBackupComplete { get; }

      /// <summary>
      ///     Boolean that indicates (for component mode operations) if the component is selectable for backup.
      /// </summary>
      /// <remarks>
      ///     The value of <see cref="Selectable"/> helps determine whether a requester has the option of including or excluding 
      ///     a given component in backup operations. 
      /// </remarks>
      /// <value>
      ///     <see langword="true"/> if the component is selectable for backup and <see langword="false"/> if it is not. 
      /// </value>
      /// <seealso href="http://msdn.microsoft.com/en-us/library/aa384680(VS.85).aspx">VSS_COMPONENTINFO structure on MSDN</seealso>
      bool Selectable { get; }

      /// <summary>
      ///     Boolean that indicates (for component-mode operations) whether the component is selectable for restore.
      /// </summary>
      /// <remarks>
      ///     <para>
      ///         <see cref="SelectableForRestore"/> allows the requester to determine whether this component can be individually selected 
      ///         for restore if it had earlier been implicitly included in the backup.
      ///     </para>
      ///     <para>
      ///         <note>
      ///             <b>Windows XP:</b> This  requires Windows Server 2003 or later. It will always return false on earlier operating systems.
      ///         </note>
      ///     </para>
      /// </remarks>    
      /// <value>
      ///     The Boolean is <see langword="true"/> if the component is selectable for restore and <see langword="false"/> if it is not.
      /// </value>
      bool SelectableForRestore { get; }

      /// <summary>
      ///     A bit mask (or bitwise OR) of values of the <see cref="VssComponentFlags"/> enumeration, indicating the 
      ///     features this component supports.
      /// </summary>
      /// <remarks>
      ///     <note>
      ///         <b>Windows Server 2003 and Windows XP:</b>  Before Windows Server 2003 SP1, this member is reserved for system use and 
      ///         	will always return <see cref="VssComponentFlags.None"/>.
      ///     </note>
      /// </remarks>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
      VssComponentFlags ComponentFlags { get; }

      /// <summary>
      ///     The file descriptors associated with this component.
      /// </summary>
      /// <remarks>This collection represents the method <c>GetFile()</c> of <c>IVssWMComponent</c> in the VSS API</remarks>
      IList<VssWMFileDescriptor> Files { get; }

      /// <summary>
      ///     A list of <see cref="VssWMFileDescriptor"/> instances containing information about the database backup component files.
      /// </summary>
      IList<VssWMFileDescriptor> DatabaseFiles { get; }

      /// <summary>
      ///     A list of file descriptors for the log files associated with the specified database backup component.
      /// </summary>
      IList<VssWMFileDescriptor> DatabaseLogFiles { get; }

      /// <summary>
      ///     A list of  <see cref="VssWMDependency"/> instances containing accessors for obtaining information about explicit writer-component 
      ///     dependencies of one of the current components.
      /// </summary>
      /// <remarks>This will always be an empty list on operating systems earlier than Windows Server 2003.</remarks>
      IList<VssWMDependency> Dependencies { get; }
   }
}
