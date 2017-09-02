using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DoLess.Rest.Tasks.Tests.RestInterfaces
{
    public interface IRestApi03<T>
    {
        [Get("v1/app/books/{id}")]
        Task<T> Get01(string id);
    }
}
