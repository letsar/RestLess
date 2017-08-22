using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public class RestClient
    {
        private readonly HttpClient httpClient;
        private readonly RestSettings settings;

        public RestClient(HttpClient httpClient, RestSettings settings = null)
        {
            this.httpClient = httpClient;
            this.settings = settings ?? new RestSettings();
        }

        public RestClient(string uriString, RestSettings settings = null) : 
            this(CreateHttpClientFromUri(uriString), settings)
        {
        }

        public RestClient(Uri uri, RestSettings settings = null) : 
            this(CreateHttpClientFromUri(uri), settings)
        {
        }



        private static HttpClient CreateHttpClientFromUri(string uriString)
        {
            return CreateHttpClientFromUri(new Uri(uriString));
        }

        private static HttpClient CreateHttpClientFromUri(Uri uri)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = uri;
            return httpClient;
        }
    }
}
