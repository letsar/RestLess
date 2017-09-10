using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class DefaultExpression : Expression
    {
        public DefaultExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, string.Empty, ',', false, string.Empty, false)
        {
        }
    }
}
