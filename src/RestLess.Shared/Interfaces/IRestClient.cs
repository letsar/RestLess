using System;
using System.Net.Http;

namespace RestLess
{
    /// <summary>
    /// Represents a REST client.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Gets or sets the http client factory.
        /// </summary>
        Func<HttpClient> HttpClientFactory { get; set; }

        /// <summary>
        /// Gets or sets the REST settings.
        /// </summary>
        RestSettings Settings { get; set; }
    }
}
