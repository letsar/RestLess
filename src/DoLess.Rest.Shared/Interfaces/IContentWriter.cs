using System.IO;
using System.Threading.Tasks;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents an object that can write the content of a <see cref="System.Net.Http.HttpRequestMessage"/>.
    /// </summary>
    public interface IContentWriter
    {
        /// <summary>
        /// Writes the <paramref name="value"/> to a stream that will be send to the content of the <see cref="System.Net.Http.HttpRequestMessage"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="content">The content to write.</param>
        /// <returns></returns>
        Task WriteAsync<T>(T content, StringWriter writer);
    }
}
