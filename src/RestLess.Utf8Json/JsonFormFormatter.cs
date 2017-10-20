using System.Reflection;
using System.Runtime.Serialization;
using RestLess.Helpers;

namespace RestLess
{
    internal class JsonFormFormatter : DefaultFormFormatter
    {
        protected override string GetFallbackPropertyNameInternal(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<DataMemberAttribute>()?.Name;
        }
    }
}
