using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Diagnostics;
using DoLess.Rest.Tasks.Exceptions;
using DoLess.Rest.Tasks.Helpers;
using DoLess.Rest.Tasks.UrlTemplating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks
{
    internal class RequestInfo
    {
        private const string TaskNamespace = "System.Threading.Tasks";
        private const string TaskName = "Task";

        private static readonly IEqualityComparer<Header> HeaderEqualityComparer = new HeaderEqualityComparer();

        private readonly HashSet<Header> commonHeaders;
        private readonly HashSet<Header> methodHeaders;
        private readonly HashSet<string> additionalQueryParameters;

        private MethodDeclarationSyntax methodDeclaration;
        private ParameterSyntax parameter;
        private string urlId;
        private bool hasTaskUsingNamespace;

        public RequestInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.commonHeaders = new HashSet<Header>(HeaderEqualityComparer);
            this.methodHeaders = new HashSet<Header>(HeaderEqualityComparer);

            this.ParseInterfaceDeclaration(interfaceDeclaration);
        }

        private RequestInfo(RequestInfo requestInfo)
        {
            this.commonHeaders = requestInfo.commonHeaders;
            this.methodHeaders = new HashSet<Header>(this.commonHeaders, HeaderEqualityComparer);
            this.BaseUrl = requestInfo.BaseUrl;
            this.additionalQueryParameters = new HashSet<string>();
            this.hasTaskUsingNamespace = requestInfo.hasTaskUsingNamespace;
        }

        public string BaseUrl { get; private set; }

        public string HttpMethod { get; private set; }

        public UrlTemplate UrlTemplate { get; private set; }

        public IReadOnlyCollection<Header> Headers => this.methodHeaders;

        public Arg Body { get; private set; }

        public IReadOnlyCollection<string> AdditionalQueryParameters => this.additionalQueryParameters;

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
            this.methodHeaders.ForEach(x => this.commonHeaders.Add(x));
        }

        private void ParseMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            this.methodDeclaration = methodDeclaration;

            this.ThrowIfReturnTypeIsNotAccepted();

            var requestAttributes = methodDeclaration.AttributeLists
                                                     .ToRequesAttributes();

            this.ThrowIfMethodDoesNotContainsSingleHttpAttribute(requestAttributes);

            requestAttributes.ForEach(x => this.ParseRequestAttribute(x));

            if (methodDeclaration.ParameterList != null)
            {
                foreach (var parameter in methodDeclaration.ParameterList.Parameters)
                {
                    this.parameter = parameter;
                    string parameterName = parameter.Identifier.Text;


                    // Only one request attribute on parameter.
                    var firstRequestAttribute = parameter.AttributeLists
                                                         .ToRequesAttributes()
                                                         .ZeroOrSingle(() => new MultipleRestAttributesError(parameter).ToException());

                    if (firstRequestAttribute != null)
                    {
                        this.ParseRequestAttribute(firstRequestAttribute);
                        if (this.urlId != null)
                        {
                            this.SetUrlParameterValue(this.urlId, parameterName);
                            this.urlId = null;
                        }
                    }
                    else
                    {
                        this.SetUrlParameterValue(parameterName, parameterName);
                    }
                }

                this.ThrowIfUrlIdNotFound();
            }
        }

        private void SetUrlParameterValue(string urlId, string value)
        {
            UrlParameter urlParameter = this.UrlTemplate.GetParameter(urlId);
            if (urlParameter != null)
            {
                // Ensure the parameter has not already been set.
                this.ThrowIfUrlIdAlreadyExists(urlParameter, urlId);
                urlParameter.Value = value;
            }
            else
            {
                this.additionalQueryParameters.Add(urlId);
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
                case RequestAttributeType.UrlId:
                    this.ParseUrlIdAttribute(attribute);
                    break;
                case RequestAttributeType.Body:
                    this.ParseBodyAttribute(attribute);
                    break;
                case RequestAttributeType.Header:
                    this.ParseHeaderAttribute(attribute);
                    break;
                case RequestAttributeType.BaseUrl:
                    this.ParseBaseUrlAttribute(attribute);
                    break;
                default:
                    break;
            }
        }

        private void ParseHeaderAttribute(RequestAttribute attribute)
        {
            this.methodHeaders.Add(new Header(attribute));
        }

        private void ParseBaseUrlAttribute(RequestAttribute attribute)
        {
            this.BaseUrl = attribute.GetArgument(0);
        }

        private void ParseBodyAttribute(RequestAttribute attribute)
        {
            this.Body = Arg.Identifier(attribute.AttachedParameterName);
        }

        private void ParseUrlIdAttribute(RequestAttribute attribute)
        {
            this.urlId = attribute.GetArgument(0);
        }

        private void ParseHttpMethodAttribute(RequestAttribute attribute)
        {
            this.HttpMethod = attribute.ClassName;

            try
            {
                this.UrlTemplate = UrlTemplate.Parse(attribute.GetArgument(0));
            }
            catch (Exception ex)
            {
                throw new MalformedUrlTemplateError(this.methodDeclaration).ToException(ex);
            }
        }

        private void ThrowIfUrlIdAlreadyExists(UrlParameter urlParameter, string urlId)
        {
            if (urlParameter.HasBeenSet)
            {
                throw new UrlIdAlreadyExistsError(urlId, this.parameter).ToException();
            }
        }

        private void ThrowIfUrlIdNotFound()
        {
            var notFoundIds = this.UrlTemplate
                                  .Parameters
                                  .Where(x => x.IsMutable && !x.HasBeenSet)
                                  .Select(x => x.Value)
                                  .ToList();

            if (notFoundIds.Count > 0)
            {
                throw new UrlIdNotFoundError(notFoundIds, this.methodDeclaration).ToException();
            }
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
