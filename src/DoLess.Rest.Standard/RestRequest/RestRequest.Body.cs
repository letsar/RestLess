using System.IO;
using System.Net.Http;
using DoLess.Rest.Http;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        public IRestRequest WithBody(HttpContent body)
        {
            this.httpRequestMessage.Content = body;
            return this;
        }

        public IRestRequest WithBody(Stream body)
        {
            this.httpRequestMessage.Content = new StreamContent(body);
            return this;
        }

        public IRestRequest WithBody(string body)
        {
            this.httpRequestMessage.Content = new StringContent(body);
            return this;
        }

        public IRestRequest WithBody(byte[] body)
        {
            this.httpRequestMessage.Content = new ByteArrayContent(body);
            return this;
        }

        public IRestRequest WithBody<T>(T body)
        {
            this.EnsureMediaTypeFormatter();
            this.httpRequestMessage.Content = new ObjectContent<T>(body, this.restClient.Settings.MediaTypeFormatter);
            return this;
        }

        public IRestRequest WithFormUrlEncodedBody<T>(T body)
        {
            this.EnsureFormFormatter();
            this.httpRequestMessage.Content = new FormUrlEncodedContent(this.restClient.Settings.FormFormatter.Format(body));
            return this;
        }
    }
}
