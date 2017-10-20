using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RestLess
{
    /// <summary>
    /// Represents a REST request.
    /// </summary>
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

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent(HttpContent content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent(Stream content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent(string content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent(byte[] content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content).</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent<T>(T content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name of the content (for multipart content).</param>
        /// <param name="fileName">The filename of the content (for multipart content). If not defined the filename of the content will be used.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns></returns>
        IRestRequest WithContent(FileInfo content, string name = null, string fileName = null, string contentType = null);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/> as a form url encoded content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        IRestRequest WithFormUrlEncodedContent<T>(T content);

        /// <summary>
        /// Adds the specified content to the <see cref="HttpRequestMessage"/> as a form url encoded content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        IRestRequest WithFormUrlEncodedContent(IEnumerable<KeyValuePair<string, string>> content);

        /// <summary>
        /// Adds the specified header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        /// <param name="isCustomParameter">Indicates that the value is the key of a custom parameter defined in the <see cref="RestSettings"/>.</param>
        /// <returns></returns>
        IRestRequest WithHeader(string name, string value, bool isCustomParameter = false);

        /// <summary>
        /// Adds the specified header.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="values">The header values.</param>
        /// <returns></returns>
        IRestRequest WithHeader(string name, IEnumerable<string> values);

        /// <summary>
        /// Sets the specified relative uri template.
        /// </summary>
        /// <param name="uriTemplate">The uri template.</param>
        /// <remarks>The uri template must follow the RFC6570.</remarks>
        /// <returns></returns>
        IRestRequest WithUriTemplate(string uriTemplate);

        /// <summary>
        /// Sets the UriTemplate prefix of this request.
        /// </summary>
        /// <param name="uriTemplate">The uri template prefix.</param>
        /// <remarks>The uri template must follow the RFC6570.</remarks>
        /// <returns></returns>
        IRestRequest WithUriTemplatePrefix(string uriTemplate);

        /// <summary>
        /// Sets the UriTemplate suffix of this request.
        /// </summary>
        /// <param name="uriTemplate">The uri template suffix.</param>
        /// <remarks>The uri template must follow the RFC6570.</remarks>
        /// <returns></returns>
        IRestRequest WithUriTemplateSuffix(string uriTemplate);

        /// <summary>
        /// Adds a uri variable that will be used to resolve the uri template.
        /// </summary>
        /// <param name="name">The name of the variable specified in the uri template.</param>
        /// <param name="variable">The value of the variable.</param>
        /// <returns></returns>
        IRestRequest WithUriVariable(string name, object variable);

        /// <summary>
        /// Sets the <see cref="IMediaTypeFormatter"/> to use.
        /// </summary>
        /// <param name="mediaTypeFormatterName">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        /// <returns></returns>
        IRestRequest WithMediaTypeFormatter(string mediaTypeFormatterName);

        /// <summary>
        /// Sets the url parameter formatter to use.
        /// </summary>
        /// <param name="urlParameterFormatterName">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        /// <returns></returns>
        IRestRequest WithUrlParameterFormatter(string urlParameterFormatterName);

        /// <summary>
        /// Sets the <see cref="IFormFormatter"/> to use.
        /// </summary>
        /// <param name="formFormatterName">The name of the formatter specified in the <see cref="IRestClient.Settings"/></param>
        /// <returns></returns>
        IRestRequest WithFormFormatter(string formFormatterName);
    }
}
