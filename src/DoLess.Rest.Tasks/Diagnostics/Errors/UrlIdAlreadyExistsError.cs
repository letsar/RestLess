using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class UrlIdAlreadyExistsError : ParameterError
    {
        public UrlIdAlreadyExistsError(string id, ParameterSyntax parameter) :
            base(parameter, Codes.UrlParameterAlreadySetErrorCode)
        {
            this.UrlId = id;
            this.Message = $"There is another parameter in the method '{this.InterfaceName}.{this.MethodName}' with the same id ({id}).";
        }

        public string UrlId { get; }
    }
}
