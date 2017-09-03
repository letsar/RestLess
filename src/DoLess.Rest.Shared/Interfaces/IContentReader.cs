using System.IO;
using System.Threading.Tasks;

namespace DoLess.Rest
{
    /// <summary>
    /// Represents an object that can read the content of a <see cref="System.Net.Http.HttpResponseMessage"/>.
    /// </summary>
    public interface IContentReader
    {
        /// <summary>
        /// Returns the content inside the <paramref name="reader"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="reader">The stream reader.</param>
        /// <returns></returns>
        Task<T> ReadAsync<T>(StreamReader reader);
    }
}
