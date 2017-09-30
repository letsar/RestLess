using System.Net.Http;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi01
    {
        [Delete("api/posts")]
        Task<HttpResponseMessage> DeleteAsync();

        [Get("api/posts")]
        Task<HttpResponseMessage> GetAsync();

        [Head("api/posts")]
        Task<HttpResponseMessage> HeadAsync();

        [Options("api/posts")]
        Task<HttpResponseMessage> OptionsAsync();

        [Patch("api/posts")]
        Task<HttpResponseMessage> PatchAsync();

        [Post("api/posts")]
        Task<HttpResponseMessage> PostAsync();

        [Put("api/posts")]
        Task<HttpResponseMessage> PutAsync();

        [Trace("api/posts")]
        Task<HttpResponseMessage> TraceAsync();
    }
}
