using System.IO;
using System.Net.Http;
using DoLess.Rest.Http;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        private const string DoLessRestBoundary = "DoLessRestBoundary";

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
            this.httpRequestMessage.Content = new ObjectContent<T>(body, this.mediaTypeFormatter);
            return this;
        }

        public IRestRequest WithFormUrlEncodedBody<T>(T body)
        {            
            this.httpRequestMessage.Content = new FormUrlEncodedContent(this.formFormatter.Format(body));
            return this;
        }

        public IRestRequest WithBodyPart(string name, string value)
        {
            this.EnsureIsMultipartContent();
            // TODO.
            return this;
        }

        private void EnsureIsMultipartContent()
        {
            if (!(this.httpRequestMessage.Content is MultipartContent))
            {
                this.httpRequestMessage.Content = new MultipartFormDataContent(DoLessRestBoundary);
            }
        }
    }
}
