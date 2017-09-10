using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class ReservedExpression : Expression
    {
        public ReservedExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, string.Empty, ',', false, string.Empty, true)
        {
        }
    }
}
