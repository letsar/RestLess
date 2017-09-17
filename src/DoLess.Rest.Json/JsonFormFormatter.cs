using System.Reflection;
using System.Runtime.Serialization;
using DoLess.Rest.Helpers;
using Newtonsoft.Json;

namespace DoLess.Rest.Json
{
    internal class JsonFormFormatter : DefaultFormFormatter
    {
        protected override string GetFallbackPropertyNameInternal(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ??
                   propertyInfo.GetCustomAttribute<DataMemberAttribute>()?.Name;
        }

        protected override bool CanReadInternal(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>() == null &&
                   propertyInfo.GetCustomAttribute<IgnoreDataMemberAttribute>() == null;
        }
    }
}
