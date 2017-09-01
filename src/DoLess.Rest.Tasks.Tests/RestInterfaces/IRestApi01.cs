using System;
using DoLess.Rest.Tasks.Tests.Constants;

namespace DoLess.Rest.RestInterfaces
{
    public interface IRestApi01
    {
        [Get("v1/app/books/{id}")]
        string GetBook([Path] string id);
    }
}
