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
    public class Api01Tests
    {
        [Test]
        [TestCaseSource(nameof(ShouldBeHttpMethodTestCases))]
        public async Task ShouldBeHttpMethod(HttpMethod httpMethod, Func<IApi01, Task<HttpResponseMessage>> sendRequestAsync)
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(httpMethod, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi01 restClient = RestClient.For<IApi01>(url, settings);

            var httpResponse = await sendRequestAsync(restClient);

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        private static readonly object[] ShouldBeHttpMethodTestCases =
        {
            new TestCaseData(HttpMethod.Delete, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.DeleteAsync())).SetName("ShouldBeDelete"),
            new TestCaseData(HttpMethod.Get, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.GetAsync())).SetName("ShouldBeGet"),
            new TestCaseData(HttpMethod.Head, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.HeadAsync())).SetName("ShouldBeHead"),
            new TestCaseData(HttpMethod.Options, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.OptionsAsync())).SetName("ShouldBeOptions"),
            new TestCaseData(new HttpMethod("PATCH"), (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.PatchAsync())).SetName("ShouldBePatch"),
            new TestCaseData(HttpMethod.Post, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.PostAsync())).SetName("ShouldBePost"),
            new TestCaseData(HttpMethod.Put, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.PutAsync())).SetName("ShouldBePut"),
            new TestCaseData(HttpMethod.Trace, (Func<IApi01, Task<HttpResponseMessage>>)((IApi01 x) => x.TraceAsync())).SetName("ShouldBeTrace")
        };
    }
}
