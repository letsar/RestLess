using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using RestLess.Tests.Interfaces;
using FluentAssertions;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using RestLess.Tests.Entities;
using System.Net.Http.Headers;
using System.Text;

namespace RestLess.Tests.Tests
{
    [TestFixture]
    public class Api09Tests
    {
        private const string PaginationPage = "X-Pagination-Page";
        private const string PaginationPageCount = "X-Pagination-Page-Count";

        [Test]        
        public async Task ShouldObjectHaveHeaderValues()
        {
            string url = "http://example.org/api/people";
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.Expect(HttpMethod.Get, url)
                    .Respond(x =>
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new StringContent("[{'firstName':'A','lastName':'AA'},{'firstName':'B','lastName':'BB'}]", Encoding.UTF8, "application/json")
                        };

                        response.Headers.Add(PaginationPage, "1");
                        response.Headers.Add(PaginationPageCount, "2");

                        return response;
                    });


            var settings = new JsonRestSettings()
            {
                HttpMessageHandlerFactory = () => mockHttp,
                HeaderWriter = new HeaderWriter()
            };

            IApi09 restClient = RestClient.For<IApi09>(url, settings);

            var people = await restClient.GetPagedPeopleAsync();

            people.Should()
                  .HaveCount(2);

            people.Page
                  .ShouldBeEquivalentTo(1);

            people.PageCount
                  .ShouldBeEquivalentTo(2);

            mockHttp.VerifyNoOutstandingExpectation();
        }

        public class HeaderWriter : IHeaderWriter
        {
            public void Write(HttpResponseHeaders headers, object obj)
            {
                if (obj is IPagedResponse pagedResponse)
                {
                    if (headers.TryGetValue(PaginationPage, out int page))
                    {
                        pagedResponse.Page = page;
                    }

                    if (headers.TryGetValue(PaginationPageCount, out int pageCount))
                    {
                        pagedResponse.PageCount = pageCount;
                    }
                }
            }
        }
    }
}
