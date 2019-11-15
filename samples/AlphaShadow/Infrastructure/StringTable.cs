
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaShadow
{
   public class StringTable 
   {
      private IList<string> m_labels;
      private IList<string> m_values;

      public StringTable()
         : this(Enumerable.Empty<string>(), Enumerable.Empty<object>())
      {         
      }


      public StringTable(IEnumerable<string> labels, IEnumerable<object> values)
      {
         if (labels == null)
            throw new ArgumentNullException(nameof(labels), "labels is null.");

         if (values == null)
            throw new ArgumentNullException(nameof(values), "values is null.");

         m_labels = new List<string>(labels);
         m_values = new List<string>(values.Select(v => v.ToString()));

         if (m_labels.Count != m_values.Count)
            throw new ArgumentException("There must be an equal number of labels and values.");
      }

      public void Add(string label, object value)
      {
         m_labels.Add(label);
         m_values.Add(value == null ? null : value.ToString());
      }

      public int Count
      {
         get
         {
            return m_labels.Count;
         }
      }

      public IList<string> Labels
      {
         get
         {
            return m_labels;
         }
      }

      public IList<string> Values
      {
         get
         {
            return m_values;
         }
      }
   }
}
