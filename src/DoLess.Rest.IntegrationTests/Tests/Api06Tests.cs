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
    public class Api06Tests
    {
        private const string ApiKey = "dflksghrenfdf";

        [Test]
        [TestCaseSource(nameof(ShouldHaveHeaderTestCases))]
        public async Task ShouldBeHttpMethod(string headerValue, Func<IApi06, Task<HttpResponseMessage>> sendRequestAsync)
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };
            settings.CustomParameters.Add("apiKey", ApiKey);

            IApi06 restClient = RestClient.For<IApi06>(url, settings);

            var httpResponse = await sendRequestAsync(restClient);

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            httpResponse.RequestMessage
                        .Headers
                        .GetValues("X-DoLess-Scope")
                        .Should()
                        .Contain(headerValue);

            httpResponse.RequestMessage
                        .Headers
                        .GetValues("X-DoLess-ApiKey")
                        .Should()
                        .Contain(ApiKey);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static readonly object[] ShouldHaveHeaderTestCases =
        {
            new TestCaseData("Interface", (Func<IApi06, Task<HttpResponseMessage>>)((IApi06 x) => x.GetInterfaceHeaderAsync())).SetName("ShouldHaveInterfaceHeader"),
            new TestCaseData("Method", (Func<IApi06, Task<HttpResponseMessage>>)((IApi06 x) => x.GetMethodHeaderAsync())).SetName("ShouldHaveMethodHeader"),
            new TestCaseData("Param", (Func<IApi06, Task<HttpResponseMessage>>)((IApi06 x) => x.GetParameterHeaderAsync("Param"))).SetName("ShouldHaveParamHeader")
        };
    }
}
