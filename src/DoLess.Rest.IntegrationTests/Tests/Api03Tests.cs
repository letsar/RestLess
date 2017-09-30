using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DoLess.Rest.IntegrationTests.Interfaces;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace DoLess.Rest.IntegrationTests.Tests
{
    [TestFixture]
    public class Api03Tests
    {
        [Test]
        public void ShoulBeCancelled()
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi03 restClient = RestClient.For<IApi03>(url, settings);

            CancellationToken cancellationToken = new CancellationToken(true);
            Assert.ThrowsAsync<TaskCanceledException>(async () => await restClient.GetCancellableAsync(cancellationToken));
        }
    }
}
