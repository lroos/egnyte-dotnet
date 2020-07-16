using Newtonsoft.Json;

namespace Egnyte.Api.Metadata
{
    public class MetadataKeySearchParameter
    {
        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}