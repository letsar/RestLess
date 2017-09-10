using System;

namespace DoLess.UriTemplates
{
    public class UriTemplateException : Exception
    {
        public UriTemplateException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
