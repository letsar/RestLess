using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RestLess.Generated
{
    /// <summary>
    /// Represents a REST client factory.
    /// </summary>
    public class RestClientFactory
    {
        private readonly Dictionary<Type, Func<IRestClient>> initializers;

        /// <summary>
        /// Creates a new <see cref="RestClientFactory"/>.
        /// </summary>
        public RestClientFactory()
        {
            this.initializers = new Dictionary<Type, Func<IRestClient>>();
        }

        /// <summary>
        /// Sets the REST client for the specified interface.
        /// </summary>
        /// <typeparam name="TInterface">The REST client interface.</typeparam>
        /// <typeparam name="TImplementation">The REST client implementation.</typeparam>
        public void SetRestClient<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : TInterface, IRestClient, new()
        {
            this.initializers[typeof(TInterface)] = () => new TImplementation();
        }


        /// <summary>
        /// Creates a REST client from a httpClientFactory.
        /// </summary>
        /// <typeparam name="T">The type of the REST client.</typeparam>
        /// <param name="httpClient">The httpClientFactory.</param>
        /// <param name="settings">The REST settings.</param>
        /// <returns></returns>
        public T Create<T>(Func<HttpClient> httpClientFactory, RestSettings settings)
            where T : class
        {
            settings = settings ?? new RestSettings();
            if (!this.initializers.TryGetValue(typeof(T), out Func<IRestClient> initializer))
            {
                throw new ArgumentException($"The type '{typeof(T).FullName}' is not a Rest interface.");
            }

            IRestClient restClient = initializer();
            restClient.HttpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            restClient.Settings = settings;
            return (T)restClient;
        }


        /// <summary>
        /// Creates a REST client from a <see cref="HttpClient"/>.
        /// </summary>
        /// <typeparam name="T">The type of the REST client.</typeparam>
        /// <param name="httpClient">The <see cref="HttpClient"/>.</param>
        /// <param name="settings">The REST settings.</param>
        /// <returns></returns>
        public T Create<T>(HttpClient httpClient, RestSettings settings)
            where T : class
        {
            return this.Create<T>(() => httpClient, settings);
        }

        /// <summary>
        /// Creates a REST client from an <see cref="Uri"/>.
        /// </summary>
        /// <typeparam name="T">The type of the REST client.</typeparam>
        /// <param name="hostUri">The <see cref="Uri"/>.</param>
        /// <param name="settings">The REST settings.</param>
        /// <returns></returns>
        public T Create<T>(Uri hostUri, RestSettings settings)
            where T : class
        {
            var handler = settings?.HttpMessageHandlerFactory?.Invoke() ?? new HttpClientHandler();
            return this.Create<T>(new HttpClient(handler) { BaseAddress = hostUri }, settings);
        }

        /// <summary>
        /// Creates a REST client from a <see cref="string"/>.
        /// </summary>
        /// <typeparam name="T">The type of the REST client.</typeparam>
        /// <param name="hostUrl">The <see cref="string"/> url.</param>
        /// <param name="settings">The REST settings.</param>
        /// <returns></returns>
        public T Create<T>(string hostUrl, RestSettings settings)
            where T : class
        {
            return this.Create<T>(new Uri(hostUrl), settings);
        }
    }
}
