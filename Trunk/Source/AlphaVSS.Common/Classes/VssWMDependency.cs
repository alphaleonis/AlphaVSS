using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
	/// <summary>
	/// <see cref="VssWMDependency"/> is used to determine the writer ID, logical path, and component name of components that must be restored or 
	/// backed up along with the target component.</summary>
	/// <remarks>Note that a dependency does not indicate an order of preference between the component with the documented dependencies and the components it depends on. A dependency merely indicates that the component and the components it depends on must always be backed up or restored together.
	/// <note><b>Windows XP:</b> This class is not supported until Windows Server 2003</note>
	/// </remarks>
	/// <seealso href="http://msdn.microsoft.com/en-us/library/aa384301(VS.85).aspx"/>
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
            mWriterId = writerId;
            mLogicalPath = logicalPath;
            mComponentName = componentName;
        }

		/// <summary>
		/// The class ID of a writer containing a component that the current component depends on.
		/// </summary>
		public Guid WriterId { get { return mWriterId; } }

		/// <summary>
		/// The logical path of a component that the current component depends on.
		/// </summary>
		public string LogicalPath { get { return mLogicalPath; } }

		/// <summary>
		/// Retrieves the name of a component that the current component depends on.
		/// </summary>
        public string ComponentName { get { return mComponentName; } }

		private Guid mWriterId;
		private string mLogicalPath;
		private string mComponentName;
	};
}
