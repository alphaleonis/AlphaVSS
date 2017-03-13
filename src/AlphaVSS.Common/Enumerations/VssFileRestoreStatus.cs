
namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     The <see cref="VssFileRestoreStatus" /> enumeration defines the set of statuses of a file restore operation performed on 
   ///     the files managed by a selected component or component set.
   /// </summary>
   /// <remarks>
   ///     See MSDN documentation on <c>VSS_FILE_RESTORE_STATUS</c> for more information.   
   /// </remarks>
   public enum VssFileRestoreStatus
   {
      /// <summary>
      /// 	<para>
      /// 	    The restore state is undefined.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error, or indicates that a restore operation has not yet started.
      /// 	</para>
      /// </summary>
      Undefined = 0,

      /// <summary>
      /// 	<para>
      /// 		No files were restored.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error in restoration that did not leave any restored files on disk.
      /// 	</para>
      /// </summary>
      None = 1,

      /// <summary>
      /// 	All files were restored. This value indicates success and should be set for each component that was restored successfully.
      /// </summary>
      All = 2,

      /// <summary>
      /// 	<para>
      /// 	    The restore process failed.
      /// 	</para>
      /// 	<para>
      /// 		This value indicates an error in restoration that did leave some restored files on disk. This means the components on disk are now corrupt.
      /// 	</para>
      /// </summary>
      Failed = 3
   }
}
