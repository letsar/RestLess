using System;
using System.Collections.Generic;
using System.Net.Http;
using DoLess.UriTemplates;

namespace RestLess.Generated
{
    /// <summary>
    /// Contains methods to create a <see cref="IRestRequest"/>.
    /// </summary>
    public sealed partial class RestRequest : IRestRequest
    {
        private readonly HttpRequestMessage httpRequestMessage;
        private readonly IRestClient restClient;
        private readonly List<ContentPart> contentParts;
                
        private string uriTemplatePrefix;
        private string uriTemplateSuffix;
        private UriTemplate uriTemplate;
        private IMediaTypeFormatter mediaTypeFormatter;
        private IFormFormatter formFormatter;
        private IValueFormatter valueFormatter;

        private RestRequest(HttpMethod httpMethod, IRestClient restClient)
        {
            this.httpRequestMessage = new HttpRequestMessage();
            this.httpRequestMessage.Method = httpMethod;
            this.restClient = restClient;
            this.contentParts = new List<ContentPart>();

            this.formFormatter = this.EnsureDefaultValueSet(this.restClient.Settings.FormFormatters);
            this.mediaTypeFormatter = this.EnsureDefaultValueSet(this.restClient.Settings.MediaTypeFormatters);
            this.valueFormatter = this.EnsureDefaultValueSet(this.restClient.Settings.UrlParameterFormatters);
        }
    }
}
