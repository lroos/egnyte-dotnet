using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Egnyte.Api.Tests.Authentication
{
    [TestFixture]
    public class TokenTests
    {
        private const string TokenRequest = @"token=testtoken";

        [Test]
        public async Task RevokeToken_ReturnsSuccess()
        {
            var httpHandlerMock = new HttpMessageHandlerMock();
            var httpClient = new HttpClient(httpHandlerMock);

            httpHandlerMock.SendAsyncFunc =
                (request, cancellationToken) =>
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(string.Empty)
                    });

            var egnyteClient = new EgnyteClient("token", "acme", httpClient);
            var revoked = await egnyteClient.Auth.RevokeToken("testtoken");

            Assert.AreEqual(true, revoked);

            var requestMessage = httpHandlerMock.GetHttpRequestMessage();
            Assert.AreEqual("https://acme.egnyte.com/pubapi/v1/tokens/revoke", requestMessage.RequestUri.ToString());

            var content = httpHandlerMock.GetRequestContentAsString();
            Assert.AreEqual(TokenRequest, content);
        }

        [Test]
        public async Task RevokeToken_ThrowsArgumentNullException_WhenTokenEmpty()
        {
            var httpClient = new HttpClient(new HttpMessageHandlerMock());
            var egnyteClient = new EgnyteClient("token", "acme", httpClient);

            var exception = await AssertExtensions.ThrowsAsync<ArgumentNullException>(
                () => egnyteClient.Auth.RevokeToken(null));

            Assert.IsTrue(exception.Message.Contains("token"));
            Assert.IsNull(exception.InnerException);
        }
    }
}