using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.CodeParsers
{
    internal class CodeParser
    {
        public IReadOnlyList<string> GetRestInterfaces(string[] files)
        {
            var syntaxTrees = files.Select(x => File.ReadAllText(x, Encoding.UTF8))
                                   .Select(x => CSharpSyntaxTree.ParseText(x))
                                   .Select(x => x.GetRoot())
                                   .Where(x => x.DescendantNodes().HasReferenceToDoLessRest())
                                   .Select(x => RestClientGenerator.Generate(x))
                                   .Select(x => x.NormalizeWhitespace())
                                   .Select(x => x.ToFullString())
                                   .ToList();



            //return this.GetSemanticModels(files, references)
            //           .SelectMany(x => GetInterfaceAnalyzers(x))
            //           .Where(x => x.IsRestInterface)
            //           .ToList();

            return null;
        }

        //private IReadOnlyList<SemanticModel> GetSemanticModels(string[] files, IReadOnlyList<string> references)
        //{
        //    var syntaxTrees = files.Select(x => File.ReadAllText(x, Encoding.UTF8))
        //               .Select(x => CSharpSyntaxTree.ParseText(x))
        //               .ToList();

        //    var compilation = CSharpCompilation.Create("DoLess.Rest.TemporaryAssembly")
        //                                       .AddReferences(references.Select(x => MetadataReference.CreateFromFile(x)))
        //                                       .AddSyntaxTrees(syntaxTrees);

        //    // TODO: Add project references: 
        //    // https://stackoverflow.com/questions/43103011/references-to-roslyn-compiled-from-file-from-another-project

        //    return compilation.SyntaxTrees.Select(x => compilation.GetSemanticModel(x))
        //                                  .ToList();
        //}

        //private static IReadOnlyList<InterfaceParser> GetInterfaceAnalyzers()
        //{
        //    return semanticModel.SyntaxTree
        //                        .GetRoot()
        //                        .DescendantNodes()
        //                        .OfType<InterfaceDeclarationSyntax>()
        //                        .Select(x => new InterfaceParser(x))
        //                        .ToList();
        //}
    }
}
