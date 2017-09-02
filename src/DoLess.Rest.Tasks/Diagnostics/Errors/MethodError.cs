using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal abstract class MethodError : ErrorDiagnostic
    {
        public MethodError(MethodDeclarationSyntax methodDeclaration, string code) :
            base(code)
        {
            this.InterfaceName = methodDeclaration.FirstAncestorOrSelf<InterfaceDeclarationSyntax>()?.Identifier.Text;
            this.MethodName = methodDeclaration.Identifier.Text;
            this.SetLocation(methodDeclaration.GetLocation());
        }

        public string InterfaceName { get; }

        public string MethodName { get; }
    }
}
