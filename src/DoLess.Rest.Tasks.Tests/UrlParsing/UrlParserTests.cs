using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Entities;
using DoLess.Rest.Tasks.UrlParsing;
using FluentAssertions;
using NUnit.Framework;

namespace DoLess.Rest.Tasks.Tests.UrlParsing
{
    //[TestFixture]
    //public class UrlParserTests
    //{
    //    [Test]
    //    public void PathWithLiteralsTest()
    //    {
    //        string url = "/v1/app/books";

    //        var parameters = UrlParser.Parse(url);

    //        parameters.Query
    //                  .Should()
    //                  .BeNullOrEmpty();
    //        parameters.Path
    //                  .Should()
    //                  .HaveCount(3);
    //        parameters.Path
    //                  .Select(x => x.Value)
    //                  .Should()
    //                  .ContainInOrder("v1", "app", "books");
    //    }

    //    [Test]
    //    public void PathWithLiteralsAndVariablesTest()
    //    {
    //        string url = "/v1/app/books/{id}/test/{param2}";

    //        var parameters = UrlParser.Parse(url);

    //        parameters.Query
    //                  .Should()
    //                  .BeNullOrEmpty();
    //        parameters.Path
    //                  .Should()
    //                  .HaveCount(6);
    //        parameters.Path
    //                  .OfType<LiteralParameter>()
    //                  .Select(x => x.Value)
    //                  .Should()
    //                  .ContainInOrder("v1", "app", "books", "test");
    //        parameters.Path
    //                  .OfType<VariableParameter>()
    //                  .Select(x => x.Name)
    //                  .Should()
    //                  .ContainInOrder("id", "param2");
    //        parameters.Path
    //                  .Select(x => x is VariableParameter ? ((VariableParameter)x).Name : x.Value)
    //                  .Should()
    //                  .ContainInOrder("v1", "app", "books", "id", "test", "param2");
    //    }

    //    [Test]
    //    public void PathWithLiteralsAndVariablesAndLiteralQueryItemsTest()
    //    {
    //        string url = "/v1/app/books/{id}/test/{param2}?sort=desc&filter=none";

    //        var parameters = UrlParser.Parse(url);

    //        parameters.Path
    //                  .Should()
    //                  .HaveCount(6);
    //        parameters.Path
    //                  .OfType<LiteralParameter>()
    //                  .Select(x => x.Value)
    //                  .Should()
    //                  .ContainInOrder("v1", "app", "books", "test");
    //        parameters.Path
    //                  .OfType<VariableParameter>()
    //                  .Select(x => x.Name)
    //                  .Should()
    //                  .ContainInOrder("id", "param2");
    //        parameters.Path
    //                  .Select(x => x is VariableParameter ? ((VariableParameter)x).Name : x.Value)
    //                  .Should()
    //                  .ContainInOrder("v1", "app", "books", "id", "test", "param2");
    //        parameters.Query
    //                  .Should()
    //                  .HaveCount(2);
    //        ShouldHaveLiteralKeyAndValue(parameters.Query[0], "sort", "desc");
    //        ShouldHaveLiteralKeyAndValue(parameters.Query[1], "filter", "none");            
    //    }

    //    private void ShouldHaveLiteralKeyAndValue(QueryItemParameters parameter, string key, string value)
    //    {
    //        parameter.Key[0]
    //                 .Value
    //                 .Should()
    //                 .Be(key);

    //        parameter.Value[0]
    //                 .Value
    //                 .Should()
    //                 .Be(value);
    //    }
    //}
}
