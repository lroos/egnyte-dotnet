using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Egnyte.Api.Common;
using Newtonsoft.Json;

namespace Egnyte.CoreApi.Metadata
{
    public class MetadataClient : BaseClient
    {
        private const string NamespaceMethod = "/pubapi/v1/properties/namespace";

        private const string FileMethod = "/pubapi/v1/fs/ids/file";

        private const string MetadataSearchMethod = "/pubapi/v1/search";

        internal MetadataClient(HttpClient httpClient, string domain = "", string host = "") : base(httpClient, domain, host)
        {
        }

        public async Task<bool> CreateNamespace(string name, NamespaceScope scope, MetadataKey[] keys, string displayName = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            /*
            if(keys == null || keys.Length == 0)
            {
                throw new ArgumentNullException(nameof(keys));
            }*/

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new EgnyteNamespace(name, displayName, scope, keys)),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> UpdateNamespace(string name, NamespaceScope scope, string displayName = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod +  "/" + name, query);
            var httpRequest = new HttpRequestMessage(new HttpMethod("PATCH"), uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new EgnyteNamespace(displayName, scope)),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> UpdateNamespaceKey(string namespaceName, string keyName, MetadataKey key)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            if (string.IsNullOrWhiteSpace(keyName))
            {
                throw new ArgumentNullException(nameof(keyName));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod + "/" + namespaceName + "/keys/" + keyName, query);
            var httpRequest = new HttpRequestMessage(new HttpMethod("PATCH"), uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(key),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> DeleteNamespace(string namespaceName)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod + "/" + namespaceName, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, uriBuilder.Uri);

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<EgnyteNamespace> GetNamespace(string namespaceName)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod + "/" + namespaceName, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var serviceHandler = new ServiceHandler<EgnyteNamespace>(httpClient);
            var response = await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return response.Data;
        }

        public async Task<bool> CreateMetadataKey(string name, MetadataKey key)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod + "/" + name + "/keys", query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(key),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> DeleteMetadataKey(string namespaceName, string keyName)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(NamespaceMethod + "/" + namespaceName + "/keys/" + keyName, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, uriBuilder.Uri);

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> SetValuesForNamespace(string entryId, string namespaceName, Dictionary<string, string> keys)
        {
            if (string.IsNullOrWhiteSpace(entryId))
            {
                throw new ArgumentNullException(nameof(entryId));
            }

            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(FileMethod + "/" + entryId + "/properties/" + namespaceName, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(keys),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }

        public async Task<NamespaceValues> GetValuesForNamespace(string entryId, string namespaceName)
        {
            if (string.IsNullOrWhiteSpace(entryId))
            {
                throw new ArgumentNullException(nameof(entryId));
            }

            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                throw new ArgumentNullException(nameof(namespaceName));
            }

            var query = string.Empty;


            var uriBuilder = BuildUri(FileMethod + "/" + entryId + "/properties/" + namespaceName, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var serviceHandler = new ServiceHandler<NamespaceValues>(httpClient);
            var result = await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return result.Data;
        }

        public async Task<object> SearchMetadata(MetadataSearchType type, bool hasKey, MetadataKeySearchParameter[] keys)
        {            
            var query = string.Empty;


            var uriBuilder = BuildUri(MetadataSearchMethod, query);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(new MetadataSearchSpec(type, hasKey, keys)),
                    Encoding.UTF8,
                    "application/json")
            };

            var serviceHandler = new ServiceHandler<string>(httpClient);
            var result = await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return result.Data;
        }
    }
}
