using System.Collections.Generic;
using System.Linq;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace DoLess.Rest.Tasks
{
    internal class RestClientFactoryBuilder : CodeBuilder
    {
        private readonly IReadOnlyList<RestClientBuilder> restClientBuilders;

        public RestClientFactoryBuilder(IReadOnlyList<RestClientBuilder> restClientBuilders, string directoryPath)
            : base(directoryPath, Constants.RestClientFactoryBuilderFileName)
        {
            this.restClientBuilders = restClientBuilders;
        }

        public RestClientFactoryBuilder Build()
        {
            this.RootNode = CompilationUnit().WithUsings(List(this.BuildUsings()))
                                             .WithMembers(SingletonList<MemberDeclarationSyntax>(this.BuildNamespaceDeclaration()));
            return this;
        }

        private IEnumerable<UsingDirectiveSyntax> BuildUsings()
        {
            return this.restClientBuilders
                       .SelectMany(x => x.RestClients)
                       .Select(x => x.InterfaceDeclaration?.GetDeclaringNamespace())
                       .Union(Constants.DoLessRestGeneratedUsings)
                       .Distinct(UsingDirectiveSyntaxEqualityComparer.Default)
                       .OrderBy(x => x, UsingDirectiveSyntaxComparer.Default);
        }

        private NamespaceDeclarationSyntax BuildNamespaceDeclaration()
        {
            return Constants.DoLessRestNamespace
                            .WithMembers(SingletonList<MemberDeclarationSyntax>(this.BuildClass()));
        }

        private ClassDeclarationSyntax BuildClass()
        {
            return ClassDeclaration(Constants.RestClientFactoryName)
                  .WithModifiers(TokenList(
                      Token(SyntaxKind.PublicKeyword),
                      Token(SyntaxKind.StaticKeyword),
                      Token(SyntaxKind.PartialKeyword)))
                  .WithMembers(SingletonList<MemberDeclarationSyntax>(this.BuildInitializerFactoryMethod()));
        }

        private MethodDeclarationSyntax BuildInitializerFactoryMethod()
        {
            return MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), "InitializeRestClientFactory")
                  .WithModifiers(TokenList(Token(SyntaxKind.StaticKeyword)))
                  .WithBody(this.BuildInitializerFactoryMethodBlock());
        }

        private BlockSyntax BuildInitializerFactoryMethodBlock()
        {
            return this.restClientBuilders.SelectMany(x => x.RestClients)
                                          .Select(x => this.BuildAddRestClientExpressionStatement(x))
                                          .Do(x => Block(List<StatementSyntax>(x)));
        }

        private ExpressionStatementSyntax BuildAddRestClientExpressionStatement(RestClientInfo restClient)
        {
            return
                ExpressionStatement(
                    InvocationExpression(
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(nameof(RestClientFactory)),
                            GenericName(Identifier(nameof(RestClientFactory.SetRestClient)))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SeparatedList(
                                        new TypeSyntax[]
                                        {
                                            IdentifierName(restClient.InterfaceName),
                                            IdentifierName(restClient.ClassName)
                                        }))))));
        }
    }
}
