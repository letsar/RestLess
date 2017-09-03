using System;

namespace DoLess.Rest.Tasks.Exceptions
{
    internal class UrlTemplateException : Exception
    {
        public UrlTemplateException(string message) :
            base(message)
        {
        }
    }
}
