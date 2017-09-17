using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using DoLess.Rest.Http;

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

        public RestRequest WithBody(byte[] body)
        {
            this.httpRequestMessage.Content = new ByteArrayContent(body);
            return this;
        }

        public RestRequest WithBody<T>(T body)
        {
            this.EnsureMediaTypeFormatter();
            this.httpRequestMessage.Content = new ObjectContent<T>(body, this.client.Settings.MediaTypeFormatter);
            return this;
        }

        public RestRequest WithFormUrlEncodedBody<T>(T body)
        {
            this.EnsureFormFormatter();
            this.httpRequestMessage.Content = new FormUrlEncodedContent(this.client.Settings.FormFormatter.Format(body));
            return this;
        }
    }
}
