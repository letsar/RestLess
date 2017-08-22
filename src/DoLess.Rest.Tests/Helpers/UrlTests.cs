using System;
using System.Collections.Generic;
using System.Text;
using DoLess.Rest.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.Helpers
{
    [TestFixture]
    public class UrlTests
    {
        [Test]
        public void SegmentsShouldBeConcatenated()
        {
            string expected = "/v1/app/resource";
            RestSettings settings = new RestSettings();

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void QueryShouldBeConcatenated()
        {
            string expected = "/v1/app/resource?param01=a&param02=b";
            RestSettings settings = new RestSettings();

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .AddQuery("param01", "a")
                               .AddQuery("param02", "b")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void FragmentShouldBeConcatenated01()
        {
            string expected = "/v1/app/resource?param01=a&param02=b#frag";
            RestSettings settings = new RestSettings();

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .AddQuery("param01", "a")
                               .AddQuery("param02", "b")
                               .SetFragment("frag")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void FragmentShouldBeConcatenated02()
        {
            string expected = "/v1/app/resource#frag";
            RestSettings settings = new RestSettings();

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")                              
                               .SetFragment("frag")
                               .ToString();

            actual.Should().Be(expected);
        }


        [Test]
        public void QueryWithMultipleValues01()
        {
            string expected = "/v1/app/resource?param01=a,b&param02=c";
            RestSettings settings = new RestSettings();

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .AddQuery("param01", "a")
                               .AddQuery("param01", "b")
                               .AddQuery("param02", "c")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void QueryWithMultipleValues02()
        {
            string expected = "/v1/app/resource?param01=a&param01=b&param02=c";
            RestSettings settings = new RestSettings();
            settings.QueryWithMultipleValuesTransformer = RestSettings.MultipleValuesToMultipleValues;

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .AddQuery("param01", "a")
                               .AddQuery("param01", "b")
                               .AddQuery("param02", "c")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void QueryWithMultipleValues03()
        {
            string expected = "/v1/app/resource?param01=a&param01=b&param02=c";
            RestSettings settings = new RestSettings();
            settings.QueryWithMultipleValuesTransformer = RestSettings.MultipleValuesToMultipleValues;
            string[] values = new string[] { "a", "b" };

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource")
                               .AddQuery("param01", values)                               
                               .AddQuery("param02", "c")
                               .ToString();

            actual.Should().Be(expected);
        }

        [Test]
        public void UrlShouldBeEncoded()
        {
            string expected = "/v1/app/resource%20with%20space?param01=a%23&param01=b%20&param02=c";
            RestSettings settings = new RestSettings();
            settings.QueryWithMultipleValuesTransformer = RestSettings.MultipleValuesToMultipleValues;
            string[] values = new string[] { "a#", "b " };

            string actual = Url.Create(settings)
                               .AddSegment("v1")
                               .AddSegment("app")
                               .AddSegment("resource with space")
                               .AddQuery("param01", values)
                               .AddQuery("param02", "c")
                               .ToString();

            actual.Should().Be(expected);
        }
    }
}
