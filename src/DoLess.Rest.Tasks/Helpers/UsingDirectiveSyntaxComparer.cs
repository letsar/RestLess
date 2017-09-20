using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks.Helpers
{
    internal class UsingDirectiveSyntaxComparer : IComparer<UsingDirectiveSyntax>
    {
        public static readonly UsingDirectiveSyntaxComparer Default = new UsingDirectiveSyntaxComparer();
        private const string SystemNamespace = "System";

        private UsingDirectiveSyntaxComparer()
        {
        }

        public int Compare(UsingDirectiveSyntax x, UsingDirectiveSyntax y)
        {
            string xName = x.Name.ToString();
            string yName = y.Name.ToString();

            bool xIsSystem = xName.StartsWith(SystemNamespace);
            bool yIsSystem = yName.StartsWith(SystemNamespace);

            if (xIsSystem == yIsSystem)
            {
                return string.Compare(xName, yName);
            }
            else if (xIsSystem)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}
