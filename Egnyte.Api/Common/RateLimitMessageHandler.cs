using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Egnyte.Api.Common
{
    public class RateLimitMessageHandler : DelegatingHandler
    {
        public RateLimitMessageHandler()
            : this(new HttpClientHandler())
        {
        }

        public RateLimitMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            do
            {
                // base.SendAsync calls the inner handler
                var response = await base.SendAsync(request, cancellationToken);

                // Egnyte uses 403 status for over QPS
                if (response.StatusCode == HttpStatusCode.Forbidden
                    && response.Headers.TryGetValues("X-Mashery-Error-Code", out var values)
                    && values.Contains("ERR_403_DEVELOPER_OVER_QPS"))
                {
                    // Currently QPS = 2
                    var seconds = 1;

                    if (response.Headers.TryGetValues("Retry-After", out values)
                        && int.TryParse(values.FirstOrDefault(), out seconds))
                    {
                    }

                    await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
                    continue;
                }

                // Not something we can retry, return the response as is
                return response;
            }
            while (true);
        }
    }
}