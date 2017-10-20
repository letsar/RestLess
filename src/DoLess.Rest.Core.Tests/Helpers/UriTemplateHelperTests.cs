using System.Collections.Generic;
using DoLess.Rest.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Core.Tests.Helpers
{
    [TestFixture]
    public class UriTemplateHelperTests
    {
        [Test]
        [TestCase("", "", "")]
        [TestCase("", null, "")]
        [TestCase(null, null, "")]
        [TestCase(null, "", "")]
        [TestCase("/a/b{?query}", "{?id}", "/a/b{?query,id}")]
        [TestCase("/a/b{?query,id}", "{?id}", "/a/b{?query,id,id}")]
        [TestCase("/a/b{?query,id}", "{?id1,id2}", "/a/b{?query,id,id1,id2}")]
        [TestCase("/a/b{?query:5,id}", "{?id1+,id2}", "/a/b{?query:5,id,id1+,id2}")]
        [TestCase("/a/b{?query,id}", "/test{?id1,id2}", "/a/b/test{?query,id,id1,id2}")]
        [TestCase("/a/b{?query,id}", "{/test}{?id1,id2}", "/a/b{/test}{?query,id,id1,id2}")]
        [TestCase("/a/b{?query,id", "{?id1,id2}", "/a/b{?query,id,id1,id2}")]
        [TestCase("/a/b{?query,id", "{?id1,id2", "/a/b{?query,id,id1,id2}")]
        [TestCase("/a/b{?query,id}", "id1,id2", "/a/bid1,id2{?query,id}")]
        public void ShouldBeMergedAsExpected(string uriTemplate, string uriTemplateSuffix, string expectedUriTemplate)
        {
            string actualUriTemplate = UriTemplateHelper.AppendUriTemplateSuffix(uriTemplate, uriTemplateSuffix);

            actualUriTemplate.ShouldBeEquivalentTo(expectedUriTemplate);
        }

        [Test]
        [TestCase("", "", "")]
        [TestCase(null, "", "")]
        [TestCase("/a/b{?query}", "/a/b", "query")]
        [TestCase("/a/b{?query,id}", "/a/b", "query,id")]
        [TestCase("/a/b{?query,id}{&test}{?another}", "/a/b{&test}", "query,id,another")]
        [TestCase("/a/b{?query,id", "/a/b", "query,id")]
        public void ShouldExtractPathAndQueryAsExpected(string uriTemplate, string expectedPath, string expectedQueryParameters)
        {
            UriTemplateHelper.ExtractPathAndQuery(uriTemplate, out string path, out IList<string> set);
            path.ShouldBeEquivalentTo(expectedPath);
            set.Should()
               .BeEquivalentTo(expectedQueryParameters.Split(new[] { "," }, System.StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
