using DoLess.UriTemplates.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.UriTemplates.Tests
{
    [TestFixture]
    public class PercentEncodingTests
    {
        [TestCase(' ', "%20")]
        [TestCase('!', "%21")]
        [TestCase('\"', "%22")]
        [TestCase('#', "%23")]
        [TestCase('$', "%24")]
        [TestCase('%', "%25")]
        [TestCase('&', "%26")]
        [TestCase('\'', "%27")]
        [TestCase('(', "%28")]
        [TestCase(')', "%29")]
        [TestCase('*', "%2A")]
        [TestCase('+', "%2B")]
        [TestCase(',', "%2C")]
        [TestCase('-', "%2D")]
        [TestCase('.', "%2E")]
        [TestCase('/', "%2F")]
        [TestCase(':', "%3A")]
        [TestCase(';', "%3B")]
        [TestCase('<', "%3C")]
        [TestCase('=', "%3D")]
        [TestCase('>', "%3E")]
        [TestCase('?', "%3F")]
        [TestCase('@', "%40")]
        [TestCase('[', "%5B")]
        [TestCase('\\', "%5C")]
        [TestCase(']', "%5D")]
        [TestCase('^', "%5E")]
        [TestCase('`', "%60")]
        [TestCase('{', "%7B")]
        [TestCase('|', "%7C")]
        [TestCase('}', "%7D")]
        [TestCase('~', "%7E")]
        [TestCase('♥', "%E2%99%A5")]
        public void ShouldBeEncodedTo(char value, string expected)
        {
            string result = PercentEncoding.Encode(value);
            result.Should()
                  .Be(expected);
        }
    }
}
