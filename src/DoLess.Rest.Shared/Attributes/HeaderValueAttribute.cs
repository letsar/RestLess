using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class HeaderValueAttribute : Attribute
    {
        public HeaderValueAttribute(string name) { }
    }
}
