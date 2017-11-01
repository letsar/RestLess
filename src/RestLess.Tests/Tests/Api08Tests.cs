using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestLess.Tests.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace RestLess.Tests.Tests
{
    [TestFixture]
    public class Api08Tests
    {
        private const string ApiKey = "fe254zyty";

        [Test]
        [TestCaseSource(nameof(ShouldBeHttpMethodTestCases))]
        public async Task ShouldHaveRightUrl(string paramName, Func<IApi08, Task<HttpResponseMessage>> sendRequestAsync)
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + $"/api/posts?{paramName}={ApiKey}")
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };
            settings.CustomParameters.Add("customKey", ApiKey);

            IApi08 restClient = RestClient.For<IApi08>(url, settings);

            var httpResponse = await sendRequestAsync(restClient);

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static readonly object[] ShouldBeHttpMethodTestCases =
        {
            new TestCaseData("apiKey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get01Async(ApiKey))).SetName("ParameterNameSameCase"),
            new TestCaseData("apiKey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get02Async(ApiKey))).SetName("ParameterNameDifferentCase"),
            new TestCaseData("apiKey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get03Async(ApiKey))).SetName("AttributeNameSameCase"),
            new TestCaseData("apiKey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get04Async(ApiKey))).SetName("AttributeNameDifferentCase"),
            new TestCaseData("customKey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get05Async())).SetName("CustomParameterNameSameCase"),
            new TestCaseData("customkey", (Func<IApi08, Task<HttpResponseMessage>>)((IApi08 x) => x.Get06Async())).SetName("CustomParameterNameDifferentCase"),
        };
    }
}
