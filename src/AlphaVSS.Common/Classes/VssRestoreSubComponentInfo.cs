

using System;
namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///		Represents information about a Subcomponent associated with a given component.
   /// </summary>
   [Serializable]
   public class VssRestoreSubcomponentInfo
   {
      /// <summary>
      ///     Initializes a new instance of <see cref="VssRestoreSubcomponentInfo" />.
      /// </summary>
      /// <param name="logicalPath">The logical path of the Subcomponent. This can not be empty when working with Subcomponents.</param>
      /// <param name="componentName">The name of the Subcomponent. This can not be empty.</param>
      public VssRestoreSubcomponentInfo(string logicalPath, string componentName)
      {
         LogicalPath = logicalPath;
         ComponentName = componentName;
      }

      #region Public Properties

      /// <summary>The logical path of the Subcomponent. This can not be empty when working with Subcomponents.</summary>
      public string LogicalPath { get; private set; }

      /// <summary>The name of the Subcomponent. This can not be empty.</summary>
      public string ComponentName { get; private set; }

      #endregion
   };



}
