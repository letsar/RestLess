using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using DoLess.Rest.Helpers;
using DoLess.UriTemplates;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest : IRestRequest
    {
        private class ContentPart
        {
            public ContentPart(HttpContent content, string name, string fileName, string contentType)
            {
                Check.NotNull(content, nameof(content));

                if (!contentType.IsNullOrWhiteSpace())
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                this.Content = content;
                this.Name = name;
                this.FileName = fileName;
            }

            public HttpContent Content { get; }

            public string Name { get; }

            public string FileName { get; }
        }
    }
}
