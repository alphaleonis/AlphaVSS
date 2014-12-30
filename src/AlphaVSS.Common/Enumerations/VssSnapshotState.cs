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
   /// <summary>The <see cref="VssSnapshotState"/> enumeration is returned by a provider to specify the state of a given shadow copy operation.</summary>
   public enum VssSnapshotState
   {
      /// <summary><para>Reserved for system use.</para><para>Unknown shadow copy state.</para></summary>
      Unknown = 0x00,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is being prepared.</para></summary>
      Preparing = 0x01,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy preparation is in progress.</para></summary>
      ProcessingPrepare = 0x02,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy has been prepared.</para></summary>
      Prepared = 0x03,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy precommit is in process.</para></summary>
      ProcessingPreCommit = 0x04,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is precommitted.</para></summary>
      PreComitted = 0x05,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy commit is in process.</para></summary>
      ProcessingCommit = 0x06,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy is committed.</para></summary>
      Committed = 0x07,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy postcommit is in process.</para></summary>
      ProcessingPostCommit = 0x08,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy file commit operation is underway.</para></summary>
      ProcessingPreFinalCommit = 0x09,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy file commit operation is done.</para></summary>
      PreFinalCommitted = 0x0A,
      /// <summary><para>Reserved for system use.</para><para>Processing of the shadow copy following the final commit and prior to shadow copy create is underway.</para></summary>
      ProcessingPostFinalCommit = 0x0B,
      /// <summary><para>Shadow copy is created.</para></summary>
      Created = 0x0C,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy creation is aborted.</para></summary>
      Aborted = 0x0D,
      /// <summary><para>Reserved for system use.</para><para>Shadow copy has been deleted.</para></summary>
      Deleted = 0x0E,
      /// <summary><para>Reserved value.</para></summary>
      PostCommitted = 0x0F,
   }
}
