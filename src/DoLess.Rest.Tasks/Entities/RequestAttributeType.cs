using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest.Tasks
{
    internal enum RequestAttributeType
    {
        HttpMethod,
        UrlParameter,
        Body,
        Header,
        BaseUrl
    }
}
