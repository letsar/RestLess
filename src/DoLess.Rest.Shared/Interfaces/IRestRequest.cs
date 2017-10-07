using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest
{
    public interface IRestRequest
    {
        /// <summary>
        /// Reads the content as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the content as <see cref="string"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the content as <see cref="byte[]"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the content as <see cref="Stream"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a request and indicates whether the response is successful or not.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> SendAndGetSuccessAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the content as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object in the content.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<T> ReadAsObject<T>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends the request without getting the content.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SendAsync(CancellationToken cancellationToken = default);

        IRestRequest WithContent(HttpContent body, string name = null, string fileName = null, string contentType = null);

        IRestRequest WithContent(Stream body, string name = null, string fileName = null, string contentType = null);

        IRestRequest WithContent(string body, string name = null, string fileName = null, string contentType = null);

        IRestRequest WithContent(byte[] body, string name = null, string fileName = null, string contentType = null);

        IRestRequest WithContent<T>(T body, string name = null, string fileName = null, string contentType = null);

        IRestRequest WithFormUrlEncodedContent<T>(T body);

        IRestRequest WithHeader(string name, string value);

        IRestRequest WithHeader(string name, IEnumerable<string> values);

        IRestRequest WithUriTemplate(string uriTemplate);

        IRestRequest WithUriVariable(string name, object variable);

        IRestRequest WithBaseUrl(string baseUrl);

        IRestRequest WithMediaTypeFormatter(string mediaTypeFormatterName);

        IRestRequest WithUrlParameterFormatter(string urlParameterFormatterName);

        IRestRequest WithFormFormatter(string formFormatterName);
    }
}
