using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DoLess.Rest.Tasks.Diagnostics;
using DoLess.Rest.Tasks.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks
{
    internal class RequestInfo
    {
        private const string TaskNamespace = "System.Threading.Tasks";
        private const string TaskName = "Task";

        private readonly List<ArgumentSyntax[]> withHeaderArguments;
        private readonly List<ArgumentSyntax[]> withUriVariableArguments;
        private readonly List<ContentArgument> withContentArguments;

        private MethodDeclarationSyntax methodDeclaration;
        private ParameterSyntax parameter;
        private ArgumentSyntax parameterNewName;
        private bool hasTaskUsingNamespace;

        public RequestInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.withHeaderArguments = new List<ArgumentSyntax[]>();
            this.ParseInterfaceDeclaration(interfaceDeclaration);
        }

        private RequestInfo(RequestInfo requestInfo)
        {
            this.withHeaderArguments = new List<ArgumentSyntax[]>(requestInfo.withHeaderArguments);
            this.withUriVariableArguments = new List<ArgumentSyntax[]>();
            this.withContentArguments = new List<ContentArgument>();

            this.UriTemplatePrefix = requestInfo.UriTemplatePrefix;
            this.UriTemplateSuffix = requestInfo.UriTemplateSuffix;
            this.hasTaskUsingNamespace = requestInfo.hasTaskUsingNamespace;
        }

        public ArgumentSyntax UriTemplatePrefix { get; private set; }

        public ArgumentSyntax UriTemplateSuffix { get; private set; }

        public string HttpMethod { get; private set; }

        public ArgumentSyntax UriTemplate { get; set; }

        public IReadOnlyList<ArgumentSyntax[]> WithHeaderArguments => this.withHeaderArguments;

        public IReadOnlyList<ArgumentSyntax[]> WithUriVariableArguments => this.withUriVariableArguments;

        public IReadOnlyList<ContentArgument> WithContentArguments => this.withContentArguments;

        public MethodDeclarationSyntax MethodDeclaration => this.methodDeclaration;

        public RequestInfo WithMethod(MethodDeclarationSyntax methodDeclaration)
        {
            var newRequestInfo = new RequestInfo(this);
            newRequestInfo.ParseMethodDeclaration(methodDeclaration);
            return newRequestInfo;
        }

        private void ParseInterfaceDeclaration(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.hasTaskUsingNamespace = interfaceDeclaration.HasUsingDirective(TaskNamespace);
            this.ParseAttributeLists(interfaceDeclaration.AttributeLists);
        }

        private void ParseMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            this.methodDeclaration = methodDeclaration;

            this.ThrowIfReturnTypeIsNotAccepted();

            var methodAttributes = methodDeclaration.AttributeLists
                                                    .ToRequesAttributes();

            this.ThrowIfMethodDoesNotContainsSingleHttpAttribute(methodAttributes);

            methodAttributes.ForEach(x => this.ParseRequestAttribute(x));

            if (methodDeclaration.ParameterList != null)
            {
                foreach (var parameter in methodDeclaration.ParameterList.Parameters)
                {
                    this.parameter = parameter;
                    string parameterName = parameter.Identifier.Text;

                    var parameterAttributes = parameter.AttributeLists
                                                       .ToRequesAttributes();

                    // Gets the new name attribute.
                    var parameterNewNameAttribute = parameterAttributes.FirstOrDefault(x => x.Type == RequestAttributeType.Name);
                    if (parameterNewNameAttribute != null)
                    {
                        this.ParseRequestAttribute(parameterNewNameAttribute);
                    }

                    // Only one request attribute on parameter.
                    var firstRequestAttribute = parameterAttributes.Where(x => x.Type != RequestAttributeType.Name)
                                                                   .ZeroOrSingle(() => new MultipleRestAttributesError(parameter).ToException());

                    if (firstRequestAttribute != null)
                    {
                        this.ParseRequestAttribute(firstRequestAttribute);
                    }
                    else if (this.parameterNewName != null)
                    {
                        this.withUriVariableArguments.Add(new[]
                        {
                                this.parameterNewName,
                                parameterName.ToArg()
                            });
                        this.parameterNewName = null;
                    }
                    else if (parameter.Type.GetTypeName() != nameof(CancellationToken))
                    {
                        this.withUriVariableArguments.Add(new[]
                        {
                            parameterName.ToArgLiteral(),
                            parameterName.ToArg()
                        });
                    }
                }
            }
        }

        private void ParseAttributeLists(SyntaxList<AttributeListSyntax> attributes)
        {
            attributes.ToRequesAttributes()
                      .ForEach(x => this.ParseRequestAttribute(x));
        }

        private void ParseRequestAttribute(RequestAttribute attribute)
        {
            switch (attribute.Type)
            {
                case RequestAttributeType.HttpMethod:
                    this.ParseHttpMethodAttribute(attribute);
                    break;
                case RequestAttributeType.Name:
                    this.ParseNameAttribute(attribute);
                    break;
                case RequestAttributeType.Content:
                    this.ParseContentAttribute(attribute);
                    break;
                case RequestAttributeType.FormUrlEncodedContent:
                    this.ParseFormUrlEncodedContentAttribute(attribute);
                    break;
                case RequestAttributeType.Header:
                    this.ParseHeaderAttribute(attribute);
                    break;
                case RequestAttributeType.HeaderValue:
                    this.ParseHeaderValueAttribute(attribute);
                    break;
                case RequestAttributeType.UriTemplatePrefix:
                    this.ParseUriTemplatePrefixAttribute(attribute);
                    break;
                case RequestAttributeType.UriTemplateSuffix:
                    this.ParseUriTemplateSuffixAttribute(attribute);
                    break;
                default:
                    break;
            }
        }

        private void ParseHeaderAttribute(RequestAttribute attribute)
        {
            this.withHeaderArguments.Add(attribute.Arguments);
        }

        private void ParseHeaderValueAttribute(RequestAttribute attribute)
        {
            this.withHeaderArguments.Add(new[]
            {
                attribute.GetArgument(0),
                attribute.AttachedParameterName.ToArg()
            });
        }

        private void ParseUriTemplatePrefixAttribute(RequestAttribute attribute)
        {
            this.UriTemplatePrefix = attribute.GetArgument(0);
        }

        private void ParseUriTemplateSuffixAttribute(RequestAttribute attribute)
        {
            this.UriTemplateSuffix = attribute.GetArgument(0);
        }

        private void ParseContentAttribute(RequestAttribute attribute)
        {
            List<ArgumentSyntax> arguments = new List<ArgumentSyntax>(attribute.ArgumentCount + 2)
            {
                attribute.AttachedParameterName.ToArg(),
                this.parameterNewName ?? attribute.AttachedParameterName.ToArgLiteral()
            };

            if (attribute.ArgumentCount > 0)
            {
                arguments.AddRange(attribute.Arguments);
            }

            this.withContentArguments.Add(new ContentArgument(arguments.ToArray()));
        }

        private void ParseFormUrlEncodedContentAttribute(RequestAttribute attribute)
        {
            this.withContentArguments.Add(new FormUlrEncodedContentArgument(attribute.AttachedParameterName.ToArg()));
        }

        private void ParseNameAttribute(RequestAttribute attribute)
        {
            this.parameterNewName = attribute.GetArgument(0);
        }

        private void ParseHttpMethodAttribute(RequestAttribute attribute)
        {
            this.HttpMethod = attribute.ClassName;
            this.UriTemplate = attribute.GetArgument(0);
        }

        private void ThrowIfMethodDoesNotContainsSingleHttpAttribute(IEnumerable<RequestAttribute> requestAttributes)
        {
            var httpAttributesCount = requestAttributes.Where(x => x.Type == RequestAttributeType.HttpMethod)
                                                       .Count();

            switch (httpAttributesCount)
            {
                case 0:
                    throw new MissingHttpAttributeError(this.methodDeclaration).ToException();
                case 1:
                    return;
                default:
                    throw new MultipleHttpAttributesError(this.methodDeclaration).ToException();
            }
        }

        private void ThrowIfReturnTypeIsNotAccepted()
        {
            // Very simple type check.
            // TODO: test with semantic model and see if it is time expansive.
            bool isTask = false;
            switch (this.methodDeclaration.ReturnType)
            {
                case SimpleNameSyntax node:
                    isTask = node.Identifier.Text == TaskName && this.hasTaskUsingNamespace;
                    break;
                case QualifiedNameSyntax node:
                    isTask = node.Right.Identifier.Text == TaskName && node.Left.ToString() == TaskNamespace;
                    break;
                default:
                    isTask = false;
                    break;
            }

            if (!isTask)
            {
                throw new ReturnTypeError(this.methodDeclaration).ToException();
            }
        }
    }
}
