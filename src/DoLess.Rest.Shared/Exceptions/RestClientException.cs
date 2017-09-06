using System;

namespace DoLess.Rest.Exceptions
{
    public class RestClientException : Exception
    {
        public RestClientException(string message) 
            : base(message)
        {
        }

        public RestClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
