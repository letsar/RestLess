using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public sealed class UrlIdAttribute : Attribute
    {
        public UrlIdAttribute(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
    }
}
