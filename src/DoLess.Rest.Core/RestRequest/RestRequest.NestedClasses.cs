using System.Net.Http;
using System.Net.Http.Headers;
using DoLess.Rest.Helpers;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest : IRestRequest
    {
        private class ContentPart
        {
            public ContentPart(HttpContent content, string name, string fileName, string contentType, bool isMultipartRequired = false)
            {
                Check.NotNull(content, nameof(content));

                if (!contentType.IsNullOrWhiteSpace())
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                this.Content = content;
                this.Name = name;
                this.FileName = fileName;
                this.IsMultipartRequired = isMultipartRequired;
            }

            public HttpContent Content { get; }

            public string Name { get; }

            public string FileName { get; }

            public bool IsMultipartRequired { get; }
        }
    }
}
