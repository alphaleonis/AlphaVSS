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
   /// The <see cref="VssComponentType"/> enumeration is used by both the requester and the writer to specify the type of component being used 
   /// with a shadow copy backup operation.
   /// </summary>
   /// <remarks>
   ///     <para>
   ///         A writer sets a component's type when it adds the component to its Writer Metadata Document using 
   ///         <c>IVssCreateWriterMetadata.AddComponent</c>
   ///     </para>
   ///     <para>
   ///         Writers and requesters can find the type information of components selected for inclusion in a Backup 
   ///         Components Document through calls to <see cref="IVssComponent.ComponentType"/> to return a component type directly.
   ///     </para>
   ///     <para>
   ///         A requester can obtain the type of any component in a given writer's Writer Metadata Document by doing the following:
   ///         <list type="number">
   ///             <item><description>Using <see cref="IVssExamineWriterMetadata.Components"/> to obtain a <see cref="IVssWMComponent"/> interface</description></item>
   ///             <item><description>Examining the Type member of the <see cref="IVssWMComponent"/> object</description></item>
   ///         </list>
   ///     </para>
   /// </remarks>
   public enum VssComponentType
   {
      /// <summary><para>Undefined component type.</para>
      /// <para>This value indicates an application error.</para>
      /// </summary>
      Undefined = 0,

      /// <summary>Database component.</summary>
      Database = 1,

      /// <summary>File group component. This is any component other than a database.</summary>
      FileGroup = 2
   };
}

