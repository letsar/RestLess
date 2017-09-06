using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest.RestInterfaces
{
    [BaseUrl("/v1/app")]
    [Header("X-test", "DoLess")]
    public interface IRestApi02<T, U>
    {
        [Get("v1/app/books/{id}")]
        Task<string> GetBook(string id);

        [Post("v1/app/books")]
        Task CreateBook(string book);

        [Put("v1/app/books/{id}")]
        Task UpdateBook(List<string> id, [UrlId("queries")] string name);

        [Delete("v1/app/books/{id}")]
        Task DeleteBook(List<string> id, [Header("X-test")] string name);

        [Get("v1/app/books/{id}")]
        Task<Stream> GetBook01(string id);

        [Get("v1/app/books/{id}")]
        Task<System.IO.Stream> GetBook011(string id);

        [Get("v1/app/books/{id}")]
        Task<byte[]> GetBook02(string id);

        [Get("v1/app/books/{id}")]
        Task<Byte[]> GetBook03(string id);

        [Get("v1/app/books/{id}")]
        Task<System.Byte[]> GetBook031(string id);

        [Get("v1/app/books/{id}")]
        Task<String> GetBook04(string id);

        [Get("v1/app/books/{id}")]
        Task<System.String> GetBook041(string id);

        [Get("v1/app/books/{id}")]
        Task<bool> GetBook05(string id);

        [Get("v1/app/books/{id}")]
        Task<Boolean> GetBook06(string id);

        [Get("v1/app/books/{id}")]
        Task<System.Boolean> GetBook061(string id);

        [Get("v1/app/books/{id}")]
        Task<HttpResponseMessage> GetBook07(string id);

        [Get("v1/app/books/{id}")]
        Task<System.Net.Http.HttpResponseMessage> GetBook071(string id);

        [Get("v1/app/books/{id}")]
        Task<List<string>> GetBook08(string id);

        [Get("v1/app/books/{id}")]
        Task<System.Collections.Generic.List<string>> GetBook081(string id, CancellationToken ct = default(CancellationToken));

        [Get("v1/app/books/{id}")]
        Task<List<string>> GetBook082(string id, CancellationToken ct);
    }
}
