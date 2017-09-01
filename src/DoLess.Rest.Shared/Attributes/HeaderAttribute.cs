using System;

namespace DoLess.Rest
{
    [AttributeUsage(
        AttributeTargets.Interface | 
        AttributeTargets.Method | 
        AttributeTargets.Parameter,
        AllowMultiple = true)]
    public sealed class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string name, string value = null) { }
    }
}
