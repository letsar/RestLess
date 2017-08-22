using System;

namespace DoLess.Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class HeaderAttribute : ParameterAttribute
    {
        public HeaderAttribute(string name, string value = null) :
            base(name)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}
