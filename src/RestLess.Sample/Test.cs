using System;
using System.Collections.Generic;
using System.Text;

namespace RestLess.Sample
{
    class Test
    {
        public Test()
        { 
            var t = RestClient.For<IRestApi01>("");            
        }
    }
}
