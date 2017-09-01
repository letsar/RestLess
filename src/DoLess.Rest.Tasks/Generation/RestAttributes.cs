using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DoLess.Rest.Tasks
{
    //internal class RestAttributes
    //{
    //    public RestAttributes(InterfaceDeclarationSyntax node)
    //    {
    //        this.Interface = node.AttributeLists
    //                             .ToRestAttributes();
    //    }

    //    public IReadOnlyList<RestAttribute> Interface { get; }

    //    public IReadOnlyList<RestAttribute> Method { get; private set; }

    //    public IReadOnlyList<RestAttribute> Parameter { get; private set; }

    //    public RestAttributes WithMethod(MethodDeclarationSyntax node)
    //    {
    //        this.Method = node.AttributeLists
    //                          .ToRestAttributes();

    //        this.Parameter = node.ParameterList?
    //                             .Parameters
    //                             .Select(x => GetRestAttribute(x))
    //                             .ToList();

    //        return this;
    //    }

    //    private static RestAttribute GetRestAttribute(ParameterSyntax parameterSyntax)
    //    {
    //        // Only the first REST attribute will be used!
    //        var restAttribute = parameterSyntax.AttributeLists
    //                                           .ToRestAttributes()
    //                                           .FirstOrDefault();

    //        if (restAttribute == null)
    //        {
    //            // There are no REST attributes on this parameter, we should retrieve one based on the parameter's name
    //            // or create a QueryAttribute (the default one).
    //        }

    //        return restAttribute;
    //    }
    //}
}
