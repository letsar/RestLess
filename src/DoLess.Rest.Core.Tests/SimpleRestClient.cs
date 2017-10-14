using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Generated;

namespace DoLess.Rest.Tests
{
    internal class SimpleRestClient : RestClientBase
    {
        public SimpleRestClient(RestSettings settings = null)
        {
            ((IRestClient)this).Settings = settings ?? new RestSettings();
        }
    }
}
