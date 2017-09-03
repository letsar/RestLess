using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public RestRequest WithBody(HttpContent body)
        {
            this.httpRequestMessage.Content = body;
            return this;
        }

        public RestRequest WithBody(Stream body)
        {
            this.httpRequestMessage.Content = new StreamContent(body);
            return this;
        }

        public RestRequest WithBody(string body)
        {
            this.httpRequestMessage.Content = new StringContent(body);
            return this;
        }

        public RestRequest WithBody(object body)
        {
            // TODO.
            this.httpRequestMessage.Content = null;
            return this;
        }
    }
}
