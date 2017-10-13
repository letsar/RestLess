using System.IO;
using System.Net.Http;
using DoLess.Rest.Http;

namespace DoLess.Rest.Generated
{    
    public sealed partial class RestRequest
    {
        public IRestRequest WithContent(HttpContent content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(content, name, fileName, contentType));
            }

            return this;
        }

        public IRestRequest WithContent(Stream content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new StreamContent(content), name, fileName, contentType));
            }

            return this;
        }

        public IRestRequest WithContent(string content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new StringContent(content), name, fileName, contentType));
            }

            return this;
        }

        public IRestRequest WithContent(byte[] content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new ByteArrayContent(content), name, fileName, contentType));
            }

            return this;
        }

        public IRestRequest WithContent<T>(T content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new ObjectContent<T>(content, this.mediaTypeFormatter), name, fileName, contentType));
            }

            return this;
        }

        public IRestRequest WithContent(FileInfo content, string name = null, string fileName = null, string contentType = null)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new StreamContent(content.OpenRead()), name, fileName ?? content.Name, contentType, true));
            }

            return this;
        }

        public IRestRequest WithFormUrlEncodedContent<T>(T content)
        {
            if (content != null)
            {
                this.contentParts.Add(new ContentPart(new FormUrlEncodedContent(this.formFormatter.Format(content)), null, null, null));
            }

            return this;
        }
    }
}
