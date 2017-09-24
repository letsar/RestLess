using System.Collections.Generic;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public IRestRequest WithHeader(string name, string value)
        {
            this.httpRequestMessage.Headers.Add(name, value);
            return this;
        }

        public IRestRequest WithHeader(string name, IEnumerable<string> values)
        {
            this.httpRequestMessage.Headers.Add(name, values);
            return this;
        }        
    }
}
