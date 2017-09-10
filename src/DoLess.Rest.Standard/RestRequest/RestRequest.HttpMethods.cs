using System.Net.Http;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private static readonly HttpMethod HttpMethodPatch = new HttpMethod("PATCH");

        public static RestRequest Delete(RestClient client)
        {
            return new RestRequest(HttpMethod.Delete, client);
        }

        public static RestRequest Get(RestClient client)
        {
            return new RestRequest(HttpMethod.Get, client);
        }

        public static RestRequest Head(RestClient client)
        {
            return new RestRequest(HttpMethod.Head, client);
        }

        public static RestRequest Options(RestClient client)
        {
            return new RestRequest(HttpMethod.Options, client);
        }

        public static RestRequest Patch(RestClient client)
        {
            return new RestRequest(HttpMethodPatch, client);
        }

        public static RestRequest Post(RestClient client)
        {
            return new RestRequest(HttpMethod.Post, client);
        }

        public static RestRequest Put(RestClient client)
        {
            return new RestRequest(HttpMethod.Put, client);
        }

        public static RestRequest Trace(RestClient client)
        {
            return new RestRequest(HttpMethod.Trace, client);
        }
    }
}
