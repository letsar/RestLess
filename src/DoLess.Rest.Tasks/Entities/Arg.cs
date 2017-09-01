using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DoLess.Rest.Tasks
{
    [DebuggerDisplay("{Text}")]
    internal class Arg
    {
        private Arg(ExpressionSyntax expression, string text)
        {
            this.Text = text;
            this.ArgumentSyntax = Argument(expression);
        }

        public string Text { get; }

        public ArgumentSyntax ArgumentSyntax { get; }

        public bool IsIdentifier => this.ArgumentSyntax.Expression.IsKind(SyntaxKind.IdentifierName);

        public static Arg Literal(string value)
        {
            return new Arg(LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value)), value);
        }

        public static Arg Identifier(string name)
        {
            return new Arg(IdentifierName(name), name);
        }
    }
}
