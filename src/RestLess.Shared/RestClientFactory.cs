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

        public RestClientFactory()
        {
            this.initializers = new Dictionary<Type, Func<IRestClient>>();
        }

        public void SetRestClient<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : TInterface, IRestClient, new()
        {
            this.initializers[typeof(TInterface)] = () => new TImplementation();
        }

        public T Create<T>(HttpClient httpClient, RestSettings settings)
            where T : class
        {
            settings = settings ?? new RestSettings();
            if (!this.initializers.TryGetValue(typeof(T), out Func<IRestClient> initializer))
            {
                throw new ArgumentException($"The type '{typeof(T).FullName}' is not a Rest interface.");
            }

            IRestClient restClient = initializer();
            restClient.HttpClient = httpClient;
            restClient.Settings = settings;
            return (T)restClient;
        }

        public T Create<T>(Uri hostUri, RestSettings settings)
            where T : class
        {
            var handler = settings?.HttpMessageHandlerFactory?.Invoke() ?? new HttpClientHandler();
            return this.Create<T>(new HttpClient(handler) { BaseAddress = hostUri }, settings);
        }

        public T Create<T>(string hostUrl, RestSettings settings)
            where T : class
        {
            return this.Create<T>(new Uri(hostUrl), settings);
        }
    }
}
