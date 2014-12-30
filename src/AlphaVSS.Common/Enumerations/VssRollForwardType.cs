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
   /// <summary>
   ///     The <c>VssRollForwardType</c> enumeration is used by a requester to indicate the type of roll-forward operation it is about to perform.
   /// </summary>
   /// <remarks>
   ///     A requester sets the roll-forward operation type and specifies the restore point for partial roll-forward operations 
   ///     using <see cref="IVssBackupComponents.SetRollForward"/>.
   /// </remarks>
   public enum VssRollForwardType
   {
      /// <summary>
      /// <para>
      ///     No roll-forward type is defined.
      /// </para>
      /// <para>
      ///     This indicates an error on the part of the requester.
      /// </para>
      /// </summary>
      Undefined = 0,
      /// <summary>
      /// The roll-forward operation should not roll forward through logs.
      /// </summary>
      None = 1,
      /// <summary>
      /// The roll-forward operation should roll forward through all logs.
      /// </summary>
      All = 2,
      /// <summary>
      /// The roll-forward operation should roll forward through logs up to a specified restore point.
      /// </summary>
      Partial = 3
   }
}
