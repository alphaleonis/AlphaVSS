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

namespace Alphaleonis.Win32.Vss
{
   /// <summary>
   ///     The <see cref="VssProviderProperties"/> class specifies shadow copy provider properties.
   /// </summary>
   [Serializable]
   public class VssProviderProperties
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VssProviderProperties"/> class.
      /// </summary>
      /// <param name="providerId">The provider id.</param>
      /// <param name="providerName">Name of the provider.</param>
      /// <param name="providerType">Type of the provider.</param>
      /// <param name="providerVersion">The provider version.</param>
      /// <param name="providerVersionId">The provider version id.</param>
      /// <param name="classId">The class id.</param>
      public VssProviderProperties(Guid providerId, string providerName, VssProviderType providerType, string providerVersion, Guid providerVersionId, Guid classId)
      {
         ProviderId = providerId;
         ProviderName = providerName;
         ProviderType = providerType;
         ProviderVersion = providerVersion;
         ProviderVersionId = providerVersionId;
         ClassId = classId;
      }

      #region Public Properties

      /// <summary>Identifies the provider who supports shadow copies of this class.</summary>
      public Guid ProviderId { get; private set; }

      /// <summary>The provider name.</summary>
      public string ProviderName { get; private set; }

      /// <summary>The provider type. See <see cref="VssProviderType"/> for more information.</summary>
      public VssProviderType ProviderType { get; private set; }

      /// <summary>The provider version in readable format.</summary>
      public string ProviderVersion { get; private set; }

      /// <summary>A <see cref="Guid"/> uniquely identifying the version of a provider.</summary>
      public Guid ProviderVersionId { get; private set; }

      /// <summary>Class identifier of the component registered in the local machine's COM catalog.</summary>
      public Guid ClassId { get; private set; }

      #endregion
   }
}
