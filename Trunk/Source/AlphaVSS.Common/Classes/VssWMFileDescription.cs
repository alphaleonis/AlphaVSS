using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alphaleonis.Win32.Vss
{
	public class VssWMFileDescription 
	{
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
