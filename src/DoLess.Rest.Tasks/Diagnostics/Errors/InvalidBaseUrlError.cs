using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class InvalidBaseUrlError : InterfaceError
    {
        public InvalidBaseUrlError(InterfaceDeclarationSyntax interfaceDeclaration) :
            base(interfaceDeclaration, Codes.InvalidBaseUrlErrorCode)
        {
            this.Message = $"The BaseUrl attribute on the interface '{this.InterfaceName}' has an invalid relative url. It cannot have a query string.";
        }
    }
}
