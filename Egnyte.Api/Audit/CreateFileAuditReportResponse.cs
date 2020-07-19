using Newtonsoft.Json;

namespace Egnyte.Api.Audit
{
    internal class CreateFileAuditReportResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}