namespace Egnyte.Api.Files
{
    using Newtonsoft.Json;

    public class FileLock
    {
        [JsonProperty(PropertyName = "timeout")]
        public long Timeout { get; set; }

        [JsonProperty(PropertyName = "lock_token")]
        public string LockToken { get; set; }
    }
}