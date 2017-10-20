using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RestLess.Tasks.Helpers
{
    internal class SyntaxNormalizer : CSharpSyntaxRewriter
    {
        int position = -1;

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            int position = this.GetNewDotTokenPosition(node);

            if (position > 0)
            {
                node = node.WithOperatorToken(
                            Token(
                                TriviaList(new[]
                                {
                                    CarriageReturnLineFeed,
                                    Whitespace(new string(' ', position))
                                }),
                                SyntaxKind.DotToken,
                                TriviaList()));
            }
            return base.VisitMemberAccessExpression(node);
        }

        private int GetNewDotTokenPosition(SyntaxNode node)
        {
            var previousMethod = node.DescendantNodes()
                                     .OfType<InvocationExpressionSyntax>()
                                     .Where(x => x.Expression is MemberAccessExpressionSyntax)
                                     .Select(x => (MemberAccessExpressionSyntax)x.Expression)
                                     .LastOrDefault();

            if (previousMethod == null)
            {
                return -1;
            }

            if (this.position > -1)
            {
                return this.position;
            }

            this.position = previousMethod.OperatorToken
                                          .GetLocation()
                                          .GetMappedLineSpan()
                                          .StartLinePosition
                                          .Character;

            return this.position;
        }
    }
}
