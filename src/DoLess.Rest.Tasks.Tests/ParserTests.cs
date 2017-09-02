using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.CodeParsers;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private const string ProjectFile = "../../../DoLess.Rest.Tasks.Tests.csproj";
        private const string InterfacesFolder = "../../../RestInterfaces";

        [Test]
        public void ShouldHaveExpectedNumberOfInterfaces()
        {
            var files = Directory.EnumerateFiles(InterfacesFolder)
                                 .Where(x => !x.EndsWith("00.cs"))
                                 .ToArray();

            CodeParser analyzer = new CodeParser();
            var restInterfaces = analyzer.GetRestInterfaces(files);

            restInterfaces.Should().HaveSameCount(files);
        }

        [Test]
        public void Test01()
        {
            string code =
                @"            string actual = Url.Create(settings)
                               .AddSegment(""v1"")
                               .AddSegment(""app"")
                               .AddSegment(""resource with space"")
                               .AddQuery(""param01"", values)
                               .AddQuery(""param02"", ""c"")
                               .ToString(); ";

            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var walker = new Walker();
            walker.Visit(syntaxTree.GetRoot());
            var tree = walker.ToString();
        }

        [Test]
        public void Test02()
        {
            var root = GetRootNode(nameof(RestClientTestXXXX));
            var walker = new Walker();
            walker.Visit(root);
            var tree = walker.ToString();
        }

        private static SyntaxNode GetRootNode(string fileName)
        {
            var interfaceFileName = $"{fileName}.cs";
            var filePath = Path.Combine("../../..", interfaceFileName);
            var fileContent = File.ReadAllText(filePath, Encoding.UTF8);
            var root = CSharpSyntaxTree.ParseText(fileContent)
                                       .GetRoot();
            return root;
        }

        public class Walker : CSharpSyntaxWalker
        {
            private int tabs = 0;
            private StringBuilder str = new StringBuilder();

            public override void Visit(SyntaxNode node)
            {
                this.tabs++;
                var line = new String('\t', this.tabs) + node.Kind();
                this.str.AppendLine(line);
                base.Visit(node);
                this.tabs--;
            }

            public override string ToString()
            {
                return this.str.ToString();
            }
        }
    }
}
