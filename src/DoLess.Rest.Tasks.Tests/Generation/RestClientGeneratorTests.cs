using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Tests.RestInterfaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.Generation
{
    [TestFixture]
    public class RestClientGeneratorTests
    {
        [Test]
        public void ShouldInterfaceHaveTypeParameter()
        {
            SyntaxNode root = GetRootNode("IRestApi03");
            SyntaxNode newRoot = RestClientGenerator.Generate(root);
        }

        private static SyntaxNode GetRootNode(string restApiName)
        {
            var interfaceFileName = $"{restApiName}.cs";
            var filePath = Path.Combine(Constants.InterfacesFolder, interfaceFileName);
            var fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            var interfaceDeclaration = CSharpSyntaxTree.ParseText(fileContent)
                                                       .GetRoot()
                                                       .DescendantNodes()
                                                       .OfType<InterfaceDeclarationSyntax>()
                                                       .FirstOrDefault();

            return interfaceDeclaration;
        }
    }
}
