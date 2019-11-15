
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
         m_host = host ?? throw new ArgumentNullException(nameof(host), $"{nameof(host)} is null.");
         WriterMetadata = writerMetadata ?? throw new ArgumentNullException(nameof(writerMetadata), $"{nameof(writerMetadata)} is null.");
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
