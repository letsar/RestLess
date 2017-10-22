using System.IO;
using System.Threading.Tasks;

namespace RestLess.Helpers
{
    /// <summary>
    /// Dummy media formatter.
    /// </summary>
    public class DefaultMediaFormatter : IMediaTypeFormatter
    {
        /// <summary>
        /// Gets the media type (also called content type).
        /// </summary>
        public string MediaType => "text/plain";

        /// <summary>
        /// Returns the content inside the <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public Task<T> ReadAsync<T>(TextReader reader)
        {
            return Task.FromResult(default(T));
        }

        /// <summary>
        /// Writes the <paramref name="content"/> using the specified writer.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="content">The content to write.</param>
        /// <param name="writer">The writer.</param>
        /// <returns></returns>
        public Task WriteAsync<T>(T content, TextWriter writer)
        {
            return writer.WriteAsync(content.ToString());
        }
    }
}
