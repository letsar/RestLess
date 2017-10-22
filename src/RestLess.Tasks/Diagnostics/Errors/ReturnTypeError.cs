using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Diagnostics
{
    internal class ReturnTypeError : MethodError
    {
        public ReturnTypeError(MethodDeclarationSyntax methodDeclaration) :
            base(methodDeclaration, Codes.ReturnTypeErrorCode)
        {
            this.Message = $"The return type of the method '{this.InterfaceName}.{this.MethodName}' is not accepted. A rest method should return Task or Task<T>";
        }
    }
}
