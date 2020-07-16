using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Egnyte.Api.Tests.Authentication
{
    [TestFixture]
    public class UserInfoTests
    {
        private const string UserInfoResponse = @"
        {
          ""id"": 123,
          ""first_name"": ""Test"",
          ""last_name"": ""User"",
          ""username"": ""test""
        }";

        [Test]
        public async Task UserInfo_ReturnsCorrectResponse()
        {
            var httpHandlerMock = new HttpMessageHandlerMock();
            var httpClient = new HttpClient(httpHandlerMock);

            httpHandlerMock.SendAsyncFunc =
                (request, cancellationToken) =>
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Created,
                        Content = new StringContent(
                                UserInfoResponse,
                                Encoding.UTF8,
                                "application/json")
                    });

            var egnyteClient = new EgnyteClient("token", "acme", httpClient);
            var userInfo = await egnyteClient.Auth.UserInfo();

            Assert.AreEqual(123, userInfo.Id);
            Assert.AreEqual("Test", userInfo.FirstName);
            Assert.AreEqual("User", userInfo.LastName);
            Assert.AreEqual("test", userInfo.Username);

            var requestMessage = httpHandlerMock.GetHttpRequestMessage();
            Assert.AreEqual("https://acme.egnyte.com/pubapi/v1/userinfo", requestMessage.RequestUri.ToString());
        }
    }
}