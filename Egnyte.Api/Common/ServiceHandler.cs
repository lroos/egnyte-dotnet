namespace Egnyte.Api.Common
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Egnyte.Api.Shared.Common;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class ServiceHandler<T> where T : class
    {
        private readonly HttpClient httpClient;

        public ServiceHandler(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ServiceResponse<T>> SendRequestAsync(HttpRequestMessage request)
        {
            request.RequestUri = ApplyAdditionalUrlMapping(request.RequestUri);
            var response = await httpClient.SendAsync(request).ConfigureAwait(false);
            var rawContent = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : null;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (typeof(T) == typeof(string))
                    {
                        return new ServiceResponse<T>
                        {
                            Data = rawContent as T,
                            Headers = response.GetResponseHeaders()
                        };
                    }

                    return new ServiceResponse<T>
                    {
                        Data = JsonConvert.DeserializeObject<T>(rawContent),
                        Headers = response.GetResponseHeaders()
                    };
                }
                catch (Exception e)
                {
                    throw new EgnyteApiException(
                        rawContent,
                        response,
                        e);
                }
            }

            if (response.Content?.Headers.ContentType.MediaType == "application/json")
            {
                try
                {
                    dynamic json = JValue.Parse(rawContent);
                    string errorMessage;

                    if (json.formErrors != null)
                    {
                        var errorContent = JsonConvert.DeserializeObject<ErrorResponse>(rawContent);
                        errorMessage = errorContent.FormErrors
                            .Union(errorContent.InputErrors.SelectMany(kvp => kvp.Value))
                            .FirstOrDefault()?.Message;

                        var inputErrors = from kvp in errorContent.InputErrors
                                          from error in kvp.Value
                                          select string.Format("{0}: {1}", kvp.Key, error.Message);

                        errorMessage = string.Join("; ", errorContent.FormErrors.Select(error => error.Message).Union(inputErrors)) + ".";
                    }
                    else
                    {
                        errorMessage = json.errorMessage ?? json.message ?? json.error;
                    }

                    rawContent = errorMessage ?? rawContent;
                }
                catch
                {
                }
            }

            throw new EgnyteApiException(
                    rawContent,
                    response);
        }

        public async Task<ServiceResponse<byte[]>> GetFileToDownload(HttpRequestMessage request)
        {
            request.RequestUri = ApplyAdditionalUrlMapping(request.RequestUri);
            var response = await this.httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var bytes = await response.Content.ReadAsByteArrayAsync();

                return new ServiceResponse<byte[]>
                {
                    Data = bytes,
                    Headers = response.GetResponseHeaders()
                };
            }

            var rawContent = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : null;

            throw new EgnyteApiException(
                   rawContent,
                   response);
        }

        public async Task<ServiceResponse<Stream>> GetFileToDownloadAsStream(HttpRequestMessage request)
        {
            request.RequestUri = ApplyAdditionalUrlMapping(request.RequestUri);
            var response = await this.httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                return new ServiceResponse<Stream>
                {
                    Data = stream,
                    Headers = response.GetResponseHeaders()
                };
            }

            var rawContent = response.Content != null ? await response.Content.ReadAsStringAsync().ConfigureAwait(false) : null;

            throw new EgnyteApiException(
                   rawContent,
                   response);
        }

        private Uri ApplyAdditionalUrlMapping(Uri requestUri)
        {
            var url = requestUri.ToString();
            url = url.Replace("[", "%5B")
                     .Replace("]", "%5D");
            return new Uri(url);
        }
    }
}