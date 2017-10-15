using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest.ConsoleSample
{
    public class DelegatingLoggingHandler : DelegatingHandler
    {
        public DelegatingLoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{request.Method} {request.RequestUri}");
            return base.SendAsync(request, cancellationToken);
        }
    }
}
