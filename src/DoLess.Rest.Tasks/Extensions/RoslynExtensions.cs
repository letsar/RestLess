using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using DoLess.Rest;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Runtime.Serialization;
using DoLess.Rest.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using DoLess.Rest.Tasks.Helpers;

namespace DoLess.Rest
{
    /// <summary>
    /// 
    /// </summary>
    internal static partial class RoslynExtensions
    {
        private static readonly string DoLessRestNamespace = typeof(RestClient).Namespace;

        /// <summary>
        /// Indicates wether the specified symbol inherits from the type parameter.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="self">The symbol.</param>
        /// <returns></returns>
        public static bool InheritsFrom<T>(this INamedTypeSymbol self)
        {
            if (self == null)
            {
                return false;
            }
            else if (self.ToString() == typeof(T).FullName)
            {
                return true;
            }
            else
            {
                return InheritsFrom<T>(self.BaseType);
            }
        }

        public static string ToParsedString(this SyntaxList<TypeParameterConstraintClauseSyntax> self)
        {
            if (self.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return Environment.NewLine + "\t" + self.ToString();
            }
        }

        public static ParameterListSyntax ToParameterList(this IEnumerable<ParameterSyntax> self)
        {
            return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(self));
        }

        public static ParameterListSyntax WithoutAttributes(this ParameterListSyntax self)
        {
            if (self?.Parameters == null)
            {
                return self;
            }

            return SyntaxFactory.ParameterList(
                   SyntaxFactory.SeparatedList<ParameterSyntax>(self.Parameters
                                                                    .Select(x => x.WithoutAttributes())));
        }

        public static TypeParameterListSyntax WithoutAttributes(this TypeParameterListSyntax self)
        {
            if (self?.Parameters == null)
            {
                return self;
            }

            return SyntaxFactory.TypeParameterList(
                   SyntaxFactory.SeparatedList<TypeParameterSyntax>(self.Parameters
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

        public static bool HasReferenceToDoLessRest(this IEnumerable<SyntaxNode> self)
        {
            return self.OfType<UsingDirectiveSyntax>()
                       .Any(x => x.Name.ToFullString() == DoLessRestNamespace) ||
                   self.OfType<NamespaceDeclarationSyntax>()
                       .Any(x => x.Name.ToFullString().StartsWith(DoLessRestNamespace));
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

        public static InvocationExpressionSyntax WithArgs(this InvocationExpressionSyntax self, params string[] arguments)
        {
            return self.WithArgs(arguments.Select(x => x.ToArg()).ToArray());
        }

        public static InvocationExpressionSyntax WithArgs(this InvocationExpressionSyntax self, params ArgumentSyntax[] arguments)
        {
            return self.WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }

        public static ArgumentSyntax ToArgWithThis(this string self)
        {
            return Argument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), IdentifierName(self)));
        }

        public static ArgumentSyntax ToArg(this string self)
        {
            return Argument(IdentifierName(self));
        }

        public static ArgumentSyntax ToArgLiteral(this string self)
        {
            return Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(self)));
        }

        public static ArgumentSyntax ToArg(this Parameter self)
        {
            return self.IsMutable ? self.Value.ToArg() : self.Value.ToArgLiteral();
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
