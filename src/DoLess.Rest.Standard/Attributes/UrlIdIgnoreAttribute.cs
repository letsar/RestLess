using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UrlIdIgnoreAttribute : Attribute
    {
    }
}
