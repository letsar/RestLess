using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest.Tasks.CodeAnalysis
{
    public class RequestAnalyzer
    {
        public RequestAnalyzer()
        {
            this.Segments = new List<string>();
            this.Queries = new Dictionary<string, List<string>>();
        }

        public HttpMethod HttpMethod { get; set; }

        public List<string> Segments { get; }

        public Dictionary<string, List<string>> Queries { get; }

        public string Fragment { get; set; }
    }
}
