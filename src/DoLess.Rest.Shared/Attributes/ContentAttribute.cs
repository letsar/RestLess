using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ContentAttribute : Attribute
    {
        public ContentAttribute(string fileName = null, string contentType = null)
        {
        }
    }
}
