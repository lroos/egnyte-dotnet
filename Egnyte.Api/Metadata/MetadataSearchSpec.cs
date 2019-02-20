using Newtonsoft.Json;

namespace Egnyte.CoreApi.Metadata
{
    internal class MetadataSearchSpec
    {
        [JsonProperty("type")]
        private MetadataSearchType Type { get; set; }

        [JsonProperty("has_key")]
        private bool HasKey { get; set; }

        [JsonProperty("key_with_value")]
        private MetadataKeySearchParameter[] Keys { get; set; }

        public MetadataSearchSpec(MetadataSearchType type, bool hasKey, MetadataKeySearchParameter[] keys)
        {
            this.Type = type;
            this.HasKey = hasKey;
            this.Keys = keys;
        }
    }
}