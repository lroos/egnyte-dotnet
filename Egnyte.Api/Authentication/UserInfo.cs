using Newtonsoft.Json;

namespace Egnyte.Api.Authentication
{
    public class UserInfo
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}