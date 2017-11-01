using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RestLess.Tests.Entities;
using RestLess.Tests.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace RestLess.Tests.Tests
{
    [TestFixture]
    public class Api05Tests
    {
        [Test]
        public async Task ShouldSendRequest()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            await restClient.GetAsync();
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallStringMethod()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            string stringResponse = await restClient.GetStringAsync();
            stringResponse.ShouldBeEquivalentTo(response);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallHttpResponseMessageMethod()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            HttpResponseMessage httpResponseMessage = await restClient.GetHttpResponseMessageAsync();
            httpResponseMessage.Content
                               .Should()
                               .BeOfType<StringContent>();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallStreamMethod()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            using (Stream stream = await restClient.GetStreamAsync())
            using (StreamReader reader = new StreamReader(stream))
            {
                var stringResponse = reader.ReadToEnd();
                stringResponse.ShouldBeEquivalentTo(response);
            }

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallByteArrayMethod()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            byte[] bytes = await restClient.GetByteArrayAsync();
            string stringResponse = Encoding.UTF8.GetString(bytes);
            stringResponse.ShouldBeEquivalentTo(response);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallIsOkMethod()
        {
            string url = "http://example.org";
            string response = "hello";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond("text/plain", response);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            bool isOk = await restClient.GetBoolIsOkAsync();
            isOk.Should().BeTrue();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallIsKoMethod()
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/ko")
                    .Respond(HttpStatusCode.InternalServerError);

            var settings = new RestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            bool isKo = await restClient.GetBoolIsKoAsync();
            isKo.Should().BeFalse();

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldCallGetStudentMethod()
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/students/1")
                    .Respond("application/json", "{'firstName':'Bob', 'lastName':'Walker', 'age': 22}");

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi05 restClient = RestClient.For<IApi05>(url, settings);

            Person student = await restClient.GetStudentAsync();
            student.FirstName
                   .ShouldBeEquivalentTo("Bob");
            student.LastName
                   .ShouldBeEquivalentTo("Walker");
            student.Age
                   .ShouldBeEquivalentTo(22);

            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
