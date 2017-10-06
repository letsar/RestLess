using System.Reflection;
using System.Runtime.Serialization;
using DoLess.Rest.Helpers;

namespace DoLess.Rest
{
    internal class JsonFormFormatter : DefaultFormFormatter
    {
        protected override string GetFallbackPropertyNameInternal(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<DataMemberAttribute>()?.Name;
        }
    }
}
