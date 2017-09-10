using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class FragmentExpression : Expression
    {
        public FragmentExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, "#", '#', false, string.Empty, true)
        {
        }
    }
}
