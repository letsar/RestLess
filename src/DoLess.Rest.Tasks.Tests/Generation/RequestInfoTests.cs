using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.RestInterfaces;
using DoLess.Rest.Tasks.Diagnostics;
using DoLess.Rest.Tasks.Exceptions;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.Generation
{
    [TestFixture]
    public class RequestInfoTests
    {

        [TestCase(nameof(IRestApi00.UrlIdNotFound01), "id")]
        [TestCase(nameof(IRestApi00.UrlIdNotFound02), "id")]
        [TestCase(nameof(IRestApi00.UrlIdNotFound03), "id")]
        [TestCase(nameof(IRestApi00.UrlIdNotFound04), "id", "foo")]
        [TestCase(nameof(IRestApi00.UrlIdNotFound05), "id", "foo", "end")]
        public void ShouldThrowUrlIdNotFound(string methodName, params string[] notFoundIds)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<UrlIdNotFoundError>()
               .Which
               .UrlIds
               .Should()
               .BeEquivalentTo(notFoundIds);
        }

        [TestCase(nameof(IRestApi00.UrlIdAlreadyExists01), "name")]
        [TestCase(nameof(IRestApi00.UrlIdAlreadyExists02), "name")]
        [TestCase(nameof(IRestApi00.UrlIdAlreadyExists03), "name")]
        [TestCase(nameof(IRestApi00.UrlIdAlreadyExists04), "name")]
        public void ShouldThrowUrlIdAlreadyExists(string methodName, params string[] notFoundIds)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<UrlIdAlreadyExistsError>()
               .Which
               .UrlId
               .Should()
               .BeEquivalentTo(notFoundIds[0]);
        }

        [TestCase(nameof(IRestApi00.MultipleRestAttributes01))]
        public void ShouldThrowMultipleRestAttributes(string methodName)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<MultipleRestAttributesError>();
        }

        [TestCase(nameof(IRestApi00.MissingHttpAttribute01))]
        [TestCase(nameof(IRestApi00.MissingHttpAttribute02))]
        [TestCase(nameof(IRestApi00.MissingHttpAttribute03))]
        public void ShouldThrowMissingHttpAttributes(string methodName)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<MissingHttpAttributeError>();
        }

        [TestCase(nameof(IRestApi00.MultipleHttpAttribute01))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute02))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute03))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute04))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute05))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute06))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute07))]
        [TestCase(nameof(IRestApi00.MultipleHttpAttribute08))]
        public void ShouldThrowMultipleHttpAttributes(string methodName)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<MultipleHttpAttributesError>();
        }

        [TestCase(nameof(IRestApi00.MalformedUrlTemplate01))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate02))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate03))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate04))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate05))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate06))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate07))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate08))]
        [TestCase(nameof(IRestApi00.MalformedUrlTemplate09))]
        public void ShouldThrowMalformedUrlTemplate(string methodName)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<MalformedUrlTemplateError>();
        }

        [TestCase(nameof(IRestApi00.ReturnType01))]
        [TestCase(nameof(IRestApi00.ReturnType02))]
        public void ShouldThrowReturnType(string methodName)
        {
            Action job = () => GetRequestInfo<IRestApi00>(methodName);
            job.ShouldThrowExactly<ErrorDiagnosticException>()
               .And
               .Error
               .Should()
               .BeOfType<ReturnTypeError>();
        }

        [Test]
        public void GetSomeStuffWithHeaderTest01()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.GetSomeStuffWithHeader01));

            requestInfo.UrlTemplate
                       .ParameterNames
                       .Should()
                       .BeEquivalentTo("id");
            requestInfo.Headers
                       .Should()
                       .HaveCount(1);
            requestInfo.Headers
                       .First()
                       .Value
                       .Value
                       .Should()
                       .Be("Interface");                       
        }

        [Test]
        public void GetSomeStuffWithHeaderTest02()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.GetSomeStuffWithHeader02));

            requestInfo.UrlTemplate
                       .ParameterNames
                       .Should()
                       .BeEquivalentTo("id");
            requestInfo.Headers
                       .Should()
                       .HaveCount(1);
            requestInfo.Headers
                       .First()
                       .Value
                       .Value
                       .Should()
                       .Be("Method");
        }

        [Test]
        public void GetSomeStuffWithHeaderTest03()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.GetSomeStuffWithHeader03));

            requestInfo.UrlTemplate
                       .ParameterNames
                       .Should()
                       .BeEquivalentTo("id");
            requestInfo.Headers
                       .Should()
                       .HaveCount(1);
            requestInfo.Headers
                       .First()
                       .Value
                       .Value
                       .Should()
                       .Be("scope");
        }

        [Test]
        public void PostSomeStuffWithoutBodyTest()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.PostSomeStuffWithoutBody));

            requestInfo.BodyIdentifier
                       .Should()
                       .BeNull();                       
        }

        [Test]
        public void PostSomeStuffWithBodyTest()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.PostSomeStuffWithBody));

            requestInfo.BodyIdentifier
                       .Should()
                       .Be("body");
        }


        [TestCase(nameof(IRestApi00.DeleteSomeStuff),"Delete")]
        [TestCase(nameof(IRestApi00.GetSomeStuff), "Get")]
        [TestCase(nameof(IRestApi00.HeadSomeStuff), "Head")]
        [TestCase(nameof(IRestApi00.OptionsSomeStuff), "Options")]
        [TestCase(nameof(IRestApi00.PatchSomeStuff), "Patch")]
        [TestCase(nameof(IRestApi00.PostSomeStuff), "Post")]
        [TestCase(nameof(IRestApi00.PutSomeStuff), "Put")]
        [TestCase(nameof(IRestApi00.TraceSomeStuff), "Trace")]
        public void ShouldBeRightHttpMethod(string method, string httpMethod)
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(method);

            requestInfo.HttpMethod
                       .Should()
                       .Be(httpMethod);
        }

        [TestCase(nameof(IRestApi00.DeleteSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.GetSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.HeadSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.OptionsSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.PatchSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.PostSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.PutSomeStuff), "/api")]
        [TestCase(nameof(IRestApi00.TraceSomeStuff), "/api")]
        public void ShouldHaveBaseUrl(string method, string baseUrl)
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(method);

            requestInfo.BaseUrl
                       .Should()
                       .Be(baseUrl);
        }

        private static RequestInfo GetRequestInfo<IRestApi>(string methodName)
        {
            var interfaceFileName = $"{typeof(IRestApi).Name}.cs";
            var filePath = Path.Combine(Constants.InterfacesFolder, interfaceFileName);
            var fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            var interfaceDeclaration = CSharpSyntaxTree.ParseText(fileContent)
                                                       .GetRoot()
                                                       .DescendantNodes()
                                                       .OfType<InterfaceDeclarationSyntax>()
                                                       .FirstOrDefault();

            if (interfaceDeclaration != null)
            {
                RequestInfo requestInfo = new RequestInfo(interfaceDeclaration);

                var methodDeclaration = interfaceDeclaration.DescendantNodes()
                                                            .OfType<MethodDeclarationSyntax>()
                                                            .Where(x => x.Identifier.Text == methodName)
                                                            .FirstOrDefault();

                return requestInfo.WithMethod(methodDeclaration);
            }

            return null;
        }
    }
}
