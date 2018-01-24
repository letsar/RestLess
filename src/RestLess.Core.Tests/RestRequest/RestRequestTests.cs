using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestLess.Generated;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using System.IO;
using Newtonsoft.Json;

namespace RestLess.Tests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        [TestCaseSource(nameof(ShouldBeRightHttpMethodTestCases))]
        public async Task ShouldBeRightHttpMethod(HttpMethod httpMethod, Func<IRestClient, IRestRequest> getRestRequest)
        {
            string url = "http://example.org";
            string relativeUrl = "/api/posts";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(httpMethod, url + relativeUrl)
                    .Respond(HttpStatusCode.OK);

            IRestClient restClient = new SimpleRestClient();
            restClient.HttpClient = new HttpClient(mockHttp);
            restClient.HttpClient.BaseAddress = new Uri(url);

            var restRequest = getRestRequest(restClient);

            var httpResponse = await restRequest.WithUriTemplate(relativeUrl)
                                                .ReadAsHttpResponseMessageAsync();

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        [TestCase("http://example.org/api/posts", "http://example.org", null, "api/posts", null)]
        [TestCase("http://example.org/api/posts", "http://example.org", null, "/api/posts", null)]
        [TestCase("http://example.org/api/posts", "http://example.org", "", "api/posts", null)]
        [TestCase("http://example.org/api/posts", "http://example.org", "", "/api/posts", null)]
        [TestCase("http://example.org/api/posts", "http://example.org", "/api", "/posts", null)]
        [TestCase("http://example.org/api/posts", "http://example.org", "api", "/posts", null)]
        [TestCase("http://example.org/api/posts/suffix", "http://example.org", "api", "/posts", "/suffix")]
        [TestCase("http://example.org/api/v2/posts?test=45", "http://example.org", "api{/version}", "/posts", "{?key,test}")]
        [TestCase("http://example.org/api/v3/posts?ctest=72", "http://example.org", "api{/cversion}", "/posts", "{?key,ctest}")]
        public async Task ShouldBeRightUrl(string expectedUrl, string hostUrl, string uriTemplatePrefix, string uriTemplate, string uriTemplateSuffix)
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, expectedUrl)
                    .Respond(HttpStatusCode.OK);

            RestSettings settings = new RestSettings();
            settings.CustomParameters.Add("cversion", "v3");
            settings.CustomParameters.Add("ctest", "72");

            IRestClient restClient = new SimpleRestClient(settings);
            restClient.HttpClient = new HttpClient(mockHttp);
            restClient.HttpClient.BaseAddress = new Uri(hostUrl);

            var restRequest = RestRequest.Get(restClient)
                                         .WithUriTemplatePrefix(uriTemplatePrefix)
                                         .WithUriTemplateSuffix(uriTemplateSuffix)
                                         .WithUriTemplate(uriTemplate)
                                         .WithUriVariable("version", "v2")
                                         .WithUriVariable("test", "45");

            var httpResponse = await restRequest.ReadAsHttpResponseMessageAsync();

            httpResponse.RequestMessage
                        .RequestUri
                        .OriginalString
                        .ShouldBeEquivalentTo(expectedUrl);

            httpResponse.StatusCode
                        .Should()
                        .Be(HttpStatusCode.OK);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldSerializeContent()
        {
            string hostUrl = "http://www.mocky.io";
            string relativeUrl = "/v2/5a68c5a32e0000be27d5b580";

            var testObject = new TestObject()
            {
                Value = "test"
            };            

            RestSettings settings = new RestSettings();
            settings.MediaTypeFormatters.Default = new TestMediaFormatter(JsonSerializer.Create(new JsonSerializerSettings()));

            IRestClient restClient = new SimpleRestClient(settings);
            restClient.HttpClient = new HttpClient();
            restClient.HttpClient.BaseAddress = new Uri(hostUrl);

            var restRequest = RestRequest.Post(restClient)
                                         .WithUriTemplate(relativeUrl)
                                         .WithContent(testObject, "testObject");

            var response = await restRequest.ReadAsObject<TestObject>();
            response.Value.ShouldBeEquivalentTo("success");                       
        }

        private static readonly object[] ShouldBeRightHttpMethodTestCases =
        {
            new TestCaseData(HttpMethod.Delete, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Delete(x))).SetName("RestRequestShouldBeDelete"),
            new TestCaseData(HttpMethod.Get, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Get(x))).SetName("RestRequestShouldBeGet"),
            new TestCaseData(HttpMethod.Head, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Head(x))).SetName("RestRequestShouldBeHead"),
            new TestCaseData(HttpMethod.Options, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Options(x))).SetName("RestRequestShouldBeOptions"),
            new TestCaseData(new HttpMethod("PATCH"), (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Patch(x))).SetName("RestRequestShouldBePatch"),
            new TestCaseData(HttpMethod.Post, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Post(x))).SetName("RestRequestShouldBePost"),
            new TestCaseData(HttpMethod.Put, (Func<IRestClient, IRestRequest>)((IRestClient x) => RestRequest.Put(x))).SetName("RestRequestShouldBePut"),
            new TestCaseData(HttpMethod.Trace, (Func<IRestClient, IRestRequest>)((IRestClient x) =>RestRequest.Trace(x))).SetName("RestRequestShouldBeTrace")
        };

        private class TestObject
        {
            public string Value { get; set; }
        }

        private class TestMediaFormatter : IMediaTypeFormatter
        {
            private readonly JsonSerializer jsonSerializer;
            public TestMediaFormatter(JsonSerializer jsonSerializer)
            {
                this.jsonSerializer = jsonSerializer;
            }

            public string MediaType => "application/json";

            public Task<T> ReadAsync<T>(TextReader reader)
            {
                using (JsonReader jsonReader = new JsonTextReader(reader))
                {
                    return Task.FromResult<T>(this.jsonSerializer.Deserialize<T>(jsonReader));
                }
            }

            public Task WriteAsync<T>(T content, TextWriter writer)
            {
                this.jsonSerializer.Serialize(writer, content, typeof(T));
                return Task.CompletedTask;
            }
        }
    }
}
