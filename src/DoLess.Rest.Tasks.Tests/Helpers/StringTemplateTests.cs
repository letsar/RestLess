using System;
using System.Linq;
using DoLess.Rest.Tasks.Exceptions;
using DoLess.Rest.Tasks.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.Helpers
{
    [TestFixture]
    public class StringTemplateTests
    {
        [TestCase("")]
        public void ShouldBeEmpty(string template)
        {
            var stringTemplate = StringTemplate.Parse(template);

            stringTemplate.Parameters
                          .Should()
                          .BeEmpty();

            stringTemplate.Parts
                          .Should()
                          .BeEmpty();
        }

        [TestCase("/v1/app?sort=desc&tag=foo")]
        [TestCase("v1/app?sort=desc&tag=foo")]
        [TestCase("/v1/app/?sort=desc&tag=foo")]
        [TestCase("/v1/app/?sort=desc&tag=foo&")]
        [TestCase("/v1////app?sort=desc&tag=foo")]
        public void ShouldHaveZeroParameters(string template)
        {
            var stringTemplate = StringTemplate.Parse(template);

            stringTemplate.Parameters
                          .Should()
                          .BeEmpty();

            stringTemplate.Parts
                          .Should()
                          .HaveCount(1);
        }

        [TestCase("/v1/app/{method}", "method")]
        [TestCase("/v1/app/{method}?sort={sortOrder}", "method", "sortOrder")]
        [TestCase("{api}/v1/app/{method}", "api", "method")]
        [TestCase("/v1/app/{width}x{height}", "width", "height")]
        public void ShouldHaveParameters(string template, params string[] parameters)
        {
            var stringTemplate = StringTemplate.Parse(template);

            stringTemplate.Parameters
                          .Select(x => x.Value)
                          .ShouldBeEquivalentTo(parameters);

        }

        [TestCase("/v1/app/}")]
        [TestCase("{/v1/app/")]
        [TestCase("/v1/{name{/id")]
        [TestCase("/v1/app{id")]
        [TestCase("}/v1/app/")]
        public void ShouldThrowException(string template)
        {
            Action job = () => StringTemplate.Parse(template);
            job.ShouldThrowExactly<StringTemplateException>();
        }

        [TestCase("/v1/app/{me?thod}")]
        [TestCase("/v1/app/{me/thod}")]
        [TestCase("/v1/app/{me&thod}")]
        [TestCase("/v1/app/{me=thod}")]
        public void ShouldNotThrowException(string template)
        {
            Action job = () => StringTemplate.Parse(template);
            job.ShouldNotThrow<StringTemplateException>();
        }
    }   
}



