using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public abstract class RestClient
    {
        public RestClient(HttpClient httpClient, RestSettings settings)
        {
            this.HttpClient = httpClient;
            this.Settings = settings ?? new RestSettings();
        }

        internal HttpClient HttpClient { get; }

        internal RestSettings Settings { get; }
    }
}
