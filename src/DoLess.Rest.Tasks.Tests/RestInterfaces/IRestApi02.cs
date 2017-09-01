using System;
using System.Collections.Generic;
using DoLess.Rest.Tasks.Tests.Constants;

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
        void UpdateBook([Path] List<string> id, string name);

        [Delete("v1/app/books/{id}")]
        void DeleteBook([Path] List<string> id, string name);
    }
}
