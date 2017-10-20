using System;
using System.Collections.Generic;
using System.Linq;
using RestLess.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess
{
    internal static partial class RoslynExtensions
    {
        private static readonly HashSet<string> HttpMethodAttributeNames =
            new[]
            {
                nameof(DeleteAttribute),
                nameof(GetAttribute),
                nameof(HeadAttribute),
                nameof(OptionsAttribute),
                nameof(PatchAttribute),
                nameof(PostAttribute),
                nameof(PutAttribute),
                nameof(TraceAttribute)
            }
            .ToAttributeNamesHashSet();

        private static readonly HashSet<string> FormatterAttributeNames =
            new[]
            {
                        nameof(MediaTypeFormatterAttribute),
                        nameof(UrlParameterFormatterAttribute),
                        nameof(FormFormatterAttribute),
            }
            .ToAttributeNamesHashSet();

        private static readonly HashSet<string> OtherRestAttributeNames =
            new[]
            {
                nameof(NameAttribute),
                nameof(UriTemplatePrefixAttribute),
                nameof(UriTemplateSuffixAttribute),
                nameof(HeaderAttribute),
                nameof(HeaderValueAttribute),
                nameof(ContentAttribute),
                nameof(FormUrlEncodedContentAttribute),
            }
            .ToAttributeNamesHashSet();

        private static readonly char[] NamespaceSeparators = new[] { '.' };

        public static bool IsRestInterface(this InterfaceDeclarationSyntax self)
        {
            return self.Members
                       .All(x => x.IsRestMethodDeclaration());
        }

        public static string GetClassName(this AttributeSyntax self)
        {
            return self.Name
                       .ToString()
                       .Split(NamespaceSeparators, StringSplitOptions.RemoveEmptyEntries)
                       .LastOrDefault()?
                       .Replace(nameof(Attribute), string.Empty);
        }


        public static bool IsRestMethodDeclaration(this MemberDeclarationSyntax self)
        {
            if (self is MethodDeclarationSyntax methodDeclarationSyntax)
            {
                return methodDeclarationSyntax.AttributeLists
                                              .SelectMany(x => x.Attributes)
                                              .Any(x => x.IsInAttributeSet(HttpMethodAttributeNames));
            }
            return false;
        }

        public static T GetAttachedElement<T>(this AttributeSyntax self)
            where T : SyntaxNode
        {
            return self.Parent?.Parent as T;
        }

        public static string GetParameterName(this AttributeSyntax self)
        {
            return self.GetAttachedElement<ParameterSyntax>()?
                       .Identifier
                       .Text;
        }

        public static RequestAttribute ToRequesAttribute(this AttributeSyntax self)
        {
            return new RequestAttribute(self);
        }

        public static IEnumerable<RequestAttribute> ToRequesAttributes(this SyntaxList<AttributeListSyntax> self)
        {
            return self.SelectMany(x => x.Attributes)
                       .Select(x => x.ToRequesAttribute());
        }

        public static RequestAttributeType GetRequestAttributeType(this AttributeSyntax self)
        {
            string className = self.GetClassName();
            if (self.IsInAttributeSet(HttpMethodAttributeNames))
            {
                return RequestAttributeType.HttpMethod;
            }
            else if (self.IsInAttributeSet(FormatterAttributeNames))
            {
                return RequestAttributeType.Formatter;
            }
            else
            {
                string fullClassName = className + (nameof(Attribute));
                switch (fullClassName)
                {
                    case nameof(NameAttribute):
                        return RequestAttributeType.Name;
                    case nameof(UriTemplatePrefixAttribute):
                        return RequestAttributeType.UriTemplatePrefix;
                    case nameof(UriTemplateSuffixAttribute):
                        return RequestAttributeType.UriTemplateSuffix;
                    case nameof(HeaderAttribute):
                        return RequestAttributeType.Header;
                    case nameof(HeaderValueAttribute):
                        return RequestAttributeType.HeaderValue;
                    case nameof(ContentAttribute):
                        return RequestAttributeType.Content;
                    case nameof(FormUrlEncodedContentAttribute):
                        return RequestAttributeType.FormUrlEncodedContent;
                    default:
                        return default;
                }
            }
        }

        private static bool IsInAttributeSet(this AttributeSyntax self, HashSet<string> set)
        {
            return self != null && set.Contains(self.GetClassName());
        }
    }
}
