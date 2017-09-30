using System.Net.Http;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi02WithoutBaseUrl
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> Get01Async();

        [Get("/api/posts")]
        Task<HttpResponseMessage> Get02Async();
    }

    [BaseUrl("api")]
    public interface IApi02WithBaseUrl01
    {
        [Get("posts")]
        Task<HttpResponseMessage> Get01Async();

        [Get("/posts")]
        Task<HttpResponseMessage> Get02Async();
    }

    [BaseUrl("/api")]
    public interface IApi02WithBaseUrl02
    {
        [Get("posts")]
        Task<HttpResponseMessage> Get01Async();

        [Get("/posts")]
        Task<HttpResponseMessage> Get02Async();
    }

    [BaseUrl("/api/")]
    public interface IApi02WithBaseUrl03
    {
        [Get("posts")]
        Task<HttpResponseMessage> Get01Async();

        [Get("/posts")]
        Task<HttpResponseMessage> Get02Async();
    }
}
