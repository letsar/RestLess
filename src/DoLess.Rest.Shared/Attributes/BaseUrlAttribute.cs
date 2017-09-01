using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class BaseUrlAttribute : Attribute
    {
        public BaseUrlAttribute(string baseUrl) { }
    }
}
