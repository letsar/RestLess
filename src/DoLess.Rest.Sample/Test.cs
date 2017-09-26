using System;
using System.Collections.Generic;
using System.Text;

namespace DoLess.Rest.Sample
{
    class Test
    {
        public Test()
        {
            var t = RestClient.For<IRestApi01>("");            
        }
    }
}
