using System.Collections.Generic;
using Newtonsoft.Json;

namespace Egnyte.Api.Shared.Common
{
    public class ErrorResponse
    {
        [JsonProperty(PropertyName = "formErrors")]
        public List<Error> FormErrors { get; set; } = new List<Error>();

        [JsonProperty(PropertyName = "inputErrors")]
        public Dictionary<string, List<Error>> InputErrors { get; set; } = new Dictionary<string, List<Error>>();
    }

    public class Error
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
    }
}