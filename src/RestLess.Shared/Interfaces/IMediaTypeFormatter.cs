using System.IO;
using System.Threading.Tasks;

namespace RestLess
{
    /// <summary>
    /// Represents a formatter used for writing into a <see cref="System.Net.Http.HttpRequestMessage"/> and reading from a <see cref="System.Net.Http.HttpResponseMessage"/>.
    /// </summary>
    public interface IMediaTypeFormatter
    {
        /// <summary>
        /// Gets the media type (also called content type).
        /// </summary>
        string MediaType { get; }

        /// <summary>
        /// Returns the content inside the <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        Task<T> ReadAsync<T>(TextReader reader);

        /// <summary>
        /// Writes the <paramref name="content"/> using the specified writer.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="content">The content to write.</param>
        /// <param name="writer">The writer.</param>
        /// <returns></returns>
        Task WriteAsync<T>(T content, TextWriter writer);
    }
}
