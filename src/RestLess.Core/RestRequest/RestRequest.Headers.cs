using System.Collections.Generic;

namespace RestLess.Internal
{
    internal sealed partial class RestRequest
    {
        public IRestRequest WithHeader(string name, string value, bool isCustomParameter = false)
        {
            string headerValue = value;
            if (isCustomParameter)
            {
                this.restClient.Settings.CustomParameters.TryGetValue(value, out headerValue);
            }

            this.httpRequestMessage.Headers.Add(name, headerValue);
            return this;
        }

        public IRestRequest WithHeader(string name, IEnumerable<string> values)
        {
            this.httpRequestMessage.Headers.Add(name, values);
            return this;
        }
    }
}
