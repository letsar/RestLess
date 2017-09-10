using DoLess.UriTemplates.Entities;

namespace DoLess.UriTemplates.Expressions
{
    internal interface IExpression
    {
        void Expand(VarSpec varSpec);

        string Process();
    }
}
