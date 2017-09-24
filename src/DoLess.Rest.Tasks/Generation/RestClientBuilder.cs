using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;


namespace DoLess.Rest.Tasks
{
    internal class RestClientBuilder : CodeBuilder
    {
        private readonly string originalFilePath;
        private RequestInfo interfaceRequestInfo;
        private RequestInfo methodRequestInfo;

        public RestClientBuilder(string originalFilePath)
            : base(originalFilePath)
        {
            this.originalFilePath = originalFilePath;
        }

        public bool HasRestInterfaces { get; private set; }

        public bool HasReferenceToDoLessRest { get; private set; }

        public SyntaxNode OriginalRoot { get; private set; }

        public IReadOnlyList<UsingDirectiveSyntax> RequiredUsings { get; private set; }

        public IReadOnlyList<RestClientInfo> RestClients { get; private set; }

        public RestClientBuilder Build()
        {
            var content = File.ReadAllText(this.originalFilePath, Encoding.UTF8);
            this.OriginalRoot = CSharpSyntaxTree.ParseText(content).GetRoot();
            var nodes = this.OriginalRoot
                            .DescendantNodes()
                            .ToArray();
            this.HasReferenceToDoLessRest = nodes.HasReferenceToDoLessRest();

            if (this.HasReferenceToDoLessRest)
            {
                var restInterfaces = nodes.OfType<InterfaceDeclarationSyntax>()
                                          .Where(x => x.IsRestInterface())
                                          .ToArray();

                this.HasRestInterfaces = restInterfaces.Length > 0;
                if (this.HasRestInterfaces)
                {
                    this.RestClients = restInterfaces.Select(x => new RestClientInfo(x))
                                                     .ToList();

                    this.RootNode = this.BuildRootNode();
                }
            }

            return this;
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

        private CompilationUnitSyntax BuildRootNode()
        {
            this.RequiredUsings = this.BuildRequiredUsings();

            return CompilationUnit().WithUsings(List(this.RequiredUsings))
                                    .WithMembers(SingletonList(this.BuildNamespace()));
        }

        private IReadOnlyList<UsingDirectiveSyntax> BuildRequiredUsings()
        {
            return this.RestClients.SelectMany(x => x.InterfaceDeclaration.GetRequiredUsings())
                                   .Distinct(UsingDirectiveSyntaxEqualityComparer.Default)
                                   .OrderBy(x => x, UsingDirectiveSyntaxComparer.Default)
                                   .ToArray();
        }

        private MemberDeclarationSyntax BuildNamespace()
        {
            return Constants.DoLessRestGeneratedNamespace
                            .WithMembers(List(this.BuildRestClients()));
        }

        private IEnumerable<MemberDeclarationSyntax> BuildRestClients()
        {
            return this.RestClients.Select(x => this.BuildRestClient(x));
        }

        private ClassDeclarationSyntax BuildRestClient(RestClientInfo restClient)
        {
            var interfaceDeclaration = restClient.InterfaceDeclaration;
            this.interfaceRequestInfo = new RequestInfo(interfaceDeclaration);

            var classDeclaration = ClassDeclaration(restClient.ClassName)
                                  .AddModifiers(Token(SyntaxKind.InternalKeyword))
                                  .AddModifiers(Token(SyntaxKind.SealedKeyword))
                                  .WithTypeParameterList(interfaceDeclaration.TypeParameterList)
                                  .WithConstraintClauses(interfaceDeclaration.ConstraintClauses)
                                  .AddMembers(this.ImplementMethods(interfaceDeclaration.Members))
                                  .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(new[]
                                  {
                                          SimpleBaseType(IdentifierName(nameof(RestClientBase))),
                                          SimpleBaseType(interfaceDeclaration.GetTypeSyntax())
                                  })));

            return classDeclaration;
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
            this.methodRequestInfo = this.interfaceRequestInfo.WithMethod(node);

            return Block(ReturnStatement(this.ImplementRequest()));
        }

        private ExpressionSyntax ImplementRequest()
        {
            InvocationExpressionSyntax result = NewMethodInvocation(nameof(RestRequest), this.methodRequestInfo.HttpMethod)
                                               .WithArgs(Argument(ThisExpression()));

            result = this.ChainWithRequestUrlBuilding(result);
            result = this.ChainWithHeaders(result);
            result = this.ChainWithBody(result);
            result = this.ChainWithSendMethod(result);

            return result;
        }

        private InvocationExpressionSyntax ChainWithRequestUrlBuilding(InvocationExpressionSyntax invocationExpression)
        {
            //Add the BaseUrl if any.
            if (!string.IsNullOrEmpty(this.methodRequestInfo.BaseUrl))
            {
                invocationExpression = invocationExpression.ChainWith(nameof(IRestRequest.WithBaseUrl))
                                                           .WithArgs(this.methodRequestInfo.BaseUrl.ToArgLiteral());
            }
            invocationExpression = invocationExpression.ChainWith(nameof(IRestRequest.WithUriTemplate))
                                                       .WithArgs(this.methodRequestInfo.UriTemplate.ToArgLiteral());

            invocationExpression = this.ChainWithUriParameters(invocationExpression);

            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithParameters(InvocationExpressionSyntax invocationExpression, IReadOnlyDictionary<string, Parameter> parameters, string methodName)
        {
            var headers = this.methodRequestInfo.Headers;
            if (parameters.Count > 0)
            {
                parameters.ForEach(x =>
                {
                    invocationExpression = invocationExpression.ChainWith(methodName)
                                                               .WithArgs(x.Key.ToArgLiteral(), x.Value.ToArg());
                });
            }

            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithHeaders(InvocationExpressionSyntax invocationExpression)
        {
            return this.ChainWithParameters(invocationExpression, this.methodRequestInfo.Headers, nameof(IRestRequest.WithHeader));
        }

        private InvocationExpressionSyntax ChainWithUriParameters(InvocationExpressionSyntax invocationExpression)
        {
            return this.ChainWithParameters(invocationExpression, this.methodRequestInfo.UriVariables, nameof(IRestRequest.WithParameter));
        }

        private InvocationExpressionSyntax ChainWithBody(InvocationExpressionSyntax invocationExpression)
        {
            var bodyIdentifier = this.methodRequestInfo.BodyIdentifier;
            if (bodyIdentifier.HasContent())
            {
                string methodName = this.methodRequestInfo.IsBodyFormUrlEncoded ?
                                    nameof(IRestRequest.WithFormUrlEncodedBody) :
                                    nameof(IRestRequest.WithBody);

                invocationExpression = invocationExpression.ChainWith(methodName)
                                                           .WithArgs(bodyIdentifier.ToArg());
            }

            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithSendMethod(InvocationExpressionSyntax invocationExpression)
        {
            TypeSyntax returnType = this.methodRequestInfo.MethodDeclaration.ReturnType;
            TypeSyntax genericType = (returnType as GenericNameSyntax)?.TypeArgumentList?.Arguments.FirstOrDefault();

            invocationExpression = this.ChainWithSendMethod(invocationExpression, genericType);

            string cancellationTokenParameterName = this.methodRequestInfo
                                                        .MethodDeclaration
                                                        .ParameterList?
                                                        .Parameters
                                                        .FirstOrDefault(x => x.Type.GetTypeName() == nameof(CancellationToken))?
                                                        .Identifier
                                                        .ValueText;

            if (cancellationTokenParameterName.HasContent())
            {
                return invocationExpression.WithArgs(cancellationTokenParameterName.ToArg());
            }

            return invocationExpression;
        }

        private InvocationExpressionSyntax ChainWithSendMethod(InvocationExpressionSyntax invocationExpression, TypeSyntax returnType)
        {
            switch (returnType)
            {
                case null:
                    // Task.
                    return invocationExpression.ChainWith(nameof(IRestRequest.SendAsync));

                case ArrayTypeSyntax type01 when type01.ElementType.GetTypeName() == nameof(Byte):
                case ArrayTypeSyntax type02 when type02.ElementType is PredefinedTypeSyntax elementType &&
                                                 elementType.Keyword.IsKind(SyntaxKind.ByteKeyword):
                    // Task<byte[]>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.ReadAsByteArrayAsync));

                case PredefinedTypeSyntax predefinedType when predefinedType.Keyword.IsKind(SyntaxKind.StringKeyword):
                case var simpleType when simpleType.GetTypeName() == nameof(String):
                    // Task<string>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.ReadAsStringAsync));

                case var type when type.GetTypeName() == nameof(Stream):
                    // Task<Stream>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.ReadAsStreamAsync));

                case PredefinedTypeSyntax predefinedType when predefinedType.Keyword.IsKind(SyntaxKind.BoolKeyword):
                case var simpleType when simpleType.GetTypeName() == nameof(Boolean):
                    // Task<bool>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.SendAndGetSuccessAsync));

                case var simpleType when simpleType.GetTypeName() == nameof(HttpResponseMessage):
                    // Task<HttpResponseMessage>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.ReadAsHttpResponseMessageAsync));

                default:
                    // Task<T>.
                    return invocationExpression.ChainWith(nameof(IRestRequest.ReadAsObject), returnType);
            }
        }
    }
}
