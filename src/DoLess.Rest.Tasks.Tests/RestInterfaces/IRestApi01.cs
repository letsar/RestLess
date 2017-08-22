using System;
using DoLess.Rest.Tasks.Tests.Constants;

namespace DoLess.Rest.RestInterfaces
{
    public interface IRestApi01
    {
        [Get(ApiConstants.Version + "/app/books/{id}")]
        string GetBook(string id);
    }
}
