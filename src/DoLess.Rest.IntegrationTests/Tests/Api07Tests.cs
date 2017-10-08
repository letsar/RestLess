using System;
using System.Collections.Generic;
using System.IO;
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
    public class Api07Tests
    {
        [Test]
        public async Task ShouldHaveUniqueContent01()
        {
            string url = "http://example.org";
            string content = "myContent";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi07 restClient = RestClient.For<IApi07>(url, settings);

            var response = await restClient.GetUniqueContent01Async(content);

            response.RequestMessage
                    .Content
                    .Should()
                    .BeOfType<StringContent>()
                    .Which
                    .ReadAsStringAsync()
                    .Result
                    .ShouldBeEquivalentTo(content);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldHaveUniqueContent02()
        {
            string url = "http://example.org";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi07 restClient = RestClient.For<IApi07>(url, settings);

            var form = new Dictionary<string, string>
            {
                ["firstName"] = "John",
                ["lastName"] = "Doe"
            };

            var response = await restClient.GetUniqueContent02Async(form);

            response.RequestMessage
                    .Content
                    .Should()
                    .BeOfType<FormUrlEncodedContent>()
                    .Which
                    .ReadAsStringAsync()
                    .Result
                    .ShouldBeEquivalentTo("firstName=John&lastName=Doe");

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldHaveMultipartContent01()
        {
            string url = "http://example.org";
            string content = @"--DoLessRestBoundary
Content-Disposition: form-data; name=content; filename=Api07File.xml; filename*=utf-8''Api07File.xml

﻿<?xml version=""1.0"" encoding=""utf-8"" ?>
<Project/>
--DoLessRestBoundary--
";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi07 restClient = RestClient.For<IApi07>(url, settings);

            var fileInfo = new FileInfo("Assets\\Api07File.xml");

            var response = await restClient.GetMultipartContent01Async(fileInfo);

            response.RequestMessage
                    .Content
                    .Should()
                    .BeOfType<MultipartFormDataContent>()
                    .Which
                    .ReadAsStringAsync()
                    .Result
                    .ShouldBeEquivalentTo(content);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldHaveMultipartContent02()
        {
            string url = "http://example.org";

            string content = @"--DoLessRestBoundary
Content-Type: text/plain; charset=utf-8
Content-Disposition: form-data; name=content

doe
--DoLessRestBoundary
Content-Type: text/plain; charset=utf-8
Content-Disposition: form-data; name=firstName

john
--DoLessRestBoundary--
";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi07 restClient = RestClient.For<IApi07>(url, settings);

            var response = await restClient.GetMultipartContent02Async("doe", "john");

            response.RequestMessage
                    .Content
                    .Should()
                    .BeOfType<MultipartFormDataContent>()
                    .Which
                    .ReadAsStringAsync()
                    .Result
                    .ShouldBeEquivalentTo(content);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Test]
        public async Task ShouldHaveMultipartContent03()
        {
            string url = "http://example.org";

            string content = @"--DoLessRestBoundary
Content-Type: text/plain; charset=utf-8
Content-Disposition: form-data; name=content

doe
--DoLessRestBoundary
Content-Type: text/plain
Content-Disposition: form-data; name=firstName; filename=f; filename*=utf-8''f

john
--DoLessRestBoundary--
";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url + "/api/posts")
                    .Respond(HttpStatusCode.OK);

            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp
            };

            IApi07 restClient = RestClient.For<IApi07>(url, settings);

            var response = await restClient.GetMultipartContent03Async("doe", "john");

            response.RequestMessage
                    .Content
                    .Should()
                    .BeOfType<MultipartFormDataContent>()
                    .Which
                    .ReadAsStringAsync()
                    .Result
                    .ShouldBeEquivalentTo(content);

            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}
