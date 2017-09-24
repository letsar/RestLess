using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class BodyAttribute : Attribute
    {
        public BodyAttribute()
        {
        }
    }
}
