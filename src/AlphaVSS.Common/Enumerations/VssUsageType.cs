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
   /// <summary>The <see cref="VssUsageType"/> enumeration specifies how the host system uses the data managed by a writer involved in a VSS operation.</summary>
   /// <remarks>Requester applications that are interested in backing up system state should look for writers with the 
   /// <see cref="VssUsageType.BootableSystemState"/> or <see cref="VssUsageType.SystemService"/> usage type.</remarks>
   public enum VssUsageType
   {
      /// <summary><para>The usage type is not known.</para><para>This indicates an error on the part of the writer.</para></summary>
      Undefined = 0,
      /// <summary>The data stored by the writer is part of the bootable system state.</summary>
      BootableSystemState = 1,
      /// <summary>The writer either stores data used by a system service or is a system service itself.</summary>
      SystemService = 2,
      /// <summary>The data is user data.</summary>
      UserData = 3,
      /// <summary>Unclassified data.</summary>
      Other = 4
   };
}
