using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{
    internal class RequestUriBuilder
    {
        private const string PathFragmentSeparator = "/";
        private const string QueryStringStart = "?";
        private const string QuerySeparator = "&";
        private readonly StringBuilder path;
        private readonly StringBuilder query;

        public RequestUriBuilder()
        {
            this.path = new StringBuilder();
            this.query = new StringBuilder();
        }

        public RequestUriBuilder AppendPathFragment(string pathFragment)
        {
            if (!string.IsNullOrWhiteSpace(pathFragment))
            {
                this.path.Append(PathFragmentSeparator);
                this.path.Append(Uri.EscapeUriString(pathFragment));
            }
            return this;
        }

        public RequestUriBuilder AppendQueryParameter(string name, string value)
        {

            return this;
        }

        public RequestUriBuilder AppendQueryParameter(string name, IReadOnlyList<string> values)
        {

            return this;
        }

        public override string ToString()
        {
            return this.path.ToString();
        }
    }
}
