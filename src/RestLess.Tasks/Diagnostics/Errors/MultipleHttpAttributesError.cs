using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Diagnostics
{
    internal class MultipleHttpAttributesError : MethodError
    {
        public MultipleHttpAttributesError(MethodDeclarationSyntax method) :
            base(method, Codes.MultipleHttpAttributesErrorCode)
        {
            this.Message = $"The method '{this.InterfaceName}.{this.MethodName}' contains more than one HTTP method attribute.";
        }       
    }
}
