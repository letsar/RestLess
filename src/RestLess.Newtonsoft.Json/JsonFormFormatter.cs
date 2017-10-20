using System.Reflection;
using System.Runtime.Serialization;
using RestLess.Helpers;
using Newtonsoft.Json;

namespace RestLess
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
