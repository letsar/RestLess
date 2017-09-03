using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Tasks.Exceptions;

namespace DoLess.Rest.Tasks.UrlTemplating
{
    internal class UrlTemplate
    {
        private const char SegmentStart = '/';
        private const char ParameterStart = '{';
        private const char ParameterEnd = '}';
        private const char QueryStringStart = '?';
        private const char QueryEnd = '&';
        private const char QueryValueStart = '=';

        private readonly Dictionary<string, Parameter> parameters;
        private readonly List<IReadOnlyList<Parameter>> segments;
        private readonly List<IReadOnlyList<Parameter>> queryKeys;
        private readonly List<IReadOnlyList<Parameter>> queryValues;
        private readonly StringBuilder parameterNameBuilder;

        private List<Parameter> parameterList;
        private List<IReadOnlyList<Parameter>> urlPart;
        private bool isInParameter;
        private char ch;
        private int position;

        private UrlTemplate(string template)
        {
            this.Template = template;
            this.parameters = new Dictionary<string, Parameter>(StringComparer.OrdinalIgnoreCase);
            this.segments = new List<IReadOnlyList<Parameter>>();
            this.queryKeys = new List<IReadOnlyList<Parameter>>();
            this.queryValues = new List<IReadOnlyList<Parameter>>();
            this.parameterNameBuilder = new StringBuilder();
            this.parameterList = new List<Parameter>();
            this.urlPart = this.segments;
            this.isInParameter = false;
        }

        public string Template { get; }

        public IReadOnlyList<IReadOnlyList<Parameter>> Segments => this.segments;

        public IReadOnlyList<IReadOnlyList<Parameter>> QueryKeys => this.queryKeys;

        public IReadOnlyList<IReadOnlyList<Parameter>> QueryValues => this.queryValues;

        public IReadOnlyCollection<string> ParameterNames => this.parameters.Keys;

        public IReadOnlyCollection<Parameter> Parameters => this.parameters.Values;

        public Parameter GetParameter(string name)
        {
            this.parameters.TryGetValue(name, out Parameter result);
            return result;
        }

        public static UrlTemplate Parse(string template)
        {
            var urlTemplate = new UrlTemplate(template);
            urlTemplate.Parse();
            return urlTemplate;
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
                    case SegmentStart:
                        this.AddParameterList();
                        break;

                    case QueryStringStart:
                        this.AddParameterList(this.queryKeys);
                        break;

                    case QueryEnd:
                        this.AddParameterList(this.queryKeys);
                        this.AddEmptyQueryValueIfNeeded();
                        break;

                    case ParameterEnd:
                        this.ThrowIfIsNotInParameter();
                        this.isInParameter = false;
                        this.AddParameter(true);
                        break;

                    case ParameterStart:
                        this.AddParameterList(null, false);
                        this.isInParameter = true;
                        break;

                    case QueryValueStart:
                        this.AddParameterList(this.queryValues);
                        break;

                    default:
                        this.parameterNameBuilder.Append(this.ch);
                        break;
                }
            }

            this.ThrowIfIsInParameter();            

            // When the last character is the '}'. We must not duplicate the last entry.
            if (this.parameterNameBuilder.Length == 0 && this.parameterList.Count > 1)
            {
                this.parameterList = new List<Parameter>();
            }

            this.AddParameterList();
            this.AddEmptyQueryValueIfNeeded();
        }

        private void AddEmptyQueryValueIfNeeded()
        {
            if (this.queryKeys.Count > this.queryValues.Count)
            {
                this.queryValues.Add(new List<Parameter>() { new Parameter(string.Empty, false) });
            }
        }

        private void AddParameterList(List<IReadOnlyList<Parameter>> newList = null, bool resetParameterList = true)
        {
            this.ThrowIfUnauthorizedCharacterInParameter();
            this.AddParameter(false);

            if (resetParameterList && this.parameterList.Count > 0)
            {
                this.parameterList = new List<Parameter>();
            }

            if (newList != null)
            {
                this.urlPart = newList;
            }
        }

        private void AddParameter(bool isMutable)
        {
            if (this.parameterNameBuilder.Length > 0)
            {
                var parameterName = this.parameterNameBuilder.ToString();
                this.parameterNameBuilder.Clear();

                bool isParameterAlreadyExists = false;
                if (!isMutable || !(isParameterAlreadyExists = this.parameters.TryGetValue(parameterName, out Parameter parameter)))
                {
                    parameter = new Parameter(parameterName, isMutable);

                    if (isMutable && !isParameterAlreadyExists)
                    {
                        this.parameters[parameterName] = parameter;
                    }
                }

                this.parameterList.Add(parameter);

                if (this.parameterList.Count == 1)
                {
                    this.urlPart.Add(this.parameterList);
                }
            }
            else if (isMutable)
            {
                // The mutable has no name, this is not authorized.
                this.ThrowExceptionWhenParameterNameIsEmpty();
            }
        }

        private void ThrowIfIsInParameter()
        {
            if (this.isInParameter)
            {
                throw new UrlTemplateException($"A parameter must be closed with the character '{ParameterEnd}'.");
            }
        }

        private void ThrowIfIsNotInParameter()
        {
            if (!this.isInParameter)
            {
                throw new UrlTemplateException($"A parameter must start with the character '{ParameterStart}'.");
            }
        }

        private void ThrowIfUnauthorizedCharacterInParameter()
        {
            if (this.isInParameter)
            {
                throw new UrlTemplateException($"The character '{this.ch}' at position {this.position} is not authorized inside a parameter");
            }
        }

        private void ThrowExceptionWhenParameterNameIsEmpty()
        {
            throw new UrlTemplateException($"A parameter must have a name (position: {this.position}).");
        }
    }
}
