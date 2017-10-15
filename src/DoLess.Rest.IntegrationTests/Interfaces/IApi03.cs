using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest.IntegrationTests.Interfaces
{
    public interface IApi03
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetCancellableAsync(CancellationToken cancellationToken = default);

        [Get("api/posts")]
        [MediaTypeFormatter("MediaTypeJsonFormatter")]
        [UrlParameterFormatter("UrlParameterJsonFormatter")]
        [FormFormatter("FormFormatterJsonFormatter")]
        Task<HttpResponseMessage> GetWithFormatters();
    }
}
