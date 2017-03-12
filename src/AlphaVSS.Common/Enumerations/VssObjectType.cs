

using System;

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///		The <see cref="VssObjectType"/> enumeration is used by requesters to identify an object as 
   ///		a shadow copy set, shadow copy, or provider.
   /// </summary>
   public enum VssObjectType
   {
      /// <summary>
      /// 	<para>
      /// 	    The object type is not known.
      /// 	</para>
      /// 	<para>
      /// 		This indicates an application error.
      /// 	</para>
      /// </summary>
      Unknown = 0,

      /// <summary>
      /// 	<para>
      /// 		The interpretation of this value depends on whether it is used as an 
      /// 		input to a VSS method or returned as an output from a VSS method.
      /// 	</para>
      /// 	<para>
      /// 		When used as an input to a VSS method, it indicates that the method is 
      /// 		not restricted to any particular object type, but should act on all 
      /// 		appropriate objects. In this sense, <see cref="None"/> can be thought 
      /// 		of as a wildcard input.
      /// 	</para>
      /// 	<para>
      /// 		When returned as an output, the object type is not known and means that 
      /// 		there has been an application error.
      /// 	</para>
      /// </summary>
      None = 1,

      /// <summary>Shadow copy set.</summary>
      SnapshotSet = 2,

      /// <summary>Shadow copy.</summary>
      Snapshot = 3,

      /// <summary>Shadow copy provider.</summary>		
      Provider = 4,
   };
}