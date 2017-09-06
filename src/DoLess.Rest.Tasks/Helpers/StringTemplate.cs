using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Tasks.Exceptions;

namespace DoLess.Rest.Tasks.Helpers
{
    internal class StringTemplate
    {
        // CONSIDER: to implement the RFC 6570.
        // https://tools.ietf.org/html/rfc6570
        // See implementation: https://github.com/tavis-software/Tavis.UriTemplates.

        private const char ParameterStart = '{';
        private const char ParameterEnd = '}';

        private readonly Dictionary<string, Parameter> parameters;
        private readonly List<Parameter> parts;
        private readonly StringBuilder partNameBuilder;

        private bool isInParameter;
        private char ch;
        private int position;

        private StringTemplate(string template)
        {
            this.Template = template;
            this.parameters = new Dictionary<string, Parameter>(StringComparer.OrdinalIgnoreCase);
            this.parts = new List<Parameter>();
            this.partNameBuilder = new StringBuilder();
            this.isInParameter = false;
        }

        public string Template { get; }

        public IReadOnlyList<Parameter> Parts => this.parts;

        public IReadOnlyCollection<string> ParameterNames => this.parameters.Keys;

        public IReadOnlyCollection<Parameter> Parameters => this.parameters.Values;

        public static StringTemplate Parse(string template)
        {
            var urlTemplate = new StringTemplate(template);
            urlTemplate.Parse();
            return urlTemplate;
        }

        public Parameter GetParameter(string name)
        {
            this.parameters.TryGetValue(name, out Parameter result);
            return result;
        }

        public bool IsParameter(string identifier)
        {
            return this.parameters.ContainsKey(identifier);
        }

        private void Parse()
        {
            for (this.position = 0; this.position < this.Template.Length; this.position++)
            {
                this.ch = this.Template[this.position];

                switch (this.ch)
                {
                    case ParameterEnd:
                        this.ThrowIfIsNotInParameter();
                        this.isInParameter = false;
                        this.AddParameter(true);
                        break;

                    case ParameterStart:
                        this.AddParameter(false);
                        this.isInParameter = true;
                        break;

                    default:
                        this.partNameBuilder.Append(this.ch);
                        break;
                }
            }

            this.ThrowIfIsInParameter();

            // Add the last part.
            this.AddParameter(false);           
        }

        private void AddParameter(bool isMutable)
        {
            if (this.partNameBuilder.Length > 0)
            {
                string parameterName = this.partNameBuilder.ToString();
                this.partNameBuilder.Clear();

                bool isParameterAlreadyExists = false;
                if (!isMutable || !(isParameterAlreadyExists = this.parameters.TryGetValue(parameterName, out Parameter parameter)))
                {
                    parameter = new Parameter(parameterName, isMutable);

                    if (isMutable && !isParameterAlreadyExists)
                    {
                        this.parameters[parameterName] = parameter;
                    }
                }

                this.parts.Add(parameter);
            }
            else if (isMutable)
            {
                this.ThrowExceptionWhenParameterNameIsEmpty();
            }
        }

        private void ThrowIfIsInParameter()
        {
            if (this.isInParameter)
            {
                throw new StringTemplateException($"A parameter must be closed with the character '{ParameterEnd}'.");
            }
        }

        private void ThrowIfIsNotInParameter()
        {
            if (!this.isInParameter)
            {
                throw new StringTemplateException($"A parameter must start with the character '{ParameterStart}'.");
            }
        }

        private void ThrowExceptionWhenParameterNameIsEmpty()
        {
            throw new StringTemplateException($"A parameter must have a name (position: {this.position}).");
        }
    }
}
