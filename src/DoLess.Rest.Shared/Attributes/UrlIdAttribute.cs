using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class UrlIdAttribute : Attribute
    {
        public UrlIdAttribute(string id) { }
    }
}
