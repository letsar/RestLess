using System.Net.Http;

namespace RestLess.Generated
{
    /// <summary>
    /// Contains methods to create a <see cref="IRestRequest"/>.
    /// </summary>
    public static partial class RestRequest
    {
        private static readonly HttpMethod HttpMethodPatch = new HttpMethod("PATCH");

        /// <summary>
        /// Creates a DELETE request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Delete(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Delete, client);
        }

        /// <summary>
        /// Creates a GET request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Get(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Get, client);
        }

        /// <summary>
        /// Creates a HEAD request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Head(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Head, client);
        }

        /// <summary>
        /// Creates a OPTIONS request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Options(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Options, client);
        }

        /// <summary>
        /// Creates a PATCH request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Patch(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethodPatch, client);
        }

        /// <summary>
        /// Creates a POST request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Post(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Post, client);
        }

        /// <summary>
        /// Creates a PUT request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Put(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Put, client);
        }

        /// <summary>
        /// Creates a TRACE request.
        /// </summary>
        /// <param name="client">The REST client.</param>
        /// <returns></returns>
        public static IRestRequest Trace(IRestClient client)
        {
            return new RestLess.Internal.RestRequest(HttpMethod.Trace, client);
        }
    }
}
