using System;

namespace DoLess.Rest.RestInterfaces
{
    public interface IRestApi01
    {
        [Get("v1/app/books/{id}")]
        string GetBook(string id);
    }
}
