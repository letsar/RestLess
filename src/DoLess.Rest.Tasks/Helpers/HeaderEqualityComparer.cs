using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest.Tasks.Helpers
{
    internal class HeaderEqualityComparer : IEqualityComparer<Header>
    {
        public bool Equals(Header x, Header y)
        {
            return string.Equals(x?.Name?.Text, y?.Name?.Text, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(Header x)
        {
            return x.Name.Text.ToUpperInvariant().GetHashCode();
        }
    }
}
