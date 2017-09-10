using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public RestRequest WithHeader(string name, string value)
        {
            this.httpRequestMessage.Headers.Add(name, value);
            return this;
        }

        public RestRequest WithHeader(string name, IEnumerable<string> values)
        {
            this.httpRequestMessage.Headers.Add(name, values);
            return this;
        }        
    }
}
