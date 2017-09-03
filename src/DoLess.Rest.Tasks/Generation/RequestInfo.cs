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

        private readonly Dictionary<string, Parameter> headers;
        private readonly Dictionary<string, string> queries;

        private MethodDeclarationSyntax methodDeclaration;
        private ParameterSyntax parameter;
        private string urlId;
        private bool hasTaskUsingNamespace;

        public RequestInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.headers = new Dictionary<string, Parameter>();

            this.ParseInterfaceDeclaration(interfaceDeclaration);
        }

        private RequestInfo(RequestInfo requestInfo)
        {
            this.headers = new Dictionary<string, Parameter>(requestInfo.headers);
            this.BaseUrl = requestInfo.BaseUrl;
            this.queries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.hasTaskUsingNamespace = requestInfo.hasTaskUsingNamespace;
        }

        public string BaseUrl { get; private set; }

        public string HttpMethod { get; private set; }

        public StringTemplate StringTemplate { get; private set; }

        public IReadOnlyDictionary<string, Parameter> Headers => this.headers;

        public string BodyIdentifier { get; private set; }

        public IReadOnlyDictionary<string, string> Queries => this.queries;

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
            this.ThrowIfInvalidBaseUrl(interfaceDeclaration);
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
                            this.SetParameterValue(this.urlId, parameterName);
                            this.urlId = null;
                        }
                    }
                    else
                    {
                        this.SetParameterValue(parameterName, parameterName);
                    }
                }

                this.ThrowIfUrlIdNotFound();
            }
        }

        private void SetParameterValue(string urlId, string value)
        {
            Parameter parameter = this.StringTemplate.GetParameter(urlId);
            if (parameter != null)
            {
                // Ensure the parameter has not already been set.
                this.ThrowIfUrlIdAlreadyExists(parameter, urlId);
                parameter.Value = value;
            }
            else
            {
                if (this.queries.ContainsKey(urlId))
                {
                    this.ThrowUrlIdAlreadyExists(urlId);
                }
                this.queries[urlId] = value;
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
            string name = attribute.GetArgument(0);
            Parameter value = attribute.AttachedParameterName?.ToMutable() ??
                              attribute.GetArgument(1)?.ToImmutable();

            this.headers[name] = value;
        }

        private void ParseBaseUrlAttribute(RequestAttribute attribute)
        {
            this.BaseUrl = attribute.GetArgument(0);
        }

        private void ParseBodyAttribute(RequestAttribute attribute)
        {
            this.BodyIdentifier = attribute.AttachedParameterName;
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
                this.StringTemplate = StringTemplate.Parse(attribute.GetArgument(0));
            }
            catch (Exception ex)
            {
                throw new InvalidUrlTemplateError(this.methodDeclaration).ToException(ex);
            }
        }

        private void ThrowIfUrlIdAlreadyExists(Parameter parameter, string urlId)
        {
            if (parameter.HasBeenSet)
            {
                throw new UrlIdAlreadyExistsError(urlId, this.parameter).ToException();
            }
        }

        private void ThrowUrlIdAlreadyExists(string urlId)
        {
            throw new UrlIdAlreadyExistsError(urlId, this.parameter).ToException();
        }

        private void ThrowIfUrlIdNotFound()
        {
            var notFoundIds = this.StringTemplate
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

        private void ThrowIfInvalidBaseUrl(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (this.BaseUrl != null)
            {
                var baseUrl = this.BaseUrl;

                // The Uri.IsWellFormedUriString does not check if there is query string in a relative uri.
                bool isWellFormed = !baseUrl.Contains('?') && Uri.IsWellFormedUriString(baseUrl, UriKind.Relative);

                if (!isWellFormed)
                {
                    throw new InvalidBaseUrlError(interfaceDeclaration).ToException();
                }
            }
        }
    }
}
