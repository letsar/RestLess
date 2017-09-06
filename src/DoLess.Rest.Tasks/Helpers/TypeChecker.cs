using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Helpers
{
    internal class TypeChecker
    {
        private readonly HashSet<string> namespaces;
        private readonly Dictionary<string, string> aliases;
        private readonly string typeDeclarationNamespace;

        public TypeChecker(TypeDeclarationSyntax typeDeclaration)
        {
            this.typeDeclarationNamespace = BuildTypeNamespace(typeDeclaration);
            BuildNamespaces(typeDeclaration, this.aliases, this.namespaces);
        }

        public bool IsSameType<T>(TypeSyntax typeSyntax)
        {
            return this.IsSameType(typeof(T), typeSyntax);
        }

        public global::System.Threading.Tasks.Task T()
        {
            return global::System.Threading.Tasks.Task.CompletedTask;
        }

        public bool IsSameType(Type type, TypeSyntax typeSyntax)
        {
            if (type.IsArray)
            {
                return typeSyntax is ArrayTypeSyntax arrayType &&
                       this.IsSameType(type.GetElementType(), arrayType.ElementType);
            }
            else
            {
                string typeName = type.Name;
                string typeNamespace = type.Namespace;

                switch (typeSyntax)
                {
                    case PredefinedTypeSyntax node:
                        return type.IsPrimitive && IsSamePredefinedType(typeName, node);

                    case SimpleNameSyntax node:
                        return node.Identifier.Text == typeName &&
                               (this.namespaces.Contains(typeNamespace) || this.typeDeclarationNamespace.StartsWith(typeNamespace));
                    case QualifiedNameSyntax node:
                        return node.Right.Identifier.Text == typeName &&
                               node.Left.ToString() == typeNamespace;

                    default:
                        return false;
                }
            }
        }

        private static bool IsSamePredefinedType(string typeName, PredefinedTypeSyntax typeSyntax)
        {
            switch (typeName)
            {
                case nameof(Byte):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.ByteKeyword);
                case nameof(SByte):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.ByteKeyword);
                case nameof(Int16):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.ShortKeyword);
                case nameof(UInt16):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.UShortKeyword);
                case nameof(Int32):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.IntKeyword);
                case nameof(UInt32):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.UIntKeyword);
                case nameof(Int64):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.LongKeyword);
                case nameof(UInt64):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.ULongKeyword);
                case nameof(String):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.StringKeyword);
                case nameof(Object):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.ObjectKeyword);
                case nameof(Boolean):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.BoolKeyword);
                case nameof(Char):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.CharKeyword);
                case nameof(Decimal):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.DecimalKeyword);
                case nameof(Double):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.DoubleKeyword);
                case nameof(Single):
                    return typeSyntax.Keyword.IsKind(SyntaxKind.FloatKeyword);
                default:
                    return false;
            }
        }

        private static void BuildNamespaces(TypeDeclarationSyntax typeDeclaration, Dictionary<string, string> aliases, HashSet<string> namespaces)
        {
            typeDeclaration.Ancestors()
                           .SelectMany(x => x.GetUsings())
                           .ForEach(x =>
                           {
                               if (x.Alias != null)
                               {
                                   aliases[x.Alias.Name.ToString()] = x.Name.ToString();
                               }
                               else
                               {
                                   namespaces.Add(x.Name.ToString());
                               }
                           });
        }

        private static string BuildTypeNamespace(TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Ancestors()
                                  .OfType<NamespaceDeclarationSyntax>()
                                  .Select(x => x.Name.ToString())
                                  .Reverse()
                                  .Do(x => string.Join(".", x));
        }
    }
}
