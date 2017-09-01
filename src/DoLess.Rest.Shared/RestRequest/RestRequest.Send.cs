using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        public Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpRequestMessage httpRequestMessage = null;
            return null;
        }
    }
}
