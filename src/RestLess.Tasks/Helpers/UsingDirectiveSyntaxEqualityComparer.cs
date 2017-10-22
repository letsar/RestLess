using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RestLess.Tasks.Helpers
{
    internal class UsingDirectiveSyntaxEqualityComparer : IEqualityComparer<UsingDirectiveSyntax>
    {
        public static readonly UsingDirectiveSyntaxEqualityComparer Default = new UsingDirectiveSyntaxEqualityComparer();

        private UsingDirectiveSyntaxEqualityComparer()
        {

        }

        public bool Equals(UsingDirectiveSyntax x, UsingDirectiveSyntax y)
        {
            return x?.Name?.ToString() == y?.Name?.ToString();
        }

        public int GetHashCode(UsingDirectiveSyntax obj)
        {
            return obj?.Name?.ToString()?.GetHashCode() ?? 0;
        }
    }
}
