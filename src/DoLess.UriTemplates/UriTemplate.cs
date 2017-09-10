using System;
using System.Collections.Generic;

namespace DoLess.UriTemplates
{
    /// <summary>
    /// Represents a uri template as described in https://tools.ietf.org/html/rfc6570.
    /// </summary>
    public class UriTemplate
    {
        private readonly Dictionary<string, object> variables;
        private readonly string template;

        private UriTemplate(string template, bool isVariableNameCaseSensitive)
        {
            this.template = template;
            this.variables = new Dictionary<string, object>(isVariableNameCaseSensitive ? null : StringComparer.OrdinalIgnoreCase);
        }

        public string Template => this.template;

        public IReadOnlyCollection<string> ParameterNames => this.variables.Keys;

        public static UriTemplate For(string template, bool isVariableNameCaseSensitive = true)
        {
            return new UriTemplate(template, isVariableNameCaseSensitive);
        }

        public UriTemplate WithoutParameter(string name)
        {
            this.variables.Remove(name);
            return this;
        }

        public UriTemplate WithParameter<T>(string name, T value)
        {
            this.variables[name] = value;
            return this;
        }

        public string ExpandToString()
        {
            TemplateProcessor templateProcessor = new TemplateProcessor(this.template, this.variables);
            return templateProcessor.Expand();
        }

        public Uri ExpandToUri()
        {
            return new Uri(this.ExpandToString());
        }

        public override string ToString()
        {
            return this.template;
        }
    }
}
