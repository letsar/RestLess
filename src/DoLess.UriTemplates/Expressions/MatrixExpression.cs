using System.Collections.Generic;

namespace DoLess.UriTemplates.Expressions
{
    internal class MatrixExpression : Expression
    {
        public MatrixExpression(IReadOnlyDictionary<string, object> variables)
            : base(variables, ";", ';', true, string.Empty, false)
        {
        }
    }
}
