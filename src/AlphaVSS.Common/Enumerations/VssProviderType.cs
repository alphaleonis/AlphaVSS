

namespace Alphaleonis.Win32.Vss
{
   /// <summary>The <see cref="VssProviderType"/> enumeration specifies the provider type.</summary>
   public enum VssProviderType
   {
      /// <summary>
      /// 	<para>
      /// 	    The provider type is unknown.
      /// 	</para>
      /// 	<para>
      /// 		This indicates an error in the application or the VSS service, or that no provider is available.
      /// 	</para>
      /// </summary>
      Unknown = 0,

      /// <summary>The default provider that ships with Windows.</summary>
      System = 1,

      /// <summary>A software-based provider.</summary>
      Software = 2,

      /// <summary>A hardware-based provider.</summary>
      Hardware = 3
   };
}
