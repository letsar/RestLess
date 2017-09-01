using System.Diagnostics;

namespace DoLess.Rest.Tasks
{
    internal class Header
    {        
        public Header(RequestAttribute attribute)
        {
            this.Name = attribute.GetArgument(0)?.ToLiteral();

            this.Value = attribute.AttachedParameterName?.ToIdentifier() ??
                         attribute.GetArgument(1)?.ToLiteral();

        }

        public Arg Name { get; }

        public Arg Value { get; set; }        
    }
}
