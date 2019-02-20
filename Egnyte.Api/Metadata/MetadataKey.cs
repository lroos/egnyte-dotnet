using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Egnyte.CoreApi.Metadata
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
    }
}
