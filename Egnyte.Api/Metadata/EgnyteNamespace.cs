using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Egnyte.CoreApi.Metadata
{
    public class EgnyteNamespace
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
        [JsonProperty("scope")]
        public NamespaceScope Scope { get; set; }

        [JsonProperty("keys")]
        public MetadataKey[] Keys { get; set; }

        public EgnyteNamespace(string displayName, NamespaceScope scope)
        {
            this.DisplayName = displayName;
            this.Scope = scope;
        }

        public EgnyteNamespace(string name, string displayName, NamespaceScope scope, MetadataKey[] keys)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.Scope = scope;
            this.Keys = keys;
        }

    }
}
