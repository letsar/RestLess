using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DoLess.Rest
{
    public class RestClientFactory
    {
        private readonly Dictionary<Type, Func<HttpClient, RestSettings, object>> factories;

        public RestClientFactory()
        {
            this.factories = new Dictionary<Type, Func<HttpClient, RestSettings, object>>();
        }

        public void AddClient(Type clientType, Func<HttpClient, RestSettings, object> clientFactory)
        {
            this.factories[clientType] = clientFactory;
        }

        /// <summary>
        /// Creates a Rest Client from the specified interface.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="settings">The optional settings.</param>
        /// <returns></returns>
        public object Create(Type clientType, HttpClient client, RestSettings settings = null)
        {
            settings = settings ?? new RestSettings();
            if (!this.factories.TryGetValue(clientType, out Func<HttpClient, RestSettings, object> factory))
            {
                throw new ArgumentException($"The type '{clientType.FullName}' is not a Rest interface.");
            }
            return factory(client, settings);
        }
    }
}
