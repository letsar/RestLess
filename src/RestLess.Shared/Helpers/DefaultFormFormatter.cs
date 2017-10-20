using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RestLess.Helpers
{
    public class DefaultFormFormatter : IFormFormatter
    {
        private static readonly Dictionary<Type, NamedPropertyInfo[]> PropertyCache = new Dictionary<Type, NamedPropertyInfo[]>();

        public IEnumerable<KeyValuePair<string, string>> Format<T>(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
            {
                return new Dictionary<string, string>();
            }

            switch (value)
            {
                case IEnumerable<KeyValuePair<string, string>> values:
                    return values;
                case IDictionary dictionary:
                    return dictionary.Keys.Cast<object>().ToDictionary(x => x.ToString(), x => dictionary[x]?.ToString() ?? string.Empty);
                default:
                    return this.DefaultFormat(value);
            }
        }

        protected virtual string GetFallbackPropertyNameInternal(PropertyInfo propertyInfo)
        {
            return null;
        }

        private bool CanRead(PropertyInfo propertyInfo)
        {
            return propertyInfo.CanRead &&
                   propertyInfo.GetCustomAttribute<NameIgnoreAttribute>() == null;
        }

        private string GetFallbackPropertyName(PropertyInfo propertyInfo)
        {
            return this.GetFallbackPropertyNameInternal(propertyInfo) ??
                   propertyInfo.Name;
        }

        private IEnumerable<KeyValuePair<string, string>> DefaultFormat<T>(T value)
        {
            var type = typeof(T);

            lock (PropertyCache)
            {
                if (!PropertyCache.TryGetValue(type, out NamedPropertyInfo[] properties))
                {
                    properties = this.GetNamedProperties(type);
                    PropertyCache[type] = properties;
                }

                return properties.ToDictionary(x => x.Name, x => x.GetValue(value));
            }
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetRuntimeProperties()
                       .Where(x => this.CanRead(x));
        }

        private string GetPropertyName(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<NameAttribute>(true)?.Name ??
                   this.GetFallbackPropertyName(propertyInfo);
        }

        private NamedPropertyInfo[] GetNamedProperties(Type type)
        {
            return this.GetProperties(type)
                       .Select(x => new NamedPropertyInfo(this.GetPropertyName(x), x))
                       .ToArray();
        }

        private class NamedPropertyInfo
        {
            private readonly PropertyInfo propertyInfo;

            public NamedPropertyInfo(string name, PropertyInfo propertyInfo)
            {
                this.Name = name;
                this.propertyInfo = propertyInfo;
            }

            public string Name { get; }

            public string GetValue(object source) => this.propertyInfo.GetValue(source, null)?.ToString() ?? string.Empty;
        }
    }
}
