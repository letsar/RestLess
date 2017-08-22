using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.CodeAnalysis
{
    public class CodeAnalyzer
    {
        public IReadOnlyList<InterfaceAnalyzer> GetRestInterfaces(params string[] files)
        {
            return this.GetSemanticModels(files)
                       .SelectMany(x => GetInterfaceAnalyzers(x))
                       .Where(x => x.IsRestInterface)
                       .ToList();
        }

        private IReadOnlyList<SemanticModel> GetSemanticModels(string[] files)
        {
            var syntaxTrees = files.Select(x => File.ReadAllText(x, Encoding.UTF8))
                       .Select(x => CSharpSyntaxTree.ParseText(x))
                       .ToList();

            var compilation = CSharpCompilation.Create("DoLess.Rest.TemporaryAssembly")
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                               .AddReferences(MetadataReference.CreateFromFile(typeof(HttpMethodAttribute).Assembly.Location))
                                               .AddSyntaxTrees(syntaxTrees);

            return compilation.SyntaxTrees.Select(x => compilation.GetSemanticModel(x))
                                          .ToList();
        }

        private static IReadOnlyList<InterfaceAnalyzer> GetInterfaceAnalyzers(SemanticModel semanticModel)
        {
            return semanticModel.SyntaxTree
                                .GetRoot()
                                .DescendantNodes()
                                .OfType<InterfaceDeclarationSyntax>()
                                .Select(x => new InterfaceAnalyzer(x, semanticModel))
                                .ToList();
        }
    }
}
