using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoLess.Rest.Helpers
{
    internal class Url
    {
        public const string SegmentStart = "/";
        public const string QueryStart = "?";
        public const string FragmentStart = "#";
        public const string QuerySeparator = "&";

        private readonly RestSettings settings;
        private List<string> segments;
        private Dictionary<string, HashSet<string>> queries;
        private string fragment;

        public Url(RestSettings settings)
        {
            this.segments = new List<string>();
            this.queries = new Dictionary<string, HashSet<string>>();
            this.fragment = null;
            this.settings = settings;
        }

        public static Url Create(RestSettings settings)
        {
            return new Url(settings);
        }

        public Url AddSegment(string segment, bool encode = true)
        {
            this.segments.Add(encode ? UrlEncode(segment) : segment);
            return this;
        }

        public Url AddQuery(string name, string value, bool encode = true)
        {
            return this.AddQuery(name, new string[] { value }, encode);
        }

        public Url AddQuery(string name, IReadOnlyList<string> values, bool encode = true)
        {
            if (values != null)
            {
                if (!this.queries.TryGetValue(name, out HashSet<string> hashSet))
                {
                    hashSet = new HashSet<string>();
                    this.queries[name] = hashSet;
                }

                for (int i = 0; i < values.Count; i++)
                {
                    hashSet.Add(encode ? UrlEncode(values[i]) : values[i]);
                }
            }

            return this;
        }

        public Url SetFragment(string fragment, bool encode = true)
        {
            this.fragment = encode ? UrlEncode(fragment) : fragment;
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendUrlPart(SegmentStart, this.BuildPath());
            sb.AppendUrlPart(QueryStart, this.BuildQuery());
            sb.AppendUrlPart(FragmentStart, this.fragment);

            return sb.ToString();
        }

        private static string UrlEncode(string value)
        {
            return Uri.EscapeDataString(value);
        }

        private string BuildPath()
        {
            return string.Join(SegmentStart, segments);
        }

        private string BuildQuery()
        {
            if (this.queries.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var query in this.queries)
                {
                    var name = query.Key;
                    var values = this.settings.QueryWithMultipleValuesTransformer(query.Value.ToList());
                    for (int i = 0; i < values.Count; i++)
                    {
                        sb.Append($"{name}={values[i]}");
                        sb.Append(QuerySeparator);
                    }
                }

                // Remove the last '&';
                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            return null;
        }
    }
}
