using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class ContinuationExpression : Expression
    {
        public ContinuationExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, "&", '&', true, "=", false)
        {
        }
    }
}
