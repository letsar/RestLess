using System.Net.Http.Headers;

namespace RestLess
{
    /// <summary>
    /// Represents an object that can populate an object with header values.
    /// </summary>    
    public interface IHeaderWriter
    {
        /// <summary>
        /// Writes some header values to the specified object.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="obj">The object.</param>
        void Write(HttpResponseHeaders headers, object obj);
    }
}
