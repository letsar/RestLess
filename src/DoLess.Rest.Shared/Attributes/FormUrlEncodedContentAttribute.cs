using System;

namespace DoLess.Rest
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class FormUrlEncodedContentAttribute : Attribute
    {
    }
}
