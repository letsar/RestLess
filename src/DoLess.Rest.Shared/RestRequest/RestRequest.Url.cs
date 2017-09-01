using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public RestRequest AddUrlPathSegment(string segment, bool encode = true)
        {
            this.url.AddSegment(segment, encode);
            return this;
        }

        public RestRequest AddUrlQuery(string name, string value, bool encode = true)
        {
            return this.AddUrlQuery(name, new string[] { value }, encode);
        }

        public RestRequest AddUrlQuery(string name, IReadOnlyList<string> values, bool encode = true)
        {
            this.url.AddQuery(name, values, encode);
            return this;
        }

        public RestRequest SetUrlFragment(string fragment, bool encode = true)
        {
            this.url.SetFragment(fragment, encode);
            return this;
        }
    }
}
