/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
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
            throw new ArgumentNullException("labels", "labels is null.");

         if (values == null)
            throw new ArgumentNullException("values", "values is null.");

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
