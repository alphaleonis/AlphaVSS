/* Copyright (c) 2008 Peter Palotas
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
            mProviderId = providerId;
            mProviderName = providerName;
            mProviderType = providerType;
            mProviderVersion = providerVersion;
            mProviderVersionId = providerVersionId;
            mClassId = classId;
        }

		/// <summary>Identifies the provider who supports shadow copies of this class.</summary>
		public Guid ProviderId { get { return mProviderId; } }

		/// <summary>The provider name.</summary>
		public string ProviderName { get { return mProviderName; } }

		/// <summary>The provider type. See <see cref="VssProviderType"/> for more information.</summary>
		public VssProviderType ProviderType { get { return mProviderType; } }

		/// <summary>The provider version in readable format.</summary>
		public string ProviderVersion { get { return mProviderVersion; } }

		/// <summary>A <see cref="Guid"/> uniquely identifying the version of a provider.</summary>
		public Guid ProviderVersionId { get { return mProviderVersionId; } }

		/// <summary>Class identifier of the component registered in the local machine's COM catalog.</summary>
		public Guid ClassId { get { return mClassId; } }
	
		private Guid mProviderId;
        private string mProviderName;
        private VssProviderType mProviderType;
        private string mProviderVersion;
        private Guid mProviderVersionId;
        private Guid mClassId;
	}
}
/*

	VssProviderProperties^ VssProviderProperties::Adopt(VSS_PROVIDER_PROP *pProp)
	{
		try
		{
			return gcnew VssProviderProperties(pProp);
		}
		finally
		{
			::CoTaskMemFree(pProp->m_pwszProviderName);
			::CoTaskMemFree(pProp->m_pwszProviderVersion);
		}
	}

	VssProviderProperties::VssProviderProperties(VSS_PROVIDER_PROP *pProp)
		:	mProviderId(ToGuid(pProp->m_ProviderId)),
			mProviderName(gcnew String(pProp->m_pwszProviderName)),
			mProviderType((VssProviderType)pProp->m_eProviderType),
			mProviderVersion(gcnew String(pProp->m_pwszProviderVersion)),
			mClassId(ToGuid(pProp->m_ClassId))
	{
	}*/