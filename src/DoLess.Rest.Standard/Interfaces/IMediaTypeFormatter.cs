using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DoLess.Rest
{
    public interface IMediaTypeFormatter
    {
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
        /// <returns></returns>
        Task WriteAsync<T>(T content, TextWriter writer);

    }
}
