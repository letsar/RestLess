using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.Generated;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace DoLess.Rest.Tasks
{
    internal class RestClientFactoryBuilder : CodeBuilder
    {
        private const string InitializeRestClientMethodName = "InitializeRestClient";
        private const string RestClientFactoryName = "RestClientFactory";
        private readonly IReadOnlyList<RestClientBuilder> restClientBuilders;

        public RestClientFactoryBuilder(IReadOnlyList<RestClientBuilder> restClientBuilders)
            : base($"{Constants.RestClientFactoryName}.cs", Constants.ProductName)
        {
            this.restClientBuilders = restClientBuilders;
        }

        public RestClientFactoryBuilder Build()
        {
            string restClientFactoryFilePath = Path.Combine(Path.GetDirectoryName(typeof(RestClient).Assembly.Location), $"{Constants.RestClientFactoryName}.cs");
            var compilationUnit = CSharpSyntaxTree.ParseText(File.ReadAllText(restClientFactoryFilePath, Encoding.UTF8))
                                            .GetCompilationUnitRoot()
                                            .WithUsings(List(this.BuildUsings()));

            compilationUnit = compilationUnit.WithMembers(SingletonList<MemberDeclarationSyntax>(this.BuildNamespaceDeclaration(compilationUnit.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault()?.Members)));

            this.RootNode = compilationUnit;

            return this;
        }

        private IEnumerable<UsingDirectiveSyntax> BuildUsings()
        {
            return this.restClientBuilders
                       .SelectMany(x => x.RestClients)
                       .Select(x => x.InterfaceDeclaration?.GetDeclaringNamespace())
                       .Union(Constants.DoLessRestFactoryRequiredUsings)
                       .Distinct(UsingDirectiveSyntaxEqualityComparer.Default)
                       .OrderBy(x => x, UsingDirectiveSyntaxComparer.Default);
        }

        private NamespaceDeclarationSyntax BuildNamespaceDeclaration(IEnumerable<MemberDeclarationSyntax> members)
        {
            return Constants.DoLessRestNamespace
                            .WithMembers(SingletonList<MemberDeclarationSyntax>(this.BuildClass(members)));
        }

        private ClassDeclarationSyntax BuildClass(IEnumerable<MemberDeclarationSyntax> members)
        {
            return ClassDeclaration(Constants.RestClientFactoryName)
                  .WithModifiers(TokenList(
                      Token(SyntaxKind.PublicKeyword),
                      Token(SyntaxKind.StaticKeyword)))
                  .WithMembers(List(members))
                  .AddMembers(this.BuildConstructorDeclaration(), this.BuildInitializerFactoryMethod());
        }

        private ConstructorDeclarationSyntax BuildConstructorDeclaration()
        {
            return ConstructorDeclaration(Constants.RestClientFactoryName)
                   .WithModifiers(TokenList(Token(SyntaxKind.StaticKeyword)))
                   .WithBody(Block(
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName(RestClientFactoryName),
                                        ObjectCreationExpression(IdentifierName(RestClientFactoryName)).WithArgumentList(ArgumentList()))),
                                ExpressionStatement(InvocationExpression(IdentifierName(InitializeRestClientMethodName)))));
        }

        private MethodDeclarationSyntax BuildInitializerFactoryMethod()
        {
            return MethodDeclaration(PredefinedType(Token(SyntaxKind.VoidKeyword)), InitializeRestClientMethodName)
                  .WithModifiers(TokenList(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.StaticKeyword)))
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
