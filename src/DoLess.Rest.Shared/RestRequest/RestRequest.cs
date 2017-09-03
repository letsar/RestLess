using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public const string SegmentStart = "/";

        private readonly HttpRequestMessage httpRequestMessage;
        private readonly StringBuilder uriStringBuilder;
        private readonly NameValueCollection queries;

        private RestRequest(HttpMethod httpMethod, RestSettings settings)
        {
            this.httpRequestMessage = new HttpRequestMessage();
            this.httpRequestMessage.Method = httpMethod;
            this.uriStringBuilder = new StringBuilder(SegmentStart);
            this.queries = new NameValueCollection();
            
        }
    }
}
