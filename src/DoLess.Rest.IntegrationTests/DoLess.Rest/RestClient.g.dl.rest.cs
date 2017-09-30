using System;
using System.Net.Http;
using DoLess.Rest.Generated;
using DoLess.Rest.IntegrationTests.Interfaces;

namespace DoLess.Rest
{
    public static class RestClient
    {
        private static readonly RestClientFactory RestClientFactory = null;
        /// <summary>
        /// Creates a Rest client using the specified <paramref name = "httpClient"/> and <paramref name = "settings"/>.
        /// </summary>
        /// <typeparam name = "T">The interface of the Rest client.</typeparam>
        /// <param name = "httpClient">The http client.</param>
        /// <param name = "settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(HttpClient httpClient, RestSettings settings = null)where T : class
        {
            return RestClientFactory.Create<T>(httpClient, settings);
        }

        /// <summary>
        /// Creates a Rest client using the specified <paramref name = "hostUri"/> and <paramref name = "settings"/>.
        /// </summary>
        /// <typeparam name = "T">The interface of the Rest client.</typeparam>
        /// <param name = "hostUri">The host uri.</param>
        /// <param name = "settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(Uri hostUri, RestSettings settings = null)where T : class
        {
            return RestClientFactory.Create<T>(hostUri, settings);
        }

        /// <summary>
        /// Creates a Rest client using the specified <paramref name = "hostUrl"/> and <paramref name = "settings"/>.
        /// </summary>
        /// <typeparam name = "T">The interface of the Rest client.</typeparam>
        /// <param name = "hostUrl">The host url.</param>
        /// <param name = "settings">The optional settings.</param>
        /// <returns></returns>
        public static T For<T>(string hostUrl, RestSettings settings = null)where T : class
        {
            return RestClientFactory.Create<T>(hostUrl, settings);
        }

        static RestClient()
        {
            RestClientFactory = new RestClientFactory();
            InitializeRestClient();
        }

        private static void InitializeRestClient()
        {
            RestClientFactory.SetRestClient<IApi01, RestClientForIApi01>();
            RestClientFactory.SetRestClient<IApi02WithoutBaseUrl, RestClientForIApi02WithoutBaseUrl>();
            RestClientFactory.SetRestClient<IApi02WithBaseUrl01, RestClientForIApi02WithBaseUrl01>();
            RestClientFactory.SetRestClient<IApi02WithBaseUrl02, RestClientForIApi02WithBaseUrl02>();
            RestClientFactory.SetRestClient<IApi02WithBaseUrl03, RestClientForIApi02WithBaseUrl03>();
            RestClientFactory.SetRestClient<IApi03, RestClientForIApi03>();
            RestClientFactory.SetRestClient<IApi05, RestClientForIApi05>();
            RestClientFactory.SetRestClient<IApi06, RestClientForIApi06>();
        }
    }
}