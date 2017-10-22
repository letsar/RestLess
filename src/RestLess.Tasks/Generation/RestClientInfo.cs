using RestLess.Tasks.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks
{
    internal class RestClientInfo
    {
        public RestClientInfo(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            this.InterfaceDeclaration = interfaceDeclaration;
            this.InterfaceName = interfaceDeclaration.Identifier.Text;
            this.ClassName = $"{Constants.RestClientPrefix}{this.InterfaceName}";
        }

        public string InterfaceName { get; }

        public string ClassName { get; }

        public InterfaceDeclarationSyntax InterfaceDeclaration { get; }
    }
}
