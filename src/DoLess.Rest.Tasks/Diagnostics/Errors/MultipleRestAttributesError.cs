using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class MultipleRestAttributesError : ParameterError
    {
        public MultipleRestAttributesError(ParameterSyntax parameter) :
            base(parameter, Codes.MultipleRestAttributesErrorCode)
        {
            this.Message = $"The parameter '{this.ParameterName}' of the method '{this.InterfaceName}.{this.MethodName}' contains more than one {Constants.ProductName} attribute.";
        }       
    }
}
