using System.Net.Http;

namespace RestLess.Generated
{
    /// <summary>
    /// Represents the base of a REST client.
    /// </summary>
    public abstract class RestClientBase : IRestClient
    {
        /// <summary>
        /// Creates a <see cref="RestClientBase"/>.
        /// </summary>
        protected RestClientBase() { }

        HttpClient IRestClient.HttpClient { get; set; }

        RestSettings IRestClient.Settings { get; set; }
    }
}
