using System.Net.Http;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        private static readonly HttpMethod HttpMethodPatch = new HttpMethod("PATCH");

        public static IRestRequest Delete(IRestClient client)
        {
            return new RestRequest(HttpMethod.Delete, client);
        }

        public static IRestRequest Get(IRestClient client)
        {
            return new RestRequest(HttpMethod.Get, client);
        }

        public static IRestRequest Head(IRestClient client)
        {
            return new RestRequest(HttpMethod.Head, client);
        }

        public static IRestRequest Options(IRestClient client)
        {
            return new RestRequest(HttpMethod.Options, client);
        }

        public static IRestRequest Patch(IRestClient client)
        {
            return new RestRequest(HttpMethodPatch, client);
        }

        public static IRestRequest Post(IRestClient client)
        {
            return new RestRequest(HttpMethod.Post, client);
        }

        public static IRestRequest Put(IRestClient client)
        {
            return new RestRequest(HttpMethod.Put, client);
        }

        public static IRestRequest Trace(IRestClient client)
        {
            return new RestRequest(HttpMethod.Trace, client);
        }
    }
}
