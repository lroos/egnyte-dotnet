namespace Egnyte.Api.Tests.Files
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class LockFileTests
    {
        private const string LockFileRequest = @"
        {
            ""action"": ""lock"",
            ""lock_token"": ""d3c09162-f76a-4bee-bb70-caaeee099a80"",
            ""lock_timeout"": 7200
        }";

        private const string LockFileResponse = @"
        {
            ""timeout"": 7199,
            ""lock_token"": ""d3c09162-f76a-4bee-bb70-caaeee099a80""
        }";

        private const string UnlockFileRequest = @"
        {
            ""action"": ""unlock"",
            ""lock_token"": ""d3c09162-f76a-4bee-bb70-caaeee099a80""
        }";

        private const string Path = "path/to/file";
        private const string Token = "d3c09162-f76a-4bee-bb70-caaeee099a80";

        [Test]
        public async Task LockFile_ReturnsSuccess()
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
                               LockFileResponse,
                               Encoding.UTF8,
                               "application/json")
                   });

            var egnyteClient = new EgnyteClient("token", "acme", httpClient);
            var fileLock = await egnyteClient.Files.LockFile(Path, Token, 7200);

            var requestMessage = httpHandlerMock.GetHttpRequestMessage();
            Assert.AreEqual("https://acme.egnyte.com/pubapi/v1/fs/" + Path, requestMessage.RequestUri.ToString());

            var content = httpHandlerMock.GetRequestContentAsString();
            Assert.AreEqual(
                TestsHelper.RemoveWhitespaces(LockFileRequest),
                TestsHelper.RemoveWhitespaces(content));

            Assert.AreEqual(Token, fileLock.LockToken);
            Assert.AreEqual(7199, fileLock.Timeout);
        }

        [Test]
        public async Task LockFile_WithNoPath_ThrowsArgumentNullException()
        {
            var httpClient = new HttpClient(new HttpMessageHandlerMock());

            var egnyteClient = new EgnyteClient("token", "acme", httpClient);

            var exception = await AssertExtensions.ThrowsAsync<ArgumentNullException>(
                () => egnyteClient.Files.LockFile(string.Empty, "token"));

            Assert.IsTrue(exception.Message.Contains("path"));
            Assert.IsNull(exception.InnerException);
        }

        [Test]
        public async Task UnlockFile_ReturnsSuccess()
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
            var isSucccess = await egnyteClient.Files.UnlockFile(Path, Token);

            Assert.AreEqual(true, isSucccess);

            var requestMessage = httpHandlerMock.GetHttpRequestMessage();
            Assert.AreEqual("https://acme.egnyte.com/pubapi/v1/fs/" + Path, requestMessage.RequestUri.ToString());

            var content = httpHandlerMock.GetRequestContentAsString();
            Assert.AreEqual(
                TestsHelper.RemoveWhitespaces(UnlockFileRequest),
                TestsHelper.RemoveWhitespaces(content));
        }

        [Test]
        public async Task UnlockFile_WithNoPathOrToken_ThrowsArgumentNullException()
        {
            var httpClient = new HttpClient(new HttpMessageHandlerMock());

            var egnyteClient = new EgnyteClient("token", "acme", httpClient);

            var exception = await AssertExtensions.ThrowsAsync<ArgumentNullException>(
                () => egnyteClient.Files.UnlockFile(null, "token"));

            Assert.IsTrue(exception.Message.Contains("path"));
            Assert.IsNull(exception.InnerException);

            exception = await AssertExtensions.ThrowsAsync<ArgumentNullException>(
                () => egnyteClient.Files.UnlockFile("path", null));

            Assert.IsTrue(exception.Message.Contains("lockToken"));
            Assert.IsNull(exception.InnerException);
        }
    }
}