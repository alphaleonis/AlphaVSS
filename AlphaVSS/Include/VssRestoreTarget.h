/* Copyright (c) 2008 Peter Palotas
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
#pragma once

#include <vss.h>

#if NTDDI_VERSION < NTDDI_LONGHORN
// Dummy value
#define VSS_RT_ORIGINAL_LOCATION 1000
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	<para>
	/// 		The <see cref="VssRestoreTarget"/> enumeration is used by a writer at restore time to 
	/// 		indicate how all the files included in a selected component, and all the files in any 
	/// 		component set it defines, are to be restored.
	/// 	</para>
	/// 	<para>
	/// 		Setting a restore target modifies or overrides the restore method set during backup (see <see dref="T:Alphaleonis.Win32.Vss.VssRestoreMethod"/>).
	/// 	</para>
	/// </summary>
	public enum class VssRestoreTarget
	{
		/// <summary>
		///	<para>No target is defined.</para>
		/// <para>This value indicates an error on the part of the writer.</para>
		/// </summary>
		Undefined = VSS_RT_UNDEFINED,

		/// <summary>
		/// 	<para>	
		/// 		This is the default restore target.
		/// 	</para>
		/// 	<para>
		/// 		This value indicates that the restoration of the files included in a selected component 
		/// 		(or the component set defined by that component) should proceed according to the original 
		/// 		restore method specified at backup time by a <see dref="T:Alphaleonis.Win32.Vss.VssRestoreMethod"/> value.
		/// 	</para>
		/// </summary>
		Original = VSS_RT_ORIGINAL,

		/// <summary>
		/// 	<para>
		/// 	    The files are restored to a location determined from an existing alternate location mapping.
		/// 	</para>
		/// 	<para>
		/// 		The restore target should be set to <see cref="Alternate"/> only when 
		/// 		alternate location mappings have been set for all the files managed by 
		/// 		a selected component or component set.
		/// 	</para>
		/// </summary>
		Alternate = VSS_RT_ALTERNATE,

		/// <summary>
		/// 	<para>
		/// 		Use directed targeting by the writer at restore time to restore a file.
		/// 	</para>
		/// 	<para>
		/// 		Directed targeting allows a writer to control, on a file-by-file basis, how a file is 
		/// 		restored—indicating how much of a file is to be restored and into which files the backed-up file is to be restored.
		/// 	</para>
		/// </summary>
		Directed = VSS_RT_DIRECTED,

		/// <summary>
		/// 	<para>
		/// 	    The files are restored to the location at which they were at backup time, even if the original restore 
		/// 		method that was specified at backup time was <see dref="F:Alphaleonis.Win32.Vss.VssRestoreMethod.RestoreToAlternateLocation"/>.
		/// 	</para>
		/// 	<para>
		/// 		<b>Windows Server 2003 and Windows XP:</b> This value is not supported.
		/// 	</para>
		/// </summary>
		OriginalLocation = VSS_RT_ORIGINAL_LOCATION
	};
}
} }