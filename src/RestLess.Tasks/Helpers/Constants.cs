using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace RestLess.Tasks.Helpers
{
    public static class Constants
    {
        public const string ProductName = "RestLess";
        public const string RestClientPrefix = "RestClientFor";
        public const string RestClientFactoryName = "RestClient";
        public const string DoLessGeneratedFileSuffix = ".g.rl.cs";
        public const string RestClientFactoryBuilderFileName = RestClientFactoryName + DoLessGeneratedFileSuffix;

        public static readonly NamespaceDeclarationSyntax DoLessRestNamespace = NamespaceDeclaration(ParseName(ProductName));
        public static readonly NamespaceDeclarationSyntax DoLessRestGeneratedNamespace = NamespaceDeclaration(ParseName(ProductName + ".Generated"));
        public static readonly IReadOnlyList<UsingDirectiveSyntax> DoLessRestFactoryRequiredUsings = new[]
        {
            UsingDirective(DoLessRestGeneratedNamespace.Name),
            UsingDirective(ParseName("System")),
            UsingDirective(ParseName("System.Net.Http"))
        };
    }
}
