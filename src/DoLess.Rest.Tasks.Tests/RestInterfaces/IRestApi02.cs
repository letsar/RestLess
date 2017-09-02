using System;
using System.Collections.Generic;
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
    }
}
