using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest.Tasks
{
    internal class UriParser
    {
        private void Test()
        {
            string id = string.Empty;
            string name = string.Empty;
            string id2 = string.Empty;
            string id3 = string.Empty;
            var url01 = $"/v1/{id}/{name}?id={id2}&id={id3}";

        }

        public void Parse(string uriString)
        {

        }
    }
}
