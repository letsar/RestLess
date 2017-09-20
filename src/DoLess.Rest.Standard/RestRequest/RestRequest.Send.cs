using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DoLess.Rest.Exceptions;

namespace DoLess.Rest
{
    public sealed partial class RestRequest
    {
        /// <summary>
        /// Reads the content as <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.EnsureAllIsSetBeforeSendingTheRequest();
            return this.restClient.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        /// <summary>
        /// Reads the content as <see cref="string"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadHttpContentAsync(x => x.ReadAsStringAsync(), cancellationToken);
        }

        /// <summary>
        /// Reads the content as <see cref="byte[]"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadHttpContentAsync(x => x.ReadAsByteArrayAsync(), cancellationToken);
        }

        /// <summary>
        /// Reads the content as <see cref="Stream"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadHttpContentAsync(x => x.ReadAsStreamAsync(), cancellationToken);
        }

        /// <summary>
        /// Sends a request and indicates whether the response is successful or not.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<bool> SendAndGetSuccessAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadAsHttpResponseMessageWithoutContent(cancellationToken)
                       .ContinueWith(x =>
                       {
                           // TODO: Create an attribute to manage the Success status codes.
                           return x.IsCompleted && x.Result.IsSuccessStatusCode;
                       });
        }

        /// <summary>
        /// Reads the content as <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object in the content.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<T> ReadAsObject<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.EnsureMediaTypeFormatter();

            using (Stream stream = await this.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return await this.restClient.Settings.MediaTypeFormatter.ReadAsync<T>(streamReader);
            }
        }

        /// <summary>
        /// Sends the request without getting the content.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task SendAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadAsHttpResponseMessageWithoutContent(cancellationToken);
        }

        private Task<HttpResponseMessage> ReadAsHttpResponseMessageWithoutContent(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.EnsureAllIsSetBeforeSendingTheRequest();
            return this.restClient.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        private async Task<T> ReadHttpContentAsync<T>(Func<HttpContent, Task<T>> func, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpResponseMessage response = await this.ReadAsHttpResponseMessageAsync(cancellationToken)
                                                     .ConfigureAwait(false);
            var httpContent = response?.Content;
            if (httpContent != null)
            {
                return await func(httpContent).ConfigureAwait(false);
            }
            else
            {
                return default(T);
            }
        }
    }
}
