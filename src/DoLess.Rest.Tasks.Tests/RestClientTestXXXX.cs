using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoLess.Rest.Tasks.CodeParsers
{
    class RestClientTestXXXX : RestClient
    {
        public RestClientTestXXXX(HttpClient httpClient, RestSettings settings) :
            base(httpClient, settings)
        { }

        public Task<string> Test()
        {
            RestRequest.Get(this.settings)
                       .AddUrlPathSegment("",false);
            return null;
        }
    }
}
