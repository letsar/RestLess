using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.UrlParsing
{
    //[TestFixture]
    //public class UrlParameterParserTests
    //{
    //    [Test]
    //    public void SingleVariableTest()
    //    {
    //        string value = "{book}";

    //        var result = UrlParameterParser.Parse(value);

    //        result.Should().HaveCount(1);
    //        ShouldBeVariable(result[0], "book");            
    //    }

    //    [Test]
    //    public void NoVariableTest()
    //    {
    //        string value = "thereisnovariableinhere";

    //        var result = UrlParameterParser.Parse(value);

    //        result.Should().HaveCount(1);
    //        ShouldBeLiteral(result[0], value);
    //    }

    //    [Test]
    //    public void OneLiteralOneVariableOneLiteralOneVariable()
    //    {
    //        string value = "one{book}is{here}";

    //        var result = UrlParameterParser.Parse(value);

    //        result.Should().HaveCount(4);
    //        ShouldBeLiteral(result[0], "one");
    //        ShouldBeVariable(result[1], "book");
    //        ShouldBeLiteral(result[2], "is");
    //        ShouldBeVariable(result[3], "here");
    //    }

    //    [Test]
    //    public void OneLiteralTwoVariables()
    //    {
    //        string value = "one{book}{here}";

    //        var result = UrlParameterParser.Parse(value);

    //        result.Should().HaveCount(3);
    //        ShouldBeLiteral(result[0], "one");
    //        ShouldBeVariable(result[1], "book");            
    //        ShouldBeVariable(result[2], "here");
    //    }

    //    [Test]
    //    public void MixedUpBrackets()
    //    {
    //        string value = "one{book{{here}";

    //        var result = UrlParameterParser.Parse(value);

    //        result.Should().HaveCount(3);
    //        ShouldBeLiteral(result[0], "one");
    //        ShouldBeLiteral(result[1], "book");
    //        ShouldBeVariable(result[2], "here");
            
    //    }

    //    private static void ShouldBeLiteral(IParameter parameter, string name)
    //    {
    //        parameter.Should()
    //                 .BeOfType<LiteralParameter>();

    //        parameter.Value
    //                 .Should()
    //                 .Be(name);
    //    }

    //    private static void ShouldBeVariable(IParameter parameter, string name)
    //    {
    //        parameter.Should()
    //                 .BeOfType<VariableParameter>();

    //        ((VariableParameter)parameter).Name
    //                                      .Should()
    //                                      .Be(name);
    //    }
    //}
}
