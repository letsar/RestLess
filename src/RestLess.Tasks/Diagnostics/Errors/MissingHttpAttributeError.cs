using RestLess.Tasks.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Diagnostics
{
    internal class MissingHttpAttributeError : MethodError
    {
        public MissingHttpAttributeError(MethodDeclarationSyntax methodDeclaration) :
            base(methodDeclaration, Codes.MissingHttpAttributeErrorCode)
        {
            this.Message = $"The method '{this.InterfaceName}.{this.MethodName}' either has no {Constants.ProductName} HTTP method attribute or you've used something other than a string literal for the 'path' argument.";
        }
    }
}
