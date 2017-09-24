using System;
using System.Threading.Tasks;

namespace DoLess.Rest.Sample
{
    public interface IRestApi01
    {
        [Get("/v1/app/books/{id}")]
        Task<string> GetBook(string id);

        [Get("/v1/app/books/{id}")]
        Task<string> GetBook2(string id);
    }
}
