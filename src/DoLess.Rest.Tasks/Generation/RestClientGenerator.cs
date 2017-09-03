using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DoLess.Rest.Tasks
{
    internal class RestClientGenerator : CSharpSyntaxRewriter
    {
        private RequestInfo requestInfo;
        private RequestInfo methodRequestInfo;
        private string className;

        private RestClientGenerator()
        {
        }

        public static SyntaxNode Generate(SyntaxNode rootNode)
        {
            var generator = new RestClientGenerator();
            return generator.Visit(rootNode);
        }

        public static ClassDeclarationSyntax Generate(InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            var generator = new RestClientGenerator();
            return generator.Visit(interfaceDeclarationSyntax) as ClassDeclarationSyntax;
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (node.IsRestInterface())
            {
                this.requestInfo = new RequestInfo(node);

                this.className = $"{Constants.RestClientPrefix}{node.Identifier.ValueText}";
                var classDeclaration = ClassDeclaration(className)
                                      .AddModifiers(Token(SyntaxKind.InternalKeyword))
                                      .AddModifiers(Token(SyntaxKind.SealedKeyword))
                                      .WithTypeParameterList(node.TypeParameterList)
                                      .WithConstraintClauses(node.ConstraintClauses)
                                      .AddMembers(ImplementConstructor())
                                      .AddMembers(ImplementMethods(node.Members))
                                      .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(new[]
                                      {
                                          SimpleBaseType(IdentifierName(nameof(RestClient))),
                                          SimpleBaseType(node.GetTypeSyntax())
                                      })));

                return classDeclaration;
            }
            else
            {
                return null;
            }
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            return null;
        }

        private MemberDeclarationSyntax ImplementConstructor()
        {
            return ConstructorDeclaration(this.className)
                  .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                  .WithParameterList(NewParameterList(NewParameter("HttpClient", "httpClient"), NewParameter("RestSettings", "settings")))
                  .WithInitializer(ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, NewArgumentList("httpClient", "settings")))
                  .WithBody(Block());
        }

        private MemberDeclarationSyntax[] ImplementMethods(SyntaxList<MemberDeclarationSyntax> syntaxList)
        {
            return syntaxList.OfType<MethodDeclarationSyntax>()
                             .Select(x => this.ImplementMethod(x))
                             .ToArray();
        }

        private MethodDeclarationSyntax ImplementMethod(MethodDeclarationSyntax node)
        {
            var methodDeclaration = node.WithoutAttributes()
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithTypeParameterList(node.TypeParameterList.WithoutAttributes())
                                        .WithParameterList(node.ParameterList.WithoutAttributes())
                                        .WithBody(this.ImplementMethodBody(node))
                                        .WithSemicolonToken(MissingToken(SyntaxKind.SemicolonToken));

            return methodDeclaration;
        }



        private BlockSyntax ImplementMethodBody(MethodDeclarationSyntax node)
        {
            this.methodRequestInfo = this.requestInfo.WithMethod(node);

            return Block(ReturnStatement(ImplementRequest()));
        }

        private ExpressionSyntax ImplementRequest()
        {
            InvocationExpressionSyntax result = NewMethodInvocation(nameof(RestRequest), this.methodRequestInfo.HttpMethod)
                                               .WithArgs(Argument(ThisExpression()));

            result = this.ChainWithRequestUrlBuilding(result);
            result = this.ChainWithHeaders(result);
            result = this.ChainWithSendMethod(result);

            return result;
        }

        private InvocationExpressionSyntax ChainWithRequestUrlBuilding(InvocationExpressionSyntax invocationExpression)
        {
            //Add the BaseUrl if any.
            if (!string.IsNullOrEmpty(this.methodRequestInfo.BaseUrl))
            {
                invocationExpression = invocationExpression.ChainWith(nameof(RestRequest.AppendUrl))
                                                           .WithArgs(this.methodRequestInfo.BaseUrl.ToArgLiteral());
            }

            var stringTemplate = this.methodRequestInfo.StringTemplate;
            stringTemplate.Parts
                          .ForEach(x =>
                          {
                              invocationExpression = invocationExpression.ChainWith(nameof(RestRequest.AppendUrl))
                                                                         .WithArgs(x.ToArg());

                              if (x.IsMutable)
                              {
                                  invocationExpression = invocationExpression.AddArgumentListArguments(NewTrueArgument());
                              }
                          });

            var queries = this.methodRequestInfo.Queries;
            if (queries.Count > 0)
            {
                queries.ForEach(x =>
                       {
                           invocationExpression = invocationExpression.ChainWith(nameof(RestRequest.AddQuery))
                                                                      .AddArgumentListArguments(x.Key.ToArgLiteral(), x.Value.ToArg());
                       });
            }


            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithHeaders(InvocationExpressionSyntax invocationExpression)
        {
            var headers = this.methodRequestInfo.Headers;
            if (headers.Count > 0)
            {
                headers.ForEach(x =>
                       {
                           invocationExpression = invocationExpression.ChainWith(nameof(RestRequest.WithHeader))
                                                                      .WithArgs(x.Key.ToArgLiteral(), x.Value.ToArg());
                       });
            }

            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithSendMethod(InvocationExpressionSyntax invocationExpression)
        {
            return invocationExpression;
        }

        private static ParameterSyntax NewParameter(string type, string identifier)
        {
            return Parameter(Identifier(identifier)).WithType(IdentifierName(type));
        }

        private static ParameterListSyntax NewParameterList(params ParameterSyntax[] parameters)
        {
            return ParameterList(SeparatedList(parameters));
        }

        private static ArgumentListSyntax NewArgumentList(params string[] identifiers)
        {
            return ArgumentList(SeparatedList(identifiers.Select(x => x.ToArg())));
        }

        private static InvocationExpressionSyntax NewMethodInvocation(string identifier, string method)
        {
            return
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(identifier),
                        IdentifierName(method)));
        }

        private static ArgumentSyntax NewFalseArgument()
        {
            return Argument(LiteralExpression(SyntaxKind.FalseLiteralExpression));
        }

        private static ArgumentSyntax NewTrueArgument()
        {
            return Argument(LiteralExpression(SyntaxKind.TrueLiteralExpression));
        }

    }
}
