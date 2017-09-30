using System.Net.Http;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi04
    {
        Task<HttpResponseMessage> GetAsync();
    }
}
