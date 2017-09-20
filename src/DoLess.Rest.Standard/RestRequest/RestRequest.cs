using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using DoLess.UriTemplates;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private readonly HttpRequestMessage httpRequestMessage;
        private readonly IRestClient restClient;

        private Uri baseUri;
        private UriTemplate uriTemplate;

        private RestRequest(HttpMethod httpMethod, IRestClient restClient)
        {
            this.httpRequestMessage = new HttpRequestMessage();
            this.httpRequestMessage.Method = httpMethod;
            this.restClient = restClient;
            this.baseUri = new Uri("/", UriKind.Relative);
        }

    }
}
