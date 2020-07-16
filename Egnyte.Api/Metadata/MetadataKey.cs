using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Egnyte.Api.Metadata
{
    public enum MetadataKeyType
    {
        Integer,
        String,
        Decimal,
        Date,
        Enum
    }

    public class MetadataKey
    {
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string KeyName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public MetadataKeyType Type { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Data { get; set; }

        [JsonProperty("displayName", NullValueHandling = NullValueHandling.Ignore)]
        public string[] DisplayName { get; set; }

        [JsonProperty("helpText", NullValueHandling = NullValueHandling.Ignore)]
        public string[] HelpText { get; set; }

        public MetadataKey(MetadataKeyType type)
        {
            this.Type = type;
        }

        public MetadataKey(string key, MetadataKeyType type) : this(type)
        {
            this.KeyName = key;
        }

        [JsonConstructor]
        public MetadataKey(string keyName, MetadataKeyType type, string[] data, string[] displayName, string[] helpText)
        {
            KeyName = keyName;
            Type = type;
            Data = data;
            DisplayName = displayName;
            HelpText = helpText;
        }

        public MetadataKey()
        {
        }
    }
}