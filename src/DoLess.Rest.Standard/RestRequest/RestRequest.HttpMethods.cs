using System.Net.Http;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        private static readonly HttpMethod HttpMethodPatch = new HttpMethod("PATCH");

        public static RestRequest Delete(IRestClient client)
        {
            return new RestRequest(HttpMethod.Delete, client);
        }

        public static RestRequest Get(IRestClient client)
        {
            return new RestRequest(HttpMethod.Get, client);
        }

        public static RestRequest Head(IRestClient client)
        {
            return new RestRequest(HttpMethod.Head, client);
        }

        public static RestRequest Options(IRestClient client)
        {
            return new RestRequest(HttpMethod.Options, client);
        }

        public static RestRequest Patch(IRestClient client)
        {
            return new RestRequest(HttpMethodPatch, client);
        }

        public static RestRequest Post(IRestClient client)
        {
            return new RestRequest(HttpMethod.Post, client);
        }

        public static RestRequest Put(IRestClient client)
        {
            return new RestRequest(HttpMethod.Put, client);
        }

        public static RestRequest Trace(IRestClient client)
        {
            return new RestRequest(HttpMethod.Trace, client);
        }
    }
}
