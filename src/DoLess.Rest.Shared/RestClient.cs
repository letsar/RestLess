using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public abstract class RestClient
    {
        protected readonly HttpClient httpClient;
        protected readonly RestSettings settings;

        public RestClient(HttpClient httpClient, RestSettings settings)
        {
            this.httpClient = httpClient;
            this.settings = settings ?? new RestSettings();
        }
    }
}
