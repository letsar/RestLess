using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest
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

        private static readonly HashSet<string> OtherRestAttributeNames =
            new[]
            {
                nameof(UrlIdAttribute),
                nameof(BaseUrlAttribute),
                nameof(HeaderAttribute),
                nameof(BodyAttribute)
            }
            .ToAttributeNamesHashSet();

        private static readonly char[] NamespaceSeparators = new[] { '.' };

        public static bool IsRestInterface(this InterfaceDeclarationSyntax self)
        {
            return self.Members
                       .All(x => x.IsRestMethodDeclaration());
        }

        public static bool HasRestAttribute(this MethodDeclarationSyntax self)
        {
            return self.AttributeLists
                       .SelectMany(x => x.Attributes)
                       .Any(x => x.IsRestAttribute());
        }

        public static string GetClassName(this AttributeSyntax self)
        {
            return self.Name
                       .ToString()
                       .Split(NamespaceSeparators, StringSplitOptions.RemoveEmptyEntries)
                       .LastOrDefault()?
                       .Replace(nameof(Attribute), string.Empty);
        }

        public static bool IsRestAttribute(this AttributeSyntax self)
        {
            if (self == null)
            {
                return false;
            }

            string className = self.GetClassName();
            return HttpMethodAttributeNames.Contains(className) ||
                   OtherRestAttributeNames.Contains(className);
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

        //public static IReadOnlyList<RestAttribute> ToRestAttributes(this SyntaxList<AttributeListSyntax> self)
        //{
        //    return self.SelectMany(x => x.Attributes)
        //               .Where(x => x.IsRestAttribute())
        //               .Select(x => x.ToRestAttribute())
        //               .ToList();
        //}

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
            else
            {
                string fullClassName = className + (nameof(Attribute));
                switch (fullClassName)
                {
                    case nameof(UrlIdAttribute):
                        return RequestAttributeType.UrlId;
                    case nameof(BaseUrlAttribute):
                        return RequestAttributeType.BaseUrl;
                    case nameof(HeaderAttribute):
                        return RequestAttributeType.Header;
                    case nameof(BodyAttribute):
                        return RequestAttributeType.Body;
                    default:
                        return default(RequestAttributeType);
                }
            }
        }

        private static bool IsInAttributeSet(this AttributeSyntax self, HashSet<string> set)
        {
            return self != null && set.Contains(self.GetClassName());
        }

        private static bool IsAttribute(this AttributeSyntax self, string attributeFullName)
        {
            return (self.GetClassName() + nameof(Attribute)) == attributeFullName;
        }
    }
}
