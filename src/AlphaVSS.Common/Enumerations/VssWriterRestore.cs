
namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssWriterRestore"/> enumeration is used by a writer to indicate to a requester the 
   /// conditions under which it will handle events generated during a restore operation.</summary>
   public enum VssWriterRestore
   {
      /// <summary>
      /// 	<para>It is not known whether the writer will perform special operations during the restore operation.</para>
      /// 	<para>This state indicates a writer error.</para>
      /// </summary>
      Undefined = 0,

      /// <summary>The writer does not require restore events.</summary>
      Never = 1,

      /// <summary>
      /// 	Indicates that the writer always expects to handle a <c>PreRestore</c> event, but expects to handle a 
      /// 	<c>PostRestore</c> event only if a restore fails when implementing either a 
      /// 	<see cref="VssRestoreMethod.RestoreIfNotThere"/> or
      /// 	<see cref="VssRestoreMethod.RestoreIfCanReplace"/> restore method (<see cref="VssRestoreMethod"/>)
      /// </summary>
      IfReplaceFails = 2,

      /// <summary>The writer always performs special operations during the restore operation.</summary>
      Always = 3
   }
}