using System.Net.Http;

namespace DoLess.Rest
{
    public interface IRestClient
    {
        HttpClient HttpClient { get; set; }

        RestSettings Settings { get; set; }
    }
}
