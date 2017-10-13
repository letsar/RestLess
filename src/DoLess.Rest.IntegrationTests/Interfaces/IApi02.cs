using System.Net.Http;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi02WithoutUriTemplatePrefix
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetAsync();
    }

    [UriTemplatePrefix("api/")]
    public interface IApi02WithUriTemplatePrefix01
    {
        [Get("posts")]
        Task<HttpResponseMessage> GetAsync();
    }

    [UriTemplatePrefix("/api/")]
    public interface IApi02WithUriTemplatePrefix02
    {
        [Get("posts")]
        Task<HttpResponseMessage> GetAsync();
    }   

    [UriTemplatePrefix("/api/")]
    [UriTemplateSuffix("/suffix")]
    public interface IApi02WithUriTemplatePrefixAndSuffix
    {
        [Get("posts")]
        Task<HttpResponseMessage> GetAsync();        
    }

    [UriTemplateSuffix("{?api_key}")]
    public interface IApi02WithUriTemplateSuffix01
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetAsync();        
    }

    [UriTemplateSuffix("{?notFound}")]
    public interface IApi02WithUriTemplateSuffix02
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetAsync();
    }
}
