using System.Collections.Generic;
using System.Linq;
using RestLess.Tasks;
using RestLess.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace RestLess
{
    internal static partial class RoslynExtensions
    {
        private static readonly string RestLessNamespace = typeof(IRestClient).Namespace;

        public static ParameterListSyntax WithoutAttributes(this ParameterListSyntax self)
        {
            if (self?.Parameters == null)
            {
                return self;
            }

            return ParameterList(
                   SeparatedList(self.Parameters
                                     .Select(x => x.WithoutAttributes())));
        }

        public static TypeParameterListSyntax WithoutAttributes(this TypeParameterListSyntax self)
        {
            if (self?.Parameters == null)
            {
                return self;
            }

            return TypeParameterList(
                   SeparatedList(self.Parameters
                                     .Select(x => x.WithoutAttributes())));
        }

        public static MethodDeclarationSyntax WithoutAttributes(this MethodDeclarationSyntax self)
        {
            return self.WithAttributeLists(default(SyntaxList<AttributeListSyntax>));
        }

        public static ParameterSyntax WithoutAttributes(this ParameterSyntax self)
        {
            return self.WithAttributeLists(default(SyntaxList<AttributeListSyntax>));
        }

        public static TypeParameterSyntax WithoutAttributes(this TypeParameterSyntax self)
        {
            return self.WithAttributeLists(default(SyntaxList<AttributeListSyntax>));
        }

        public static bool HasReferenceToRestLess(this IEnumerable<SyntaxNode> self)
        {
            return self.OfType<UsingDirectiveSyntax>()
                       .Any(x => x.Name.ToFullString() == RestLessNamespace) ||
                   self.OfType<NamespaceDeclarationSyntax>()
                       .Any(x => x.Name.ToFullString().StartsWith(RestLessNamespace));
        }

        public static TypeSyntax GetTypeSyntax(this TypeDeclarationSyntax self)
        {
            var typeName = self.Identifier.Text;
            return self.TypeParameterList?.Parameters != null ?
                   GenericName(
                       Identifier(typeName),
                       TypeArgumentList(SeparatedList(self.TypeParameterList.Parameters.Select(x => ParseTypeName(x.Identifier.Text))))) :
                   ParseTypeName(typeName);
        }

        public static IEnumerable<UsingDirectiveSyntax> GetUsings(this SyntaxNode self)
        {
            switch (self)
            {
                case CompilationUnitSyntax node:
                    return node.Usings;
                case NamespaceDeclarationSyntax node:
                    return node.Usings;
                default:
                    return Enumerable.Empty<UsingDirectiveSyntax>();
            }
        }

        public static bool HasUsingDirective(this SyntaxNode self, string usingName)
        {
            return self.AncestorsAndSelf()
                       .SelectMany(x => x.GetUsings())
                       .Any(x => x.Name.ToString() == usingName);
        }

        public static IEnumerable<UsingDirectiveSyntax> GetRequiredUsings(this InterfaceDeclarationSyntax self)
        {
            return self.AncestorsAndSelf()
                       .SelectMany(x => x.GetUsings())
                       .Union(new[] { self.GetDeclaringNamespace() });
        }

        public static UsingDirectiveSyntax GetDeclaringNamespace(this InterfaceDeclarationSyntax self)
        {
            return UsingDirective(ParseName(self.Ancestors().OfType<NamespaceDeclarationSyntax>().Select(x => x.Name.ToString()).Concatenate(".")));
        }

        public static string GetTypeName(this TypeSyntax self)
        {
            switch (self)
            {
                case SimpleNameSyntax node:
                    return node.Identifier.Text;
                case QualifiedNameSyntax node:
                    return node.Right.Identifier.Text;
                default:
                    return null;
            }
        }

        public static InvocationExpressionSyntax WithArgs(this InvocationExpressionSyntax self, params ArgumentSyntax[] arguments)
        {
            return self.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        public static ArgumentSyntax ToArg(this string self)
        {
            return Argument(IdentifierName(self));
        }

        public static ArgumentSyntax ToArgLiteral(this string self)
        {
            return Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(self)));
        }

        public static InvocationExpressionSyntax ChainWith(this InvocationExpressionSyntax self, string methodName)
        {
            return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, self, IdentifierName(methodName)));
        }

        public static InvocationExpressionSyntax ChainWith(this InvocationExpressionSyntax self, string methodName, TypeSyntax genericType)
        {
            return InvocationExpression(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, self, GenericName(methodName).AddTypeArgumentListArguments(genericType)));
        }

        public static SyntaxNode Normalize(this SyntaxNode self)
        {
            var normalizer = new SyntaxNormalizer();
            return normalizer.Visit(self);
        }
    }
}
