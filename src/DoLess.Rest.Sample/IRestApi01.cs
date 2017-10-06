using System;
using System.Threading.Tasks;
using DoLess.Rest.Sample.ConstantsNamespace;

namespace DoLess.Rest.Sample
{
    [BaseUrl(Constants.BaseApi)]
    public interface IRestApi01
    {
        [Get("/v1/app/books/{id}")]
        Task<string> GetBook(string id);

        [Get("/v1/app/books/{id}")]
        Task<string> GetBook2(string id);
    }
}
