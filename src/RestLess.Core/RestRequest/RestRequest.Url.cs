using System;
using RestLess.Helpers;
using DoLess.UriTemplates;

namespace RestLess.Generated
{
    public sealed partial class RestRequest
    {
        public IRestRequest WithUriTemplate(string uriTemplate)
        {
            uriTemplate = UriTemplateHelper.AppendUriTemplateSuffix(this.uriTemplatePrefix + uriTemplate, this.uriTemplateSuffix);
            this.uriTemplate = UriTemplate.For(uriTemplate, false);
            return this;
        }

        public IRestRequest WithUriVariable(string name, object variable)
        {
            this.uriTemplate.WithParameter(name, variable);
            return this;
        }

        public IRestRequest WithUriTemplatePrefix(string urlPrefix)
        {
            this.uriTemplatePrefix = urlPrefix;
            return this;
        }

        public IRestRequest WithUriTemplateSuffix(string urlSuffix)
        {
            this.uriTemplateSuffix = urlSuffix;
            return this;
        }

        private Uri BuildUri()
        {
            // Adds the custom parameters if any.
            this.restClient.Settings?.CustomParameters.ForEach(x => this.uriTemplate.WithParameter(x.Key, x.Value));

            var uriString = this.uriTemplate.ExpandToString();

            // The UriBuilder needs to be initialized with an absolute uri, so we
            // give him a dumb one.
            var uriBuilder = new UriBuilder(new Uri(new Uri("http://api"), uriString));

            return new Uri(uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped), UriKind.Relative);
        }
    }
}
