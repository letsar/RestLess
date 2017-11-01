using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestLess.Tests.Interfaces
{
    public interface IApi03
    {
        [Get("api/posts")]
        Task<HttpResponseMessage> GetCancellableAsync(CancellationToken cancellationToken = default);

        [Get("api/posts")]
        [MediaTypeFormatter("MediaTypeJsonFormatter")]
        [UrlParameterFormatter("UrlParameterFormatter")]
        [FormFormatter("FormFormatter")]
        Task<HttpResponseMessage> GetWithFormatters();
    }
}
