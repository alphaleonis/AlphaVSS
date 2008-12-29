using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
	/// <summary>
	/// Represents information about how a writer wants its data to be restored.
	/// </summary>
	/// <remarks>This class is a container for the data returned by <see cref="IVssExamineWriterMetadata.GetRestoreMethod"/>.</remarks>
	public class VssWMRestoreMethod
	{
        public VssWMRestoreMethod(VssRestoreMethod restoreMethod, string service, string userProcedure, VssWriterRestore writerRestore, bool rebootRequired, int mappings)
        {
            mRestoreMethod = restoreMethod;
            mService = service;
            mUserProcedure = userProcedure;
            mWriterRestore = writerRestore;
            mRebootRequired = rebootRequired;
            mMappingCount = mappings;
        }

        /// <summary>
		/// A <see cref="VssRestoreMethod"/> value that specifies file overwriting, the use of alternate locations specifying the method that 
		/// will be used in the restore operation.
		/// </summary>
		public VssRestoreMethod Method { get { return mRestoreMethod; } }

		/// <summary>
		/// If the value of <see cref="Method" /> is <see dref="F:Alphaleonis.Win32.Vss.VssRestoreMethod.StopRestoreStart" />, a pointer to a string containing the name 
		/// of the service that is started and stopped. Otherwise, the value is <see langword="null"/>.
		/// </summary>
		public string Service { get { return mService; } }

		/// <summary>
		/// Pointer to the URL of an HTML or XML document describing to the user how the restore is to be performed. The value may be <see langword="null" />.
		/// </summary>
		public string UserProcedure { get { return mUserProcedure; } }

		/// <summary>
		/// A <see cref="VssWriterRestore" /> value specifying whether the writer will be involved in restoring its data.
		/// </summary>
		public VssWriterRestore WriterRestore { get { return mWriterRestore; } }

		/// <summary>A <see langword="bool"/> indicating whether a reboot will be required after the restore operation is complete.</summary>
		/// <value><see langword="true"/> if a reboot will be required and <see langword="false"/> if it will not.</value>
		public bool RebootRequired { get { return mRebootRequired; } }

		/// <summary>The number of alternate mappings associated with the writer.</summary>
		public int MappingCount { get { return mMappingCount; } }
	
        private VssRestoreMethod mRestoreMethod;
		private string mService;
		private string mUserProcedure;
		private VssWriterRestore mWriterRestore;
		private bool mRebootRequired;
		private int mMappingCount;
	};
}
