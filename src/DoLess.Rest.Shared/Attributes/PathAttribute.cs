using System;

namespace DoLess.Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class PathAttribute : ParameterAttribute
    {
        public PathAttribute(string name) :
            base(name)
        {
        }
    }
}
