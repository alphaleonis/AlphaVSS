

namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssSourceType"/> enumeration specifies the type of data that a writer manages.</summary>
   public enum VssSourceType
   {
      /// <summary><para>The source of the data is not known.</para><para>This indicates a writer error, and the requester should report it.</para></summary>
      Undefined = 0,

      /// <summary>The source of the data is a database that supports transactions, such as Microsoft SQL Server.</summary>
      TransactedDB = 1,

      /// <summary>The source of the data is a database that does not support transactions.</summary>
      NonTransactedDB = 2,

      /// <summary>
      ///     <para>Unclassified source type—data will be in a file group.</para>
      ///		<para>This is the default source type.</para>
      /// </summary>
      Other = 3

   };
}
