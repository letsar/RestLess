using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks
{
    public class BenchmarkHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                Content = new StringContent("hello benchmark")
            };

            return Task.FromResult(responseMessage);
        }
    }
}
