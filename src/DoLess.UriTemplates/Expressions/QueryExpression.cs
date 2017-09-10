using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class QueryExpression : Expression
    {
        public QueryExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, "?", '&', true, "=", false)
        {
        }
    }
}
