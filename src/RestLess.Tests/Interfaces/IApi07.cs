using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RestLess.IntegrationTests.Interfaces
{
    public interface IApi07
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetUniqueContent01Async([Content]string content);

        [Get("api/posts")]
        Task<HttpResponseMessage> GetUniqueContent02Async([FormUrlEncodedContent]Dictionary<string, string> content);

        [Get("api/posts")]
        Task<HttpResponseMessage> GetMultipartContent01Async([Content]FileInfo content);

        [Get("api/posts")]
        Task<HttpResponseMessage> GetMultipartContent02Async([Content]string content, [Name("firstName")][Content]string content2);

        [Get("api/posts")]
        Task<HttpResponseMessage> GetMultipartContent03Async([Content]string content, [Name("firstName")][Content("f", "text/plain")]string content2);

    }
}
