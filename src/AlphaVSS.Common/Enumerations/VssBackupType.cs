
namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     The <see cref="VssBackupType"/> enumeration indicates the type of backup to be performed using VSS writer/requester 
   ///     coordination.
   /// </summary>
   /// <remarks>
   ///     For more information see <see href="http://msdn.microsoft.com/en-us/library/aa384679(VS.85).aspx">MSDN documentation on 
   ///     VSS_BACKUP_TYPE Enumeration</see>
   /// </remarks>
   public enum VssBackupType
   {
      /// <summary>
      ///     <para>
      ///         The backup type is not known.
      ///     </para>
      ///     <para>
      ///         This value indicates an application error.
      ///      </para>
      ///  </summary>
      Undefined = 0,

      /// <summary>
      ///		<para>Full backup: all files, regardless of whether they have been marked as backed up or not, are saved. This is the default backup type and schema, and all writers support it.</para>
      ///		<para>Each file's backup history will be updated to reflect that it was backed up.</para>
      /// </summary>
      Full = 1,

      /// <summary>
      ///	<para>
      ///		Incremental backup: files created or changed since the last full or incremental backup are saved. Files are marked as having been backed up.
      ///	</para>
      ///	<para>
      ///	A requester can implement this sort of backup on a particular writer only if it supports the <see cref="VssBackupSchema.Incremental" /> schema.
      ///	</para>
      ///	<para>
      ///		If a requester's backup type is <see cref="Incremental"/> and a particular writer's backup schema does not support that sort of backup, the requester will always perform a full (<see cref="Full"/>) backup on that writer's data.
      ///	</para>
      /// </summary>
      Incremental = 2,

      /// <summary>
      /// 	<para>
      /// 		Differential backup: files created or changed since the last full backup are saved. Files are not marked as having been backed up.
      /// 	</para>
      /// 	<para>
      /// 		A requester can implement this sort of backup on a particular writer only if it supports the <see cref="VssBackupSchema.Differential" /> schema.
      /// 	</para>
      /// 	<para>
      /// 		If a requester's backup type is <see cref="Differential"/> and a particular writer's backup schema does not support that sort of backup, the requester will always perform a full (<see cref="Full"/>) backup on that writer's data.
      /// 	</para>
      /// </summary>
      Differential = 3,


      /// <summary>
      /// 	<para>
      /// 		The log file of a writer is to participate in backup or restore operations.
      /// 	</para>
      /// 	<para>
      /// 		A requester can implement this sort of backup on a particular writer only if it supports the <see cref="VssBackupSchema.Log" /> schema.
      /// 	</para>
      /// 	<para>
      /// 		If a requester's backup type is <see cref="Log"/> and a particular writer's backup schema does not support that sort of backup, the requester will always perform a full (<see cref="Full"/>) backup on that writer's data.
      /// 	</para>
      /// </summary>
      Log = 4,

      /// <summary>
      /// 	<para>
      /// 	    Files on disk will be copied to a backup medium regardless of the state of each file's backup history, and the backup history will not be updated.
      /// 	</para>
      /// 	<para>
      /// 		A requester can implement this sort of backup on a particular writer only if it supports the <see cref="VssBackupSchema.Copy" /> schema.
      /// 	</para>
      /// 	<para>
      /// 		If a requester's backup type is <see cref="Copy"/> and a particular writer's backup schema does not support that sort of backup, the requester will always perform a full (<see cref="Full"/>) backup on that writer's data.
      /// 	</para>
      /// </summary>
      Copy = 5,

      /// <summary>Backup type that is not full, copy, log, incremental, or differential.</summary>
      Other = 6
   };
}
