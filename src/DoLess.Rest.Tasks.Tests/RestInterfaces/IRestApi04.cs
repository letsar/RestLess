using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoLess.Rest.RestInterfaces
{
    [BaseUrl("")]
    public interface IRestApiForTestingBaseUrl01
    {
        [Get("v1/app/books/{id}")]
        Task<string> Get01(string id);
    }

    [BaseUrl("/v1?querystring=invalid")]
    public interface IRestApiForTestingBaseUrl02
    {
        [Get("v1/app/books/{id}")]
        Task<string> Get01(string id);
    }

    [BaseUrl("/v1/{parameter}/is/invalid")]
    public interface IRestApiForTestingBaseUrl03
    {
        [Get("v1/app/books/{id}")]
        Task<string> Get01(string id);
    }

    [BaseUrl("/v1")]
    public interface IRestApiForTestingBaseUrl04
    {
        [Get("v1/app/books/{id}")]
        Task<string> Get01(string id);
    }

    [BaseUrl("v1")]
    public interface IRestApiForTestingBaseUrl05
    {
        [Get("v1/app/books/{id}")]
        Task<string> Get01(string id);
    }
}
