using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            return this.client.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
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
        /// Reads the content as <see cref="bool"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task<bool> ReadAsBoolAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this.ReadAsHttpResponseMessageWithoutContent(cancellationToken)
                       .ContinueWith(x =>
                       {
                           // TODO: Create an attribute to manage the Success status codes.
                           return x.IsCompleted && x.Result.IsSuccessStatusCode;
                       });
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
            return this.client.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
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

        private void EnsureAllIsSetBeforeSendingTheRequest()
        {
            this.EnsureRequestUriIsSet();
        }
    }
}
