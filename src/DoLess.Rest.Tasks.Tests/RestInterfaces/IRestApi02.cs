using System;
using DoLess.Rest.Tasks.Tests.Constants;

namespace DoLess.Rest.RestInterfaces
{
    public interface IRestApi02
    {
        [Get("v1/app/books/{id}")]
        string GetBook(string id);

        [Post("v1/app/books")]
        void CreateBook(string book);
    }
}
