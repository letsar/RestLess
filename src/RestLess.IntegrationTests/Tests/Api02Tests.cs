using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestLess.IntegrationTests.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace RestLess.IntegrationTests.Tests
{
    [TestFixture]
    public class Api02Tests
    {
        [Test]
        [TestCaseSource(nameof(ShouldBeRightUrlTestCases))]
        public async Task ShouldBeRightUrl<T>(string relativeUrl, Func<T, Task<HttpResponseMessage>> sendRequestAsync)
            where T : class
        {
            string url = "http://example.org";
            string fullUrl = url + relativeUrl;
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, fullUrl)
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp                
            };
            settings.CustomParameters.Add("api_key", "4ab542de1f");

            T restClient = RestClient.For<T>(url, settings);

            var httpResponse = await sendRequestAsync(restClient);

            httpResponse.RequestMessage
                        .RequestUri
                        .OriginalString
                        .ShouldBeEquivalentTo(fullUrl);

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static readonly object[] ShouldBeRightUrlTestCases =
        {
            new TestCaseData("/api/posts", (Func<IApi02WithoutUriTemplatePrefix, Task<HttpResponseMessage>>)((IApi02WithoutUriTemplatePrefix x) => x.GetAsync())).SetName("IApi02WithoutBaseUrl 01"),
            new TestCaseData("/api/posts", (Func<IApi02WithUriTemplatePrefix01, Task<HttpResponseMessage>>)((IApi02WithUriTemplatePrefix01 x) => x.GetAsync())).SetName("IApi02WithUriTemplatePrefix01"),
            new TestCaseData("/api/posts", (Func<IApi02WithUriTemplatePrefix02, Task<HttpResponseMessage>>)((IApi02WithUriTemplatePrefix02 x) => x.GetAsync())).SetName("IApi02WithUriTemplatePrefix02"),
            new TestCaseData("/api/posts/suffix", (Func<IApi02WithUriTemplatePrefixAndSuffix, Task<HttpResponseMessage>>)((IApi02WithUriTemplatePrefixAndSuffix x) => x.GetAsync())).SetName("IApi02WithUriTemplatePrefixAndSuffix"),
            new TestCaseData("/api/posts?api_key=4ab542de1f", (Func<IApi02WithUriTemplateSuffix01, Task<HttpResponseMessage>>)((IApi02WithUriTemplateSuffix01 x) => x.GetAsync())).SetName("IApi02WithUriTemplateSuffix01"),
            new TestCaseData("/api/posts", (Func<IApi02WithUriTemplateSuffix02, Task<HttpResponseMessage>>)((IApi02WithUriTemplateSuffix02 x) => x.GetAsync())).SetName("IApi02WithUriTemplateSuffix02")
        };
    }
}
