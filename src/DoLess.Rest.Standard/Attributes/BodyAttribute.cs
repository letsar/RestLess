using System;
using System.Collections.Generic;
using System.Text;

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
