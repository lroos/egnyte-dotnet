using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Egnyte.Api.Metadata
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
        public Dictionary<string, MetadataKey> Keys { get; set; }

        public EgnyteNamespace()
        {
        }

        public EgnyteNamespace(string displayName, NamespaceScope scope)
        {
            this.DisplayName = displayName;
            this.Scope = scope;
        }

        [JsonConstructor]
        public EgnyteNamespace(string name, string displayName, NamespaceScope scope, Dictionary<string, MetadataKey> keys)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.Scope = scope;
            this.Keys = keys;
        }
    }
}