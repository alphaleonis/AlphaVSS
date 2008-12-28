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

#if ALPHAVSS_TARGET < ALPHAVSS_TARGET_WIN2008
// Dummy value
#define VSS_CF_NOT_SYSTEM_STATE 77
#endif 

#if ALPHAVSS_TARGET < ALPHAVSS_TARGET_WIN2003 
#define VSS_CF_BACKUP_RECOVERY 0 
#define VSS_CF_APP_ROLLBACK_RECOVERY 1 
#endif

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>
	/// 	The <see cref="VssComponentFlags"/> enumeration is used by writers to indicate support for auto-recovery.
	/// </summary>
	/// <remarks>For more information see <see href="http://msdn.microsoft.com/en-us/library/aa384681(VS.85).aspx">MSDN documentation on VSS_COMPONENT_FLAGS Enumeration</see></remarks>
	[System::Flags]
	public enum class VssComponentFlags
	{
		/// <summary>
		/// <para>
		/// 	The writer will need write access to this component after the shadow copy has been created.
		/// </para>
		/// <para>
		/// 	This flag is incompatible with <see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.Transportable"/>.
		/// </para>
		/// </summary>
		BackupRecovery = VSS_CF_BACKUP_RECOVERY,

		/// <summary>
		/// 	<para>
		/// 		If this is a rollback shadow copy (<see dref="T:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes"/> enumeration value of 
		/// 		<see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.RollbackRecovery"/>), the writer for this component will need 
		/// 		write access to this component after the shadow copy has been created.
		/// 	</para>
		/// 	<para>
		/// 		This flag is incompatible with <see dref="F:Alphaleonis.Win32.Vss.VssVolumeSnapshotAttributes.Transportable"/>.
		/// 	</para>
		/// </summary>
		RollbackRecovery = VSS_CF_APP_ROLLBACK_RECOVERY,

		/// <summary>
		/// 	<para>
		/// 	    This component is not part of system state.
		///		</para>
		///		<para>
		/// 		<b>Windows Server 2003:</b> This value is not supported until Windows Vista.
		/// 	</para>
		/// </summary>
		NotSystemState = VSS_CF_NOT_SYSTEM_STATE

	};
}
} }

