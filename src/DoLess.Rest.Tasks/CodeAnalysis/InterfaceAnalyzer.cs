using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.CodeAnalysis
{
    public class InterfaceAnalyzer
    {
        private readonly InterfaceDeclarationSyntax interfaceSyntax;
        private readonly SemanticModel semanticModel;
        private readonly IReadOnlyList<MethodDeclarationSyntax> restMethods;

        public InterfaceAnalyzer(InterfaceDeclarationSyntax interfaceSyntax, SemanticModel semanticModel)
        {
            this.interfaceSyntax = interfaceSyntax;
            this.semanticModel = semanticModel;
            this.restMethods = this.GetRestMethods();

            this.IsRestInterface = this.restMethods?.Count > 0;
            this.Requests = new List<RequestAnalyzer>();

            this.InterfaceName = interfaceSyntax.Identifier.ValueText;
        }

        public bool IsRestInterface { get; }

        public string InterfaceName { get; }

        public List<RequestAnalyzer> Requests { get; }

        private IReadOnlyList<MethodDeclarationSyntax> GetRestMethods()
        {
            return this.interfaceSyntax.Members
                                       .OfType<MethodDeclarationSyntax>()
                                       .Where(x => IsRestMethod(x, this.semanticModel))
                                       .ToList();
        }
        private static bool IsRestMethod(MethodDeclarationSyntax method, SemanticModel semanticModel)
        {
            return method.AttributeLists.SelectMany(list => list.Attributes)
                                        .Select(a => semanticModel.GetSymbolInfo(a).CandidateSymbols.FirstOrDefault())
                                        .Any(s => (s?.ContainingType.InheritsFrom<HttpMethodAttribute>()).GetValueOrDefault());
        }
    }
}
