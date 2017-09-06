using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public RestRequest AppendUrl(string urlPart, bool encode = false)
        {
            if (urlPart != null)
            {
                this.uriStringBuilder.Append(UrlEncode(urlPart, encode));
            }
            return this;
        }

        public RestRequest AddQuery(string name, string value)
        {
            this.queries.Add(name, value);
            return this;
        }

        public RestRequest AddQuery(string name, IEnumerable<string> values)
        {
            values.ForEach(x => this.queries.Add(name, x));
            return this;
        }

        public RestRequest AddQuery(string dontCare, IReadOnlyDictionary<string, string> queries)
        {
            queries.ForEach(x => this.queries.Add(x.Key, x.Value));
            return this;
        }

        public RestRequest AddQuery(string dontCare, IReadOnlyDictionary<string, IEnumerable<string>> queries)
        {
            queries.ForEach(x => this.AddQuery(x.Key, x.Value));
            return this;
        }

        public RestRequest AddQuery(string name, object value)
        {
            this.EnsureUrlParameterFormatter();
            return this.AddQuery(name, this.client.Settings.UrlParameterFormatter.Format(value));
        }

        private static string UrlEncode(string value, bool encode = true)
        {
            return encode ? Uri.EscapeDataString(value) : value;
        }

        private static string BuildQueryString(NameValueCollection queries)
        {
            return string.Join("&", queries.AllKeys.Select(x => $"{UrlEncode(x)}={UrlEncode(queries.Get(x))}"));
        }

        private Uri BuildUri()
        {
            var uriString = this.uriStringBuilder.ToString();

            // The UriBuilder needs to be initialized with an absolute uri, so we
            // give him a dumb one.
            var uriBuilder = new UriBuilder(new Uri(new Uri("http://api"), uriString));

            var uriStringQueries = HttpUtility.ParseQueryString(uriBuilder.Query ?? string.Empty);
            this.queries.Add(uriStringQueries);
            if (this.queries.HasKeys())
            {
                uriBuilder.Query = BuildQueryString(this.queries);
            }
            else
            {
                uriBuilder.Query = null;
            }

            return new Uri(uriBuilder.Uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped), UriKind.Relative);
        }        
    }
}
