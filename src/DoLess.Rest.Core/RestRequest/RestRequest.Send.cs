using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DoLess.Rest.Generated
{
    public sealed partial class RestRequest
    {
        public Task<HttpResponseMessage> ReadAsHttpResponseMessageAsync(CancellationToken cancellationToken = default)
        {
            this.EnsureAllIsSetBeforeSendingTheRequest();
            return this.restClient.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        public Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default)
        {
            return this.ReadHttpContentAsync(x => x.ReadAsStringAsync(), cancellationToken);
        }

        public Task<byte[]> ReadAsByteArrayAsync(CancellationToken cancellationToken = default)
        {
            return this.ReadHttpContentAsync(x => x.ReadAsByteArrayAsync(), cancellationToken);
        }

        public Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken = default)
        {
            return this.ReadHttpContentAsync(x => x.ReadAsStreamAsync(), cancellationToken);
        }

        public Task<bool> SendAndGetSuccessAsync(CancellationToken cancellationToken = default)
        {
            return this.ReadAsHttpResponseMessageWithoutContent(cancellationToken)
                       .ContinueWith(x =>
                       {
                           // TODO: Create an attribute to manage the Success status codes.
                           return x.IsCompleted && x.Result.IsSuccessStatusCode;
                       });
        }

        public async Task<T> ReadAsObject<T>(CancellationToken cancellationToken = default)
        {
            using (Stream stream = await this.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false))
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return await this.mediaTypeFormatter.ReadAsync<T>(streamReader);
            }
        }
        
        public Task SendAsync(CancellationToken cancellationToken = default)
        {
            return this.ReadAsHttpResponseMessageWithoutContent(cancellationToken);
        }

        private Task<HttpResponseMessage> ReadAsHttpResponseMessageWithoutContent(CancellationToken cancellationToken = default)
        {
            this.EnsureAllIsSetBeforeSendingTheRequest();
            return this.restClient.HttpClient.SendAsync(this.httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        private async Task<T> ReadHttpContentAsync<T>(Func<HttpContent, Task<T>> func, CancellationToken cancellationToken = default)
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
