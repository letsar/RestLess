using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Tests.Helpers
{
    [TestFixture]
    public class DefaultFormFormatterTests
    {
        [TestCase(null)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(true)]
        [TestCase(false)]
        [TestCase(42.2)]
        [TestCase("")]
        public void ShouldBeEmptyIfDefaultOrPrimitive(object value)
        {
            var formatter = new DefaultFormFormatter();

            var result = formatter.Format(value);

            result.Should()
                  .BeEmpty();
        }

        [Test]
        public void ShouldBeEquivalentToDictionary()
        {
            var expected = new Dictionary<string, string>
            {
                ["Key1"] = "value1",
                ["Key2"] = "value2",
            };

            var formatter = new DefaultFormFormatter();

            var result = formatter.Format(expected);

            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void ShouldFormatAnonymousType()
        {
            var expected = new Dictionary<string, string>
            {
                ["Key1"] = "value1",
                ["Key2"] = "value2",
            };

            var value = new
            {
                Key1 = "value1",
                Key2 = "value2"
            };

            var formatter = new DefaultFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void ShouldFormatObjectWithoutAttributes()
        {
            var expected = new Dictionary<string, string>
            {
                ["Key1"] = "value1",
                ["Key2"] = "value2",
            };

            var value = new ObjectWithoutAttributes
            {
                Key1 = "value1",
                Key2 = "value2"
            };

            var formatter = new DefaultFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void ShouldFormatObjectWithUrlIdAttributes()
        {
            var expected = new Dictionary<string, string>
            {
                ["key1"] = "value1",
                ["Key2"] = "value2",
                ["Key4"] = "",
            };

            var value = new ObjectWithUrlIdAttributes
            {
                Key1 = "value1",
                Key2 = "value2",
                Key3 = "value3"
            };

            var formatter = new DefaultFormFormatter();

            var result = formatter.Format(value);

            result.ShouldBeEquivalentTo(expected);
        }

        private class ObjectWithoutAttributes
        {
            public string Key1 { get; set; }

            public string Key2 { get; set; }
        }

        private class ObjectWithUrlIdAttributes
        {
            [Name("key1")]
            public string Key1 { get; set; }
                        
            public string Key2 { get; set; }

            [NameIgnore]
            public string Key3 { get; set; }

            public string Key4 { get; set; }
        }
    }
}
