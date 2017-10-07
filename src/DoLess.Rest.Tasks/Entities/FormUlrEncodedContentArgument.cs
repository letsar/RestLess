using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Entities
{
    internal sealed class FormUlrEncodedContentArgument : ContentArgument
    {
        public FormUlrEncodedContentArgument(params ArgumentSyntax[] arguments)
            : base(arguments)
        {
        }
    }
}
