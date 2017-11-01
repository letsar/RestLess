using System.Net.Http;
using System.Threading.Tasks;

namespace RestLess.Tests.Interfaces
{
    public interface IApi04
    {
        Task<HttpResponseMessage> GetAsync();
    }
}
