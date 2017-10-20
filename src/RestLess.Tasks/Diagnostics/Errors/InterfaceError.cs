using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Diagnostics
{
    internal abstract class InterfaceError : ErrorDiagnostic
    {
        public InterfaceError(InterfaceDeclarationSyntax interfaceDeclaration, string code) :
            base(code)
        {
            this.InterfaceName = interfaceDeclaration.Identifier.Text;
            this.SetLocation(interfaceDeclaration.GetLocation());
        }

        public string InterfaceName { get; }
    }
}
