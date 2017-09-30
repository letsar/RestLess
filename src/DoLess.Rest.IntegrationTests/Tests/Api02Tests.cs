using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DoLess.Rest.IntegrationTests.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace DoLess.Rest.IntegrationTests.Tests
{
    [TestFixture]
    public class Api02Tests
    {
        [Test]
        [TestCaseSource(nameof(ShouldBeRightUrlTestCases))]
        public async Task ShouldBeRightUrl<T>(Func<T, Task<HttpResponseMessage>> sendRequestAsync)
            where T : class
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            T restClient = RestClient.For<T>(url, settings);

            var httpResponse = await sendRequestAsync(restClient);

            httpResponse.RequestMessage
                        .RequestUri
                        .OriginalString
                        .ShouldBeEquivalentTo("http://example.org/api/posts");

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static readonly object[] ShouldBeRightUrlTestCases =
        {
            new TestCaseData((Func<IApi02WithoutBaseUrl, Task<HttpResponseMessage>>)((IApi02WithoutBaseUrl x) => x.Get01Async())).SetName("IApi02WithoutBaseUrl 01"),
            new TestCaseData((Func<IApi02WithoutBaseUrl, Task<HttpResponseMessage>>)((IApi02WithoutBaseUrl x) => x.Get02Async())).SetName("IApi02WithoutBaseUrl 02"),
            new TestCaseData((Func<IApi02WithBaseUrl01, Task<HttpResponseMessage>>)((IApi02WithBaseUrl01 x) => x.Get01Async())).SetName("IApi02WithBaseUrl01 01"),
            new TestCaseData((Func<IApi02WithBaseUrl01, Task<HttpResponseMessage>>)((IApi02WithBaseUrl01 x) => x.Get02Async())).SetName("IApi02WithBaseUrl01 02"),
            new TestCaseData((Func<IApi02WithBaseUrl02, Task<HttpResponseMessage>>)((IApi02WithBaseUrl02 x) => x.Get01Async())).SetName("IApi02WithBaseUrl02 01"),
            new TestCaseData((Func<IApi02WithBaseUrl02, Task<HttpResponseMessage>>)((IApi02WithBaseUrl02 x) => x.Get02Async())).SetName("IApi02WithBaseUrl02 02"),
            new TestCaseData((Func<IApi02WithBaseUrl03, Task<HttpResponseMessage>>)((IApi02WithBaseUrl03 x) => x.Get01Async())).SetName("IApi02WithBaseUrl03 01"),
            new TestCaseData((Func<IApi02WithBaseUrl03, Task<HttpResponseMessage>>)((IApi02WithBaseUrl03 x) => x.Get02Async())).SetName("IApi02WithBaseUrl03 02"),
        };
    }
}
