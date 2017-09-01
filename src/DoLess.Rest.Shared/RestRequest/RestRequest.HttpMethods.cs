using System.Net.Http;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private static readonly HttpMethod HttpMethodPatch = new HttpMethod("PATCH");

        public static RestRequest Delete(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Delete, settings);
        }

        public static RestRequest Get(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Get, settings);
        }

        public static RestRequest Head(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Head, settings);
        }

        public static RestRequest Options(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Options, settings);
        }

        public static RestRequest Patch(RestSettings settings)
        {
            return new RestRequest(HttpMethodPatch, settings);
        }

        public static RestRequest Post(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Post, settings);
        }

        public static RestRequest Put(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Put, settings);
        }

        public static RestRequest Trace(RestSettings settings)
        {
            return new RestRequest(HttpMethod.Trace, settings);
        }
    }
}
