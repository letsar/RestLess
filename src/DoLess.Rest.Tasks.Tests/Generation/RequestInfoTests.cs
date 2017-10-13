using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.RestInterfaces;
using DoLess.Rest.Tasks.Diagnostics;
using DoLess.Rest.Tasks.Entities;
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

            requestInfo.WithHeaderArguments
                       .Should()
                       .HaveCount(1);
        }

        [Test]
        public void GetSomeStuffWithHeaderTest02()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.GetSomeStuffWithHeader02));

            requestInfo.WithHeaderArguments
                       .Should()
                       .HaveCount(2);
        }

        [Test]
        public void GetSomeStuffWithHeaderTest03()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.GetSomeStuffWithHeader03));
                     
            requestInfo.WithHeaderArguments
                       .Should()
                       .HaveCount(3);            
        }

        [Test]
        public void PostSomeStuffWithoutBodyTest()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.PostSomeStuffWithoutBody));

            requestInfo.WithContentArguments
                       .Should()
                       .BeEmpty();
        }

        [Test]
        public void PostSomeStuffWithBodyTest()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.PostSomeStuffWithBody));

            requestInfo.WithContentArguments
                       .FirstOrDefault()
                       .Arguments[1]                       
                       .Expression
                       .As<LiteralExpressionSyntax>()
                       .Token
                       .ValueText
                       .Should()
                       .Be("body");
        }


        [TestCase(nameof(IRestApi00.DeleteSomeStuff), "Delete")]
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

            requestInfo.UriTemplatePrefix
                       .Expression
                       .As<LiteralExpressionSyntax>()
                       .Token
                       .ValueText
                       .Should()
                       .Be(baseUrl);
        }        

        [TestCase(nameof(IRestApiForTestingBaseUrl01))]
        [TestCase(nameof(IRestApiForTestingBaseUrl04))]
        [TestCase(nameof(IRestApiForTestingBaseUrl05))]
        public void ShouldNotThrowInvalidBaseUrl(string interfaceName)
        {
            Action job = () => new RequestInfo(GetInterfaceDeclaration(interfaceName, "IRestApi04.cs"));

            job.ShouldNotThrow<ErrorDiagnosticException>();
        }

        [Test]
        public void ShouldHaveFormUrlEncodedContent()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.FormUrlEncodedContent));

            requestInfo.WithContentArguments
                       .Should()
                       .HaveCount(1);

            requestInfo.WithContentArguments
                       .OfType<FormUlrEncodedContentArgument>()
                       .Should()
                       .HaveCount(1);
        }

        [Test]
        public void ShouldHaveMultipartContent01()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.MultipartContent01));

            requestInfo.WithContentArguments
                       .Should()
                       .HaveCount(1);
        }

        [Test]
        public void ShouldHaveMultipartContent02()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.MultipartContent02));

            requestInfo.WithContentArguments
                       .Should()
                       .HaveCount(2);

            requestInfo.WithContentArguments[1]
                       .Arguments
                       .Should().HaveCount(4);

            requestInfo.WithContentArguments[1]
                       .Arguments[2]
                       .Expression
                       .As<LiteralExpressionSyntax>()
                       .Token
                       .ValueText
                       .ShouldBeEquivalentTo("fileName");

            requestInfo.WithContentArguments[1]
                       .Arguments[3]
                       .Expression
                       .As<LiteralExpressionSyntax>()
                       .Token
                       .ValueText
                       .ShouldBeEquivalentTo("text/plain");
        }

        [Test]
        public void ShouldHaveMultipartContent03()
        {
            RequestInfo requestInfo = GetRequestInfo<IRestApi00>(nameof(IRestApi00.MultipartContent03));

            requestInfo.WithContentArguments
                       .Should()
                       .HaveCount(2);

            requestInfo.WithContentArguments[1]
                       .Arguments
                       .Should().HaveCount(2);

            requestInfo.WithContentArguments[1]
                       .Arguments[1]
                       .Expression
                       .As<LiteralExpressionSyntax>()
                       .Token
                       .ValueText
                       .ShouldBeEquivalentTo("newContent");
        }

        private static RequestInfo GetRequestInfo<IRestApi>(string methodName, string fileName = null)
        {
            var interfaceDeclaration = GetInterfaceDeclaration(typeof(IRestApi).Name, fileName);

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

        private static InterfaceDeclarationSyntax GetInterfaceDeclaration(string interfaceName, string fileName = null)
        {
            if (fileName == null)
            {
                fileName = $"{interfaceName}.cs";
            }
            var filePath = Path.Combine(Constants.InterfacesFolder, fileName);
            var fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            var interfaceDeclaration = CSharpSyntaxTree.ParseText(fileContent)
                                                       .GetRoot()
                                                       .DescendantNodes()
                                                       .OfType<InterfaceDeclarationSyntax>()
                                                       .FirstOrDefault(x => x.Identifier.Text == interfaceName);

            return interfaceDeclaration;
        }
    }
}
