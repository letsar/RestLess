﻿using System;
using System.Collections.Generic;
using System.Text;
using RestLess.Generated;

namespace RestLess.Tests
{
    internal class SimpleRestClient : RestClientBase
    {
        public SimpleRestClient(RestSettings settings = null)
        {
            ((IRestClient)this).Settings = settings ?? new RestSettings();
        }
    }
}
