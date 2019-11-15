using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.VSWhere;
using Nuke.Common.Utilities.Collections;
[Serializable]
public partial class VSWhereResult2 : ISettingsEntity
{
   [JsonProperty]
   public virtual string InstanceId { get; internal set; }
   [JsonProperty]
   public virtual DateTime InstallDate { get; internal set; }
   [JsonProperty]
   public virtual string InstallationName { get; internal set; }
   [JsonProperty]
   public virtual string InstallationPath { get; internal set; }
   [JsonProperty]
   public virtual string InstallationVersion { get; internal set; }
   [JsonProperty]
   public virtual string ProductId { get; internal set; }
   [JsonProperty]
   public virtual string ProductPath { get; internal set; }
   [JsonProperty]
   public virtual bool? IsPreRelease { get; internal set; }
   [JsonProperty]
   public virtual string DisplayName { get; internal set; }
   [JsonProperty]
   public virtual string Description { get; internal set; }
   [JsonProperty]
   public virtual string ChannelId { get; internal set; }
   [JsonProperty]
   public virtual string ChannelUri { get; internal set; }
   [JsonProperty]
   public virtual string EnginePath { get; internal set; }
   [JsonProperty]
   public virtual string ReleaseNotes { get; internal set; }
   [JsonProperty]
   public virtual string ThirdPartyNotices { get; internal set; }
   [JsonProperty]
   public virtual DateTime UpdateDate { get; internal set; }
   [JsonProperty]
   public virtual VSWhereCatalog Catalog { get; internal set; }
   [JsonProperty]
   public virtual IReadOnlyDictionary<string, object> Properties => PropertiesInternal.AsReadOnly();
   [JsonProperty]
   internal Dictionary<string, object> PropertiesInternal { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
}
