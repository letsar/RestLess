using System;
using System.Net.Http;
using DoLess.UriTemplates;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest : IRestRequest
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
