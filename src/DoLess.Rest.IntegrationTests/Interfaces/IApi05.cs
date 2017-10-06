using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DoLess.Rest.IntegrationTests.Entities;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi05
    {
        [Get("api/posts")]
        Task GetAsync();

        [Get("api/posts")]
        Task<HttpResponseMessage> GetHttpResponseMessageAsync();

        [Get("api/posts")]
        Task<string> GetStringAsync();

        [Get("api/posts")]
        Task<byte[]> GetByteArrayAsync();

        [Get("api/posts")]
        Task<Stream> GetStreamAsync();

        [Get("api/posts")]
        Task<bool> GetBoolIsOkAsync();

        [Get("api/ko")]
        Task<bool> GetBoolIsKoAsync();

        [Get("api/students/1")]
        Task<Person> GetStudentAsync();
    }
}
