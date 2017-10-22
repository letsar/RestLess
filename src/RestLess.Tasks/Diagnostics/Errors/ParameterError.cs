using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Diagnostics
{
    internal abstract class ParameterError : ErrorDiagnostic
    {
        public ParameterError(ParameterSyntax parameter, string code) :
            base(code)
        {
            this.InterfaceName = parameter.FirstAncestorOrSelf<InterfaceDeclarationSyntax>()?.Identifier.Text;
            this.MethodName = parameter.FirstAncestorOrSelf<MethodDeclarationSyntax>()?.Identifier.Text;
            this.ParameterName = parameter.Identifier.Text;           
            this.SetLocation(parameter.GetLocation());
        }

        public string InterfaceName { get; }

        public string MethodName { get; }

        public string ParameterName { get; }
    }
}
