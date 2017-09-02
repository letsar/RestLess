using System;
using System.Collections.Generic;

namespace DoLess.Rest.RestInterfaces
{
    [BaseUrl("/v1/app")]
    [Header("X-test", "DoLess")]
    public interface IRestApi02<T, U>
    {
        [Get("v1/app/books/{id}")]
        string GetBook(string id);

        [Post("v1/app/books")]
        void CreateBook(string book);

        [Put("v1/app/books/{id}")]
        void UpdateBook(List<string> id, [UrlId("queries")] string name);

        [Delete("v1/app/books/{id}")]
        void DeleteBook(List<string> id, [Header("X-test")] string name);
    }
}
