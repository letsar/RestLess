using System;
using DoLess.UriTemplates;

namespace DoLess.Rest
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
            this.baseUri = new Uri(baseUrl, UriKind.Relative);
            return this;
        }

        private Uri BuildUri()
        {
            var uriString = this.uriTemplate.ExpandToString();

            // The UriBuilder needs to be initialized with an absolute uri, so we
            // give him a dumb one.
            var uriBuilder = new UriBuilder(new Uri(new Uri("http://api"), new Uri(this.baseUri, new Uri(uriString, UriKind.Relative))));

            return new Uri(uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped), UriKind.Relative);
        }
    }
}
