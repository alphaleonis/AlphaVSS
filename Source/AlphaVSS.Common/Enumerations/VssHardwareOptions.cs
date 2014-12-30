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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   /// Defines shadow copy LUN flags.
   /// </summary>
   /// <remarks>
   ///     Only supported on Windows Server 2008.
   /// </remarks>
   [Flags]
   public enum VssHardwareOptions
   {
      /// <summary>
      /// The shadow copy LUN will be masked from the host.
      /// </summary>
      MaskLuns = 0x00000001,
      /// <summary>
      /// The shadow copy LUN will be exposed to the host as a read-write volume.
      /// </summary>
      MakeReadWrite = 0x00000002,
      /// <summary>
      /// The disk identifiers of all of the shadow copy LUNs will be reverted to that of the 
      /// original LUNs. However, if any of the original LUNs are present on the system, the operation will 
      /// fail and none of the identifiers will be reverted.
      /// </summary>
      RevertIdentityAll = 0x00000004,
      /// <summary>
      /// None of the disk identifiers of the shadow copy LUNs will be reverted.
      /// </summary>
      RevertIdentityNone = 0x00000008,
      /// <summary>
      /// The shadow copy LUNs will be converted permanently to read-write. 
      /// This flag is set only as a notification for the provider; no provider action is required. 
      /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange</c> method.
      /// </summary>
      OnLunStateChangeNotifyReadWrite = 0x00000100,
      /// <summary>
      /// The shadow copy LUNs will be converted temporarily to read-write and are about to undergo TxF recovery 
      /// or VSS auto-recovery. This flag is set only as a notification for the provider; no provider action is required. 
      /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
      /// </summary>
      OnLunStateChangeNotifyLunPreRecovery = 0x00000200,
      /// <summary>
      /// The shadow copy LUNs have just undergone TxF recovery or VSS auto-recovery and have been converted back to 
      /// read-only. This flag is set only as a notification for the provider; no provider action is required. 
      /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
      /// </summary>
      OnLunStateChangeNotifyLunPostRecovery = 0x00000400,
      /// <summary>
      /// The provider must mask shadow copy LUNs from this computer. 
      /// For more information, see the <c>IVssHardwareSnapshotProviderEx::OnLunStateChange method</c>.
      /// </summary>
      OnLunStateChangeDoMaskLuns = 0x00000800
   }
}
