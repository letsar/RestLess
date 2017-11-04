using System;
using System.Collections.Generic;
using System.Text;
using DoLess.UriTemplates;

namespace RestLess.Tests
{
    public class SubNamespaceObject : QueryObject
    {
        public int Page
        {
            get => this.Get<int>();
            set => this.Set<int>(value);
        }

        public int Limit
        {
            get => this.Get<int>();
            set => this.Set<int>(value);
        }
    }
}
