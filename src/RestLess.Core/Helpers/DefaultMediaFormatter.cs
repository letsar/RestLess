using System.IO;
using System.Threading.Tasks;

namespace RestLess.Helpers
{
    /// <summary>
    /// Dummy media formatter.
    /// </summary>
    public class DefaultMediaFormatter : IMediaTypeFormatter
    {
        public string MediaType => "text/plain";

        public Task<T> ReadAsync<T>(TextReader reader)
        {
            return Task.FromResult(default(T));
        }

        public Task WriteAsync<T>(T content, TextWriter writer)
        {
            return writer.WriteAsync(content.ToString());
        }
    }
}
