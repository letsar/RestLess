using System;
using System.Threading.Tasks;

namespace RestLess.RestInterfaces
{
    public interface IRestApi01
    {
        [Get("v1/app/books/{id}")]
        Task<string> GetBook(string id);
    }
}
