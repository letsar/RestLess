using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class MalformedUrlTemplateError : MethodError
    {
        public MalformedUrlTemplateError(MethodDeclarationSyntax methodDeclaration) :
            base(methodDeclaration, Codes.MalformedUrlTemplateErrorCode)
        {
            this.Message = $"The HTTP attribute of the method '{this.InterfaceName}.{this.MethodName}' has a malformed url template.";
        }
    }
}
