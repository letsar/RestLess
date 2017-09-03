using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class InvalidUrlTemplateError : MethodError
    {
        public InvalidUrlTemplateError(MethodDeclarationSyntax methodDeclaration) :
            base(methodDeclaration, Codes.MalformedUrlTemplateErrorCode)
        {
            this.Message = $"The HTTP attribute of the method '{this.InterfaceName}.{this.MethodName}' has an invalid url template.";
        }
    }
}
