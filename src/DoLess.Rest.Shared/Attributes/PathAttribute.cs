using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class PathAttribute : Attribute
    {
        public PathAttribute(string name = null) { }
    }
}
