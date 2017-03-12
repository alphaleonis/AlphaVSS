
namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssUsageType"/> enumeration specifies how the host system uses the data managed by a writer involved in a VSS operation.</summary>
   /// <remarks>Requester applications that are interested in backing up system state should look for writers with the 
   /// <see cref="VssUsageType.BootableSystemState"/> or <see cref="VssUsageType.SystemService"/> usage type.</remarks>
   public enum VssUsageType
   {
      /// <summary><para>The usage type is not known.</para><para>This indicates an error on the part of the writer.</para></summary>
      Undefined = 0,
      /// <summary>The data stored by the writer is part of the bootable system state.</summary>
      BootableSystemState = 1,
      /// <summary>The writer either stores data used by a system service or is a system service itself.</summary>
      SystemService = 2,
      /// <summary>The data is user data.</summary>
      UserData = 3,
      /// <summary>Unclassified data.</summary>
      Other = 4
   };
}
