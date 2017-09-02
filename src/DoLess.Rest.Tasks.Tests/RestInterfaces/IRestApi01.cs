using System;
using System.Threading.Tasks;

namespace DoLess.Rest.RestInterfaces
{
    public interface IRestApi01
    {
        [Get("v1/app/books/{id}")]
        Task<string> GetBook(string id);
    }
}
