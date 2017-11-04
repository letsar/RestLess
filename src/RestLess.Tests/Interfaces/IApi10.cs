using System.Net.Http;
using System.Threading.Tasks;
using RestLess.Tests.Entities;

namespace RestLess.Tests.Interfaces
{
    public interface IApi10
    {
        [Get("api{?pagination*}")]
        Task<HttpResponseMessage> GetResponseWithObjectInSubNamespaceAsync(SubNamespaceObject pagination);
    }
}
