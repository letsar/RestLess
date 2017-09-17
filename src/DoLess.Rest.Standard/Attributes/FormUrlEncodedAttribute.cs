using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FormUrlEncodedAttribute : Attribute
    {
    }
}
