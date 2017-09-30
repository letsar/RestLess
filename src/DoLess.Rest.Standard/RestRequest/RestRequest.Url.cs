using System;
using DoLess.UriTemplates;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        public IRestRequest WithUriTemplate(string uriTemplate)
        {
            this.uriTemplate = UriTemplate.For(uriTemplate, true);
            return this;
        }

        public IRestRequest WithParameter(string name, object parameter)
        {
            this.uriTemplate.WithParameter(name, parameter);
            return this;
        }

        public IRestRequest WithBaseUrl(string baseUrl)
        {
            if (baseUrl.IsNullOrWhiteSpace())
            {
                baseUrl = "/";
            }
            this.baseUri = new Uri(baseUrl, UriKind.Relative);
            return this;
        }

        private Uri BuildUri()
        {
            var uriString = this.uriTemplate.ExpandToString();
            var relativeUri = $"{this.baseUri.OriginalString.TrimEnd('/')}/{uriString.TrimStart('/')}";

            // The UriBuilder needs to be initialized with an absolute uri, so we
            // give him a dumb one.
            var uriBuilder = new UriBuilder(new Uri(new Uri("http://api"), relativeUri));

            return new Uri(uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped), UriKind.Relative);
        }
    }
}
