using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class PathExpression : Expression
    {
        public PathExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, "/", '/', false, string.Empty, false)
        {
        }
    }
}
