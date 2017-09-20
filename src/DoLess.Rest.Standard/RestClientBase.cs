using System.Net.Http;

namespace DoLess.Rest
{
    public abstract class RestClientBase : IRestClient
    {
        protected RestClientBase() { }

        HttpClient IRestClient.HttpClient { get; set; }

        RestSettings IRestClient.Settings { get; set; }
    }
}
