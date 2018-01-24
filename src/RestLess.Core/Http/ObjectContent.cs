using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using RestLess.Helpers;

namespace RestLess.Http
{
    internal class ObjectContent<T> : HttpContent
    {
        private readonly T content;
        private readonly IMediaTypeFormatter mediaTypeFormatter;

        public ObjectContent(T content, IMediaTypeFormatter mediaTypeFormatter)
        {
            Check.NotNull(mediaTypeFormatter, nameof(mediaTypeFormatter));

            this.content = content;
            this.mediaTypeFormatter = mediaTypeFormatter;

            this.SetMediaTypeFormatterHeaders();
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var serializeToStreamTask = new TaskCompletionSource<bool>();

            Stream wrappedStream = new CompleteTaskOnCloseStream(stream, serializeToStreamTask);

            // We had to create this wrapped stream due to the way the http client is implemented on the .net framework.
            using (TextWriter writer = new StreamWriter(wrappedStream))
            {
                await this.mediaTypeFormatter.WriteAsync(content, writer);
            }

            // Wait for wrappedStream.Close/Dispose to get called.
            await serializeToStreamTask.Task;            
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1L;
            return false;
        }

        private void SetMediaTypeFormatterHeaders()
        {
            var mediaType = this.mediaTypeFormatter.MediaType;
            if (mediaType.HasContent())
            {
                this.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            }
        }

        internal class CompleteTaskOnCloseStream : DelegatingStream
        {
            private readonly TaskCompletionSource<bool> serializeToStreamTask;

            public CompleteTaskOnCloseStream(Stream innerStream, TaskCompletionSource<bool> serializeToStreamTask)
                : base(innerStream)
            {
                this.serializeToStreamTask = serializeToStreamTask;
            }

            [SuppressMessage(
                "Microsoft.Usage",
                "CA2215:Dispose methods should call base class dispose",
                Justification = "See comments, this is intentional.")]
            protected override void Dispose(bool disposing)
            {
                // We don't dispose the underlying stream because we don't own it. Dispose in this case just signifies
                // that the user's action is finished.
                this.serializeToStreamTask.TrySetResult(true);
            }
        }

        /// <summary>
        /// Stream that delegates to inner stream. 
        /// This is taken from System.Net.Http
        /// </summary>
        // https://github.com/ASP-NET-MVC/aspnetwebstack/blob/d5188c8a75b5b26b09ab89bedfd7ee635ae2ff17/src/System.Net.Http.Formatting/Internal/DelegatingStream.cs
        internal abstract class DelegatingStream : Stream
        {
            private readonly Stream innerStream;

            protected DelegatingStream(Stream innerStream)
            {
                this.innerStream = innerStream ?? throw new ArgumentNullException(nameof(innerStream));
            }

            protected Stream InnerStream
            {
                get { return innerStream; }
            }

            public override bool CanRead
            {
                get { return innerStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return innerStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return innerStream.CanWrite; }
            }

            public override long Length
            {
                get { return innerStream.Length; }
            }

            public override long Position
            {
                get { return innerStream.Position; }
                set { innerStream.Position = value; }
            }

            public override int ReadTimeout
            {
                get { return innerStream.ReadTimeout; }
                set { innerStream.ReadTimeout = value; }
            }

            public override bool CanTimeout
            {
                get { return innerStream.CanTimeout; }
            }

            public override int WriteTimeout
            {
                get { return innerStream.WriteTimeout; }
                set { innerStream.WriteTimeout = value; }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    innerStream.Dispose();
                }
                base.Dispose(disposing);
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return innerStream.Seek(offset, origin);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return innerStream.Read(buffer, offset, count);
            }

            public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return innerStream.ReadAsync(buffer, offset, count, cancellationToken);
            }

            public override int ReadByte()
            {
                return innerStream.ReadByte();
            }

            public override void Flush()
            {
                innerStream.Flush();
            }

            public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            {
                return innerStream.CopyToAsync(destination, bufferSize, cancellationToken);
            }

            public override Task FlushAsync(CancellationToken cancellationToken)
            {
                return innerStream.FlushAsync(cancellationToken);
            }

            public override void SetLength(long value)
            {
                innerStream.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                innerStream.Write(buffer, offset, count);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                return innerStream.WriteAsync(buffer, offset, count, cancellationToken);
            }

            public override void WriteByte(byte value)
            {
                innerStream.WriteByte(value);
            }
        }
    }
}
