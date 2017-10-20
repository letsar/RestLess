using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Entities
{
    internal class ContentArgument
    {
        public ContentArgument(params ArgumentSyntax[] arguments)
        {
            this.Arguments = arguments;
        }

        public ArgumentSyntax[] Arguments { get; set; }
    }
}
