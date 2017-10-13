using System.Net.Http;

namespace DoLess.Rest.Generated
{
    /// <summary>
    /// Represents the base of a REST client.
    /// </summary>
    public abstract class RestClientBase : IRestClient
    {
        protected RestClientBase() { }

        HttpClient IRestClient.HttpClient { get; set; }

        RestSettings IRestClient.Settings { get; set; }
    }
}
