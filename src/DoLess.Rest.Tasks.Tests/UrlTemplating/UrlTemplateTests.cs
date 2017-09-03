using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Exceptions;
using DoLess.Rest.Tasks.UrlTemplating;
using FluentAssertions;
using FluentAssertions.Collections;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.UrlTemplating
{
    [TestFixture]
    public class UrlTemplateTests
    {
        [TestCase("")]
        [TestCase("/")]
        public void ShouldBeEmpty(string template)
        {
            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .Should()
                       .HaveCount(0);

            urlTemplate.QueryKeys
                       .Should()
                       .HaveCount(0);

            urlTemplate.QueryValues
                       .Should()
                       .HaveCount(0);
        }

        [TestCase("/v1/app?sort=desc&tag=foo")]
        [TestCase("v1/app?sort=desc&tag=foo")]
        [TestCase("/v1/app/?sort=desc&tag=foo")]
        [TestCase("/v1/app/?sort=desc&tag=foo&")]
        [TestCase("/v1////app?sort=desc&tag=foo")]
        public void ShouldHaveZeroParameters(string template)
        {
            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app");

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "tag");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("desc", "foo");

            urlTemplate.ParameterNames
                       .Should()
                       .HaveCount(0);
        }

        [Test]
        public void ShouldHaveOneSegmentParameter()
        {
            string template = "/v1/app/{method}?sort=desc&tag=foo";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app", "method");

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "tag");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("desc", "foo");

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("method");

        }

        [Test]
        public void ShouldHaveOneQueryKeyParameter()
        {
            string template = "/v1/app?{sortKey}=desc&tag=foo";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app");

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sortKey", "tag");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("desc", "foo");

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("sortKey");

        }


        [Test]
        public void ShouldHaveOneQueryValueParameter()
        {
            string template = "/v1/app?sort={sortOrder}&tag=foo";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app");

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "tag");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("sortOrder", "foo");

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("sortOrder");

        }

        [Test]
        public void ShouldHaveThreeParameters()
        {
            string template = "/v1/app/{method}?{sortKey}={sortOrder}&tag=foo";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app", "method");

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sortKey", "tag");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("sortOrder", "foo");

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("method", "sortOrder", "sortKey");

        }

        [Test]
        public void ShouldHaveOneListOfParameters()
        {
            string template = "/v1/app/{width}x{height}";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app", "width", "x", "height");

            urlTemplate.QueryKeys
                       .Should()
                       .HaveCount(0);

            urlTemplate.QueryValues
                       .Should()
                       .HaveCount(0);

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("width", "height");

            urlTemplate.Segments[2]
                       .Select(x => x.Value)
                       .Should()
                       .BeEquivalentTo("width", "x", "height");

        }

        [TestCase("/v1/app?sort&key=value")]
        [TestCase("/v1/app?sort=&key=value")]
        public void ShouldHaveSameNumberOfQueryKeysAndQueryValues01(string template)
        {
            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "key");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo(string.Empty, "value");

        }

        [TestCase("/v1/app?sort=value&key")]
        [TestCase("/v1/app?sort=value&key=")]
        public void ShouldHaveSameNumberOfQueryKeysAndQueryValues02(string template)
        {
            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "key");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo("value", string.Empty);

        }

        [TestCase("/v1/app?sort{parameter}by&key=value")]
        public void ShouldHaveSameNumberOfQueryKeysAndQueryValues03(string template)
        {
            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.QueryKeys
                       .Should()
                       .HaveCount(2);

            urlTemplate.QueryKeys
                       .ShouldBeEquivalentTo("sort", "parameter", "by", "key");

            urlTemplate.QueryValues
                       .ShouldBeEquivalentTo(string.Empty, "value");

        }

        [Test]
        public void ShouldHaveThreeSegments()
        {
            string template = "/v1/app/{id}";

            var urlTemplate = UrlTemplate.Parse(template);

            urlTemplate.Segments
                       .ShouldBeEquivalentTo("v1", "app", "id");

            urlTemplate.QueryKeys
                       .Should()
                       .HaveCount(0);

            urlTemplate.QueryValues
                       .Should()
                       .HaveCount(0);

            urlTemplate.ParameterNames
                       .Should()
                       .BeEquivalentTo("id");
        }

        [TestCase("/v1/app/}")]
        [TestCase("{/v1/app/")]
        [TestCase("/v1/{name{/id")]
        [TestCase("/v1/app{id")]
        [TestCase("}/v1/app/")]
        [TestCase("/v1/app/{me?thod}")]
        [TestCase("/v1/app/{me/thod}")]
        [TestCase("/v1/app/{me&thod}")]
        [TestCase("/v1/app/{me=thod}")]
        public void ShouldThrowException(string template)
        {
            Assert.Throws<UrlTemplateException>(new TestDelegate(() => UrlTemplate.Parse(template)));
        }
    }
    static partial class Extensions
    {
        public static AndConstraint<StringCollectionAssertions> ShouldBeEquivalentTo(this IReadOnlyList<IReadOnlyList<Parameter>> self, params string[] values)
        {
            return self.SelectMany(x => x)
                       .Select(x => x.Value)
                       .Should()
                       .BeEquivalentTo(values);
        }
    }
}



