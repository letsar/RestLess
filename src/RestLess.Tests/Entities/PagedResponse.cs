using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestLess.Tests.Entities
{
    public interface IPagedResponse
    {
        int Page { get; set; }

        int PageCount { get; set; }
    }

    public class PagedResponse<T> : List<T>, IPagedResponse
    {
        [JsonIgnore]
        public int Page { get; set; }

        [JsonIgnore]
        public int PageCount { get; set; }
    }
}
