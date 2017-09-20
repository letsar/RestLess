using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DoLess.Rest.Json.Tests
{
    [TestFixture]
    public class JsonFormFormatterTests
    {
        [Test]
        public void ShouldFormatObjectWithJsonAttributes()
        {
            var expected = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["Key2"] = "value2",
                ["Key3"] = "value3",
                ["Key4"] = "",
            };

            var value = new ObjectWithJsonAttributes
            {
                Key1 = "value1",
                Key2 = "value2",
                Key3 = "value3"
            };

            var formatter = new JsonFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void ShouldFormatObjectWithDataMemberAttributes()
        {
            var expected = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["Key2"] = "value2",
                ["Key3"] = "value3",
                ["Key4"] = "",
            };

            var value = new ObjectWithDataMemberAttributes
            {
                Key1 = "value1",
                Key2 = "value2",
                Key3 = "value3"
            };

            var formatter = new JsonFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }


        [Test]
        public void ShouldFormatObjectWithAllAttributes()
        {
            var expected = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["key2"] = "value2",
                ["key3"] = "value3",
                ["Key4"] = "value4",
                ["Key5"] = "value5",
                ["Key7"] = "value7",
                ["urlId8"] = "value8",
                ["urlId9"] = "value9",
                ["jsonProperty10"] = "value10",
                ["urlId11"] = "value11",
                ["urlId13"] = "value13",
                ["urlId14"] = "value14",
            };

            var value = new ObjectWithAllAttributes
            {
                Key1 = "value1",
                Key2 = "value2",
                Key3 = "value3",
                Key4 = "value4",
                Key5 = "value5",
                Key6 = "value6",
                Key7 = "value7",
                Key8 = "value8",
                Key9 = "value9",
                Key10 = "value10",
                Key11 = "value11",
                Key12 = "value12",
                Key13 = "value13",
                Key14 = "value14",
            };

            var formatter = new JsonFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }

        private class ObjectWithJsonAttributes
        {
            [JsonProperty("key1")]
            public string Key1 { get; set; }

            public string Key2 { get; set; }

            [JsonIgnore]
            public string Key3 { get; set; }

            public string Key4 { get; set; }
        }

        private class ObjectWithDataMemberAttributes
        {
            [DataMember(Name = "key1")]
            public string Key1 { get; set; }

            public string Key2 { get; set; }

            [IgnoreDataMember]
            public string Key3 { get; set; }

            public string Key4 { get; set; }
        }
        private class ObjectWithAllAttributes
        {
            [DataMember(Name = "key1")]
            public string Key1 { get; set; }

            [JsonProperty("key2")]
            public string Key2 { get; set; }

            [UrlId("key3")]
            public string Key3 { get; set; }

            [IgnoreDataMember]
            public string Key4 { get; set; }

            [JsonIgnore]
            public string Key5 { get; set; }

            [UrlIdIgnore]
            public string Key6 { get; set; }

            public string Key7 { get; set; }

            [UrlId("urlId8")]
            [JsonProperty("jsonProperty8")]
            public string Key8 { get; set; }

            [UrlId("urlId9")]
            [DataMember(Name = "dataMember9")]
            public string Key9 { get; set; }

            [JsonProperty("jsonProperty10")]
            [DataMember(Name = "dataMember10")]
            public string Key10 { get; set; }

            [UrlId("urlId11")]
            [JsonProperty("jsonProperty11")]
            [DataMember(Name = "dataMember11")]
            public string Key11 { get; set; }

            [UrlId("urlId12")]
            [JsonProperty("jsonProperty12")]
            [DataMember(Name = "dataMember12")]
            [UrlIdIgnore]
            public string Key12 { get; set; }

            [UrlId("urlId13")]
            [JsonProperty("jsonProperty13")]
            [DataMember(Name = "dataMember13")]
            [JsonIgnore]
            public string Key13 { get; set; }

            [UrlId("urlId14")]
            [JsonProperty("jsonProperty14")]
            [DataMember(Name = "dataMember14")]
            [IgnoreDataMember]
            public string Key14 { get; set; }
        }
    }
}
