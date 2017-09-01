using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                                            .Select(x => x.ToString())
                                            .ToList();
            this.ArgumentCount = (this.Arguments?.Count).GetValueOrDefault();
            this.AttachedParameterName = attributeSyntax.GetParameterName();
            this.Type = attributeSyntax.GetRequestAttributeType();
        }

        public string ClassName { get; }

        public IReadOnlyList<string> Arguments { get; }

        public int ArgumentCount { get; }

        public string AttachedParameterName { get; }

        public bool HasAttachedParameter => this.AttachedParameterName != null;

        public RequestAttributeType Type { get; }

        public string GetArgument(int index)
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
