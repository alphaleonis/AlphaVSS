

namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssRestoreType"/> enumeration is used by a requester to indicate the type of restore operation it is about to perform.</summary>
   /// <remarks>
   ///     <para>A requester sets the type of a restore operation using <see cref="IVssBackupComponents.SetRestoreState"/>.</para>
   ///     <!-- <para>A writer can retrieve the type of a restore operation by calling CVssWriter::GetRestoreType.</para> -->
   /// </remarks>
   public enum VssRestoreType
   {
      /// <summary>
      /// 	<para>No restore type is defined.</para>
      /// 	<para>This indicates an error on the part of the requester.</para>
      /// </summary>
      Undefined = 0,

      /// <summary>The default restore type: A requester restores backed-up data to the original volume from a backup medium.</summary>
      ByCopy = 1,

      /// <summary>
      /// 	<para>
      /// 		A requester does not copy data from a backup medium, but imports a transportable shadow copy 
      /// 		and uses this imported volume for operations such as data mining.
      /// 	</para>
      /// 	<para>
      /// 		<b>Windows Server 2003, Standard Edition and Windows Server 2003, Web Edition:</b> This value is not supported. All editions of Windows Server 2003 SP1 support this value.
      /// 	</para>
      /// </summary>
      Import = 2,

      /// <summary>A restore type not currently enumerated. This value indicates an application error.</summary>
      Other = 3

   };
}
