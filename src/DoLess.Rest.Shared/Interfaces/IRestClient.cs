using System.Net.Http;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents a REST client.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Gets or sets the http client.
        /// </summary>
        HttpClient HttpClient { get; set; }

        /// <summary>
        /// Gets or sets the REST settings.
        /// </summary>
        RestSettings Settings { get; set; }
    }
}
