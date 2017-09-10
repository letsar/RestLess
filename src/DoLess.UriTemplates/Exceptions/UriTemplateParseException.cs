using System;

namespace DoLess.UriTemplates
{
    public sealed class UriTemplateParseException : UriTemplateException
    {
        internal UriTemplateParseException(string message, TemplateProcessor uriTemplateParser, Exception innerException = null)
            : base($"Malformed uri template '{uriTemplateParser.Template}' : {message} at position {uriTemplateParser.Position}.", innerException)
        {
            this.Template = uriTemplateParser.Template;
            this.Position = uriTemplateParser.Position;
            this.Parsed = this.Template.Substring(0, this.Position);
            this.NotParsed = this.Template.Substring(this.Position);
        }

        public string Template { get; }

        public string Parsed { get; }

        public string NotParsed { get; }

        public int Position { get; }
    }
}
