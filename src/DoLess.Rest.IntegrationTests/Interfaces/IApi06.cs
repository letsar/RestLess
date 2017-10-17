using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    [Header("X-DoLess", "Rest")]
    [Header("X-DoLess-Scope", "Interface")]
    [Header("X-DoLess-ApiKey", "apiKey", true)]
    public interface IApi06
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetInterfaceHeaderAsync();

        [Header("X-DoLess-Scope", "Method")]
        [Get("api/posts")]
        Task<HttpResponseMessage> GetMethodHeaderAsync();

        [Header("X-DoLess-Scope", "Method")]
        [Get("api/posts")]
        Task<HttpResponseMessage> GetParameterHeaderAsync([HeaderValue("X-DoLess-Scope")]string header);        
    }
}
