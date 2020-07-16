using System;
using System.Net.Http;
using System.Threading.Tasks;
using Egnyte.Api.Common;

namespace Egnyte.Api.Authentication
{
    public class AuthenticationClient : BaseClient
    {
        private const string UserInfoMethod = "/pubapi/v1/userinfo";
        private const string TokensMethod = "/pubapi/v1/tokens";

        internal AuthenticationClient(HttpClient httpClient, string domain = "", string host = "") : base(httpClient, domain, host)
        {
        }

        /// <summary>
        /// Obtains user info for the current OAuth access token
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> UserInfo()
        {
            var uriBuilder = BuildUri(UserInfoMethod);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            var serviceHandler = new ServiceHandler<UserInfo>(httpClient);
            var response = await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return response.Data;
        }

        /// <summary>
        /// Revoke access to an OAuth token
        /// </summary>
        /// <param name="token">The OAuth access token you would like to revoke</param>
        /// <returns></returns>
        public async Task<bool> RevokeToken(string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            var uriBuilder = BuildUri(TokensMethod + "/revoke");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uriBuilder.Uri);

            httpRequest.Content = new StringContent(
                string.Format("token={0}", token),
                System.Text.Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var serviceHandler = new ServiceHandler<string>(httpClient);
            await serviceHandler.SendRequestAsync(httpRequest).ConfigureAwait(false);

            return true;
        }
    }
}