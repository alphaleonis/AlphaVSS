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
using Alphaleonis.Win32.Vss;

namespace AlphaShadow
{
   class VssWriterDescriptor : IDisposable
   {
      private List<VssComponentDescriptor> m_components;
      private IUIHost m_host;

      public VssWriterDescriptor(IUIHost host, IVssExamineWriterMetadata writerMetadata)
      {
         if (writerMetadata == null)
            throw new ArgumentNullException("writerMetadata", "writerMetadata is null.");

         m_host = host;
         WriterMetadata = writerMetadata;
         m_components = new List<VssComponentDescriptor>(writerMetadata.Components.Select(c => new VssComponentDescriptor(host, WriterMetadata.WriterName, c)));         

         // Discover top-level components
         for (int i = 0; i < m_components.Count; i++)
         {
            m_components[i].IsTopLevel = true;
            for (int j = 0; j < m_components.Count; j++)
            {
               if (m_components[j].IsAncestorOf(m_components[i]))
                  m_components[i].IsTopLevel = false;
            }
         }
      }

      public IVssExamineWriterMetadata WriterMetadata { get; private set; }

      public IList<VssComponentDescriptor> ComponentDescriptors
      {
         get
         {
            return m_components;
         }
      }

      public bool IsExcluded { get; set; }

      public void Dispose()
      {
         if (WriterMetadata != null)
         {
            WriterMetadata.Dispose();
            WriterMetadata = null;
         }
      }      

      internal void InitializeComponentsForRestore(IVssWriterComponents components)
      {
         // Erase the current list of components for this writer.
         ComponentDescriptors.Clear();         

         // Enumerate the components from the BC document
         foreach (IVssComponent component in components.Components)
         {
            VssComponentDescriptor desc = new VssComponentDescriptor(m_host, this.WriterMetadata.WriterName, component);
            m_host.WriteLine("Found component available for restore: \"{0}\"", desc.FullPath);
            ComponentDescriptors.Add(desc);
         }
      }
   }
}
