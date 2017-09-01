using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Helpers;
using DoLess.Rest.Tasks.UrlTemplating;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks
{
    internal class RequestInfo
    {
        private static readonly IEqualityComparer<Header> HeaderEqualityComparer = new HeaderEqualityComparer();

        private readonly HashSet<Header> commonHeaders;
        private readonly HashSet<Header> methodHeaders;

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
        }

        public string BaseUrl { get; private set; }

        public string HttpMethod { get; private set; }

        public UrlTemplate UrlTemplate { get; private set; }

        public IReadOnlyCollection<Header> Headers => this.methodHeaders;

        public Arg Body { get; private set; }

        public RequestInfo WithMethod(MethodDeclarationSyntax methodDeclaration)
        {
            var newRequestInfo = new RequestInfo(this);
            newRequestInfo.ParseMethodDeclaration(methodDeclaration);
            return newRequestInfo;
        }

        private void ParseInterfaceDeclaration(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.ParseAttributeLists(interfaceDeclaration.AttributeLists);
            this.methodHeaders.ForEach(x => this.commonHeaders.Add(x));
        }

        private void ParseMethodDeclaration(MethodDeclarationSyntax methodDeclaration)
        {
            this.ParseAttributeLists(methodDeclaration.AttributeLists);
            if (methodDeclaration.ParameterList != null)
            {
                foreach (var parameter in methodDeclaration.ParameterList.Parameters)
                {
                    // Take the first one.

                    var firstRequestAttribute = parameter.AttributeLists
                                                         .ToRequesAttributes()
                                                         .FirstOrDefault();

                    if (firstRequestAttribute != null)
                    {
                        this.ParseRequestAttribute(firstRequestAttribute);
                    }
                    else
                    {
                        
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
                case RequestAttributeType.UrlParameter:
                    this.ParseUrlParameterAttribue(attribute);
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

        private void ParseUrlParameterAttribue(RequestAttribute attribute)
        {
            // TODO : add to urltemplate ?
        }

        private void ParseHttpMethodAttribute(RequestAttribute attribute)
        {
            this.HttpMethod = attribute.ClassName;
            this.UrlTemplate = UrlTemplate.Parse(attribute.GetArgument(0));
        }

    }
}
