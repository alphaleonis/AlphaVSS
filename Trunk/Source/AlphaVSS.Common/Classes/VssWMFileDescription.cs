
namespace Alphaleonis.Win32.Vss
{
    /// <summary>
    /// The <see cref="VssWMFileDescription"/> class is returned to a calling application by a number of query methods. 
    /// It provides detailed information about a file or set of files (a file set).
    /// </summary>
    /// <remarks>
    ///     The following methods return a <see cref="VssWMFileDescription"/> instance:
    ///     <list type="bullet">
    ///         <item><description><see cref="IVssComponent.AlternateLocationMappings"/></description></item>
    ///         <item><description><see cref="IVssComponent.NewTargets"/></description></item>
    ///         <item><description><see cref="IVssExamineWriterMetadata.ExcludeFiles"/></description></item>
    ///         <item><description><see cref="IVssExamineWriterMetadata.AlternateLocationMappings"/></description></item>
    ///         <item><description><see cref="IVssWMComponent.Files"/></description></item>
    ///         <item><description><see cref="IVssWMComponent.DatabaseFiles"/></description></item>
    ///         <item><description><see cref="IVssWMComponent.DatabaseLogFiles"/></description></item>
    ///     </list>
    /// </remarks>
	public class VssWMFileDescription 
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="VssWMFileDescription"/> class.
        /// </summary>
        /// <param name="alternateLocation">The alternate location.</param>
        /// <param name="backupTypeMask">The backup type mask.</param>
        /// <param name="fileSpecification">The file specification.</param>
        /// <param name="path">The path.</param>
        /// <param name="isRecursive">if set to <c>true</c> this file description is recursive.</param>
        public VssWMFileDescription(string alternateLocation, VssFileSpecificationBackupType backupTypeMask, string fileSpecification, string path, bool isRecursive)
        {
            mAlternateLocation = alternateLocation;
            mBackupTypeMask = backupTypeMask;
            mFileSpecification = fileSpecification;
            mPath = path;
            mIsRecursive = isRecursive;
        }
        
        /// <summary>
		/// Obtains the alternate backup location of the component files.
		/// </summary>
		public string AlternateLocation { get { return mAlternateLocation; } }		

		/// <summary>
		/// Obtains the file backup specification for a file or set of files.
		/// </summary>
		/// <remarks><note><b>Windows XP:</b> This value is not supported in Windows XP and will always return <see cref="VssFileSpecificationBackupType.Unknown"/></note></remarks>
		public VssFileSpecificationBackupType BackupTypeMask 
        { 
            get { return mBackupTypeMask; } }

		/// <summary>
		/// Obtains the file specification for the list of files provided.
		/// </summary>
		public string FileSpecification { get { return mFileSpecification; } }
		
        /// <summary>
		/// Obtains the fully qualified directory path for the list of files provided.
		/// </summary>
		public string Path { get { return mPath; } }

		/// <summary>
		/// Determines whether only files in the root directory or files in the entire directory hierarchy are considered for backup.
		/// </summary>
		/// <remarks>VSS API reference: <c>IVssWMFiledesc::GetRecursive()</c></remarks>
		public bool IsRecursive { get { return mIsRecursive; } }
	        
		private string mAlternateLocation;
		private VssFileSpecificationBackupType mBackupTypeMask;
		private string mFileSpecification;
		private string mPath;
		private bool mIsRecursive;
	};
}
