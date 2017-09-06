using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Test.A
{
    namespace Test.B
    {
        public class C
        {

        }
    }
}

namespace DoLess.Rest.Tasks.Tests
{


    [TestFixture]
    public class BullshitTests
    {
        [Test]
        public void Test01()
        {
            string tt = typeof(Test.A.Test.B.C).FullName;

            var tree = CSharpSyntaxTree.ParseText(
            @"namespace Test.A
            {
                using MyNamespace = DoLess.Rest;
                using Mk.t
                namespace Test.B
                {
                    public class C
                    {
                        public global::System.Threading.Tasks.Task T()
                        {
                            return null;
                        }

                        public byte[] U(){return null;}

                        public Task<byte> EE(){return null;}
                    }
                }
            }");

            var d = tree.GetRoot().DescendantNodes().ToList();
            var c = d.OfType<ClassDeclarationSyntax>().FirstOrDefault();
            var f = c.Ancestors()
                     .OfType<NamespaceDeclarationSyntax>()
                     .Select(x => x.Name.ToString())
                     .Reverse();

            var m = d.OfType<MethodDeclarationSyntax>()
                     .ToList();

            var t = typeof(byte[]).Name;

            var n = string.Join('.', f);

            var u =c.Ancestors()    
                    .SelectMany(x => x.GetUsings())                    
                    .ToList();

        }
    }
}
