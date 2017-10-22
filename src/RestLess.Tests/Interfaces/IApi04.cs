using System.Net.Http;
using System.Threading.Tasks;

namespace RestLess.IntegrationTests.Interfaces
{
    public interface IApi04
    {
        Task<HttpResponseMessage> GetAsync();
    }
}
