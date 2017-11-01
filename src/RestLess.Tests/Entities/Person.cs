using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestLess.Tests.Entities
{
    public class Person
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public string Age { get; set; }
    }
}
