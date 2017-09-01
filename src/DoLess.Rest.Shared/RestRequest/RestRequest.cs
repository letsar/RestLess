using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using DoLess.Rest.Helpers;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private readonly Url url;
        private readonly HttpMethod httpMethod;

        private RestRequest(HttpMethod httpMethod, RestSettings settings)
        {
            this.httpMethod = httpMethod;
            this.url = new Url(settings);
        }
    }
}
