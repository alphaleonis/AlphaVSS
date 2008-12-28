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

namespace Alphaleonis { namespace Win32 { namespace Vss
{
	/// <summary>The <see cref="VssProviderType"/> enumeration specifies the provider type.</summary>
	public enum class VssProviderType
	{
		/// <summary>
		/// 	<para>
		/// 	    The provider type is unknown.
		/// 	</para>
		/// 	<para>
		/// 		This indicates an error in the application or the VSS service, or that no provider is available.
		/// 	</para>
		/// </summary>
		Unknown = VSS_PROV_UNKNOWN,

		/// <summary>The default provider that ships with Windows.</summary>
		System = VSS_PROV_SYSTEM,

		/// <summary>A software-based provider.</summary>
		Software = VSS_PROV_SOFTWARE,
		
		/// <summary>A hardware-based provider.</summary>
		Hardware = VSS_PROV_HARDWARE
	};
}
} }