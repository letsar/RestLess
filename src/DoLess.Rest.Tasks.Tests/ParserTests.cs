using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.CodeAnalysis;
using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private const string InterfacesFolderPath = "../../../RestInterfaces";

        [Test]
        public void ShouldHaveExpectedNumberOfInterfaces()
        {
            var files = Directory.EnumerateFiles(InterfacesFolderPath)
                                 .ToArray();

            CodeAnalyzer analyzer = new CodeAnalyzer();
            var restInterfaces = analyzer.GetRestInterfaces(files);

            restInterfaces.Should().HaveSameCount(files);
        }
    }
}
