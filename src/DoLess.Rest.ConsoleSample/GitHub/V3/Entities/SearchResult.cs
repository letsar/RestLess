using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DoLess.Rest.ConsoleSample.GitHub.V3.Entities
{
    public class SearchResult<T>
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("incomplete_results")]
        public bool HasIncompleteResults { get; set; }

        [JsonProperty("items")]
        public List<T> Items { get; set; }
    }
}
