using System;

namespace DoLess.Rest.Exceptions
{
    /// <summary>
    /// Represents an exception raised by DoLess.Rest.
    /// </summary>
    public class RestClientException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="RestClientException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        public RestClientException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new <see cref="RestClientException"/>.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RestClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
