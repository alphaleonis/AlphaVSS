
using System;

namespace AlphaShadow
{
   [Flags]
   public enum OptionTypes
   {
      ValueProhibited = 0x0, 
      SingleValueAllowed = 1,
      MultipleValuesAllowed = 2,
      SingleValueRequired = Required | SingleValueAllowed,
      MultipleValuesRequired = Required | MultipleValuesAllowed,
      Required = 0x10
   }
}
