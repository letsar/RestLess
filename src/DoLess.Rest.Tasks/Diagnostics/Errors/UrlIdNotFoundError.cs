using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Diagnostics
{
    internal class UrlIdNotFoundError : MethodError
    {
        public UrlIdNotFoundError(IReadOnlyList<string> urlIds, MethodDeclarationSyntax methodDeclaration) :
            base(methodDeclaration, Codes.UrlParameterNotFoundErrorCode)
        {
            this.UrlIds = urlIds;

            string start;
            string end;
            if (urlIds.Count == 1)
            {
                start = $"The id ({urlIds[0]})";
                end = "has no matching parameters";
            }
            else
            {
                start = $"The ids ({string.Join(",", urlIds)})";
                end = "have no matching parameters";
            }

            this.Message = $"{start} in the HTTP attribute of the method '{this.InterfaceName}.{this.MethodName}' {end}.";
        }

        public IReadOnlyList<string> UrlIds { get; }
    }
}
