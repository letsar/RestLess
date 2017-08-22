using System;

namespace DoLess.Rest.Attributes
{
    public abstract class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
