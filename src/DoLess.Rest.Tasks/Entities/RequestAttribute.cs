using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks
{
    internal class RequestAttribute
    {
        public RequestAttribute(AttributeSyntax attributeSyntax)
        {

            this.ClassName = attributeSyntax.GetClassName();
            this.Arguments = attributeSyntax.ArgumentList?
                                            .Arguments
                                            .Select(x => SyntaxFactory.Argument(x.Expression))
                                            .ToArray();
            this.ArgumentCount = (this.Arguments?.Length).GetValueOrDefault();
            this.AttachedParameterName = attributeSyntax.GetParameterName();
            this.Type = attributeSyntax.GetRequestAttributeType();
        }

        public string ClassName { get; }

        public ArgumentSyntax[] Arguments { get; }

        public int ArgumentCount { get; }

        public string AttachedParameterName { get; }

        public bool HasAttachedParameter => this.AttachedParameterName != null;

        public RequestAttributeType Type { get; }

        public ArgumentSyntax GetArgument(int index)
        {
            if (this.ArgumentCount > index)
            {
                return this.Arguments[index];
            }
            else
            {
                return null;
            }
        }
    }
}
