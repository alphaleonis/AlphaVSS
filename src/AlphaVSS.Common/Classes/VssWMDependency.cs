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
   /// <see cref="VssWMDependency"/> is used to determine the writer ID, logical path, and component name of components that must be restored or 
   /// backed up along with the target component.</summary>
   /// <remarks>Note that a dependency does not indicate an order of preference between the component with the documented dependencies and the components it depends on. A dependency merely indicates that the component and the components it depends on must always be backed up or restored together.
   /// <note><b>Windows XP:</b> This class is not supported until Windows Server 2003</note>
   /// </remarks>
   /// <seealso href="http://msdn.microsoft.com/en-us/library/aa384301(VS.85).aspx"/>
   [Serializable]
   public class VssWMDependency
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssWMDependency"/> class.
      /// </summary>
      /// <param name="writerId">The writer id.</param>
      /// <param name="logicalPath">The logical path.</param>
      /// <param name="componentName">Name of the component.</param>
      public VssWMDependency(Guid writerId, string logicalPath, string componentName)
      {
         WriterId = writerId;
         LogicalPath = logicalPath;
         ComponentName = componentName;
      }

      #region Public Properties

      /// <summary>
      /// The class ID of a writer containing a component that the current component depends on.
      /// </summary>
      public Guid WriterId { get; private set; }

      /// <summary>
      /// The logical path of a component that the current component depends on.
      /// </summary>
      public string LogicalPath { get; private set; }

      /// <summary>
      /// Retrieves the name of a component that the current component depends on.
      /// </summary>
      public string ComponentName { get; private set; }

      #endregion
   };
}
