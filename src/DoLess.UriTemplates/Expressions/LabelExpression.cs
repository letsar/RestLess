using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class LabelExpression : Expression
    {
        public LabelExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, ".", '.', false, string.Empty, false)
        {
        }
    }
}
