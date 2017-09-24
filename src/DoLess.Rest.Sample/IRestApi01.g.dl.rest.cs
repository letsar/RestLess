using System;
using System.Threading.Tasks;
using DoLess.Rest.Sample;

namespace DoLess.Rest.Generated
{
    internal sealed class RestClientForIRestApi01 : RestClientBase, IRestApi01
    {
        public Task<string> GetBook(string id)
        {
            return RestRequest.Get(this)
                              .WithUriTemplate("/v1/app/books/{id}")
                              .WithParameter("id", id)
                              .ReadAsStringAsync();
        }

        public Task<string> GetBook2(string id)
        {
            return RestRequest.Get(this)
                              .WithUriTemplate("/v1/app/books/{id}")
                              .WithParameter("id", id)
                              .ReadAsStringAsync();
        }
    }
}