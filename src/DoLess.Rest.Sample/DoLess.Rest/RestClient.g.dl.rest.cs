using DoLess.Rest.Generated;
using DoLess.Rest.Sample;

namespace DoLess.Rest
{
    public static partial class RestClient
    {
        static partial void InitializeRestClientFactory()
        {
            RestClientFactory.SetRestClient<IRestApi01, RestClientForIRestApi01>();
        }
    }
}