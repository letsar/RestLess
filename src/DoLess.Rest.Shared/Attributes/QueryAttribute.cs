using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueryAttribute : Attribute
    {
        public QueryAttribute(string name = null) { }
    }
}
