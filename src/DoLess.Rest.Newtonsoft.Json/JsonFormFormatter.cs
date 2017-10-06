using System.Reflection;
using System.Runtime.Serialization;
using DoLess.Rest.Helpers;
using Newtonsoft.Json;

namespace DoLess.Rest
{
    internal class JsonFormFormatter : DefaultFormFormatter
    {
        protected override string GetFallbackPropertyNameInternal(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   propertyInfo.GetCustomAttribute<DataMemberAttribute>()?.Name;
        }
    }
}
