using System;

namespace DoLess.Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class QueryAttribute : ParameterAttribute
    {
        public QueryAttribute(string name) : 
            base(name)
        {
        }
    }
}
