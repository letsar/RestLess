using System;
using System.Net.Http;

/* 
 * This file is copied in your poject when you install the NuGet package DoLess.Rest.
 * It is used to let you use RestClient.For<T> before the first compilation.
 * Every time you build the project, the dependent file, RestClient.g.dl.rest.cs is updated with the last Rest implementations generated.
 * You must not modified this file.
 */

namespace DoLess.Rest
{
    /// <summary>
    /// Contains methods used to create the Rest clients.
    /// </summary>
    public static partial class RestClient
    {
        private static readonly RestClientFactory RestClientFactory;

        static RestClient()
        {
            RestClientFactory = new RestClientFactory();
            InitializeRestClientFactory();
        }

        /// <summary>
        /// Creates a Rest client using the specified <paramref name="httpClient"/> and <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="T">The interface of the Rest client.</typeparam>
        /// <param name="httpClient">The http client.</param>
        /// <param name="settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(HttpClient httpClient, RestSettings settings = null)
            where T : class
        {
            return RestClientFactory.Create<T>(httpClient, settings);
        }

        /// <summary>
        /// Creates a Rest client using the specified <paramref name="hostUri"/> and <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="T">The interface of the Rest client.</typeparam>
        /// <param name="hostUri">The host uri.</param>
        /// <param name="settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(Uri hostUri, RestSettings settings = null)
            where T : class
        {
            return RestClientFactory.Create<T>(hostUri, settings);
        }

        /// <summary>
        /// Creates a Rest client using the specified <paramref name="hostUrl"/> and <paramref name="settings"/>.
        /// </summary>
        /// <typeparam name="T">The interface of the Rest client.</typeparam>
        /// <param name="hostUrl">The host url.</param>
        /// <param name="settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(string hostUrl, RestSettings settings = null)
            where T : class
        {
            return RestClientFactory.Create<T>(hostUrl, settings);
        }

        /// <summary>
        /// This method is generated in RestClient.g.dl.rest.cs and contains the rest clients initializations.
        /// </summary>
        static partial void InitializeRestClientFactory();        
    }
}
