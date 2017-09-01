using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DoLess.Rest.Tasks
{
    internal class RestClientGenerator : CSharpSyntaxRewriter
    {
        private RestClientGenerator()
        {
        }

        public static SyntaxNode Generate(SyntaxNode rootNode)
        {
            var generator = new RestClientGenerator();
            return generator.Visit(rootNode);
        }

        public static ClassDeclarationSyntax Generate(InterfaceDeclarationSyntax interfaceDeclarationSyntax)
        {
            var generator = new RestClientGenerator();
            return generator.Visit(interfaceDeclarationSyntax) as ClassDeclarationSyntax;
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (node.IsRestInterface())
            {
                var requestInfo = new RequestInfo(node);

                var className = $"RestClientFor{node.Identifier.ValueText}";
                var classDeclaration = ClassDeclaration(className)
                                      .WithModifiers(node.Modifiers)
                                      .WithTypeParameterList(node.TypeParameterList)
                                      .WithConstraintClauses(node.ConstraintClauses)
                                      .WithMembers(ImplementMethods(node.Members, requestInfo))
                                      .WithBaseList(BaseList(SeparatedList<BaseTypeSyntax>(new[] { SimpleBaseType(node.GetTypeSyntax()) })));

                return classDeclaration;
            }
            else
            {
                return null;
            }
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            return null;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            return null;
        }

        private SyntaxList<MemberDeclarationSyntax> ImplementMethods(SyntaxList<MemberDeclarationSyntax> syntaxList, RequestInfo requestInfo)
        {
            return List<MemberDeclarationSyntax>(syntaxList.OfType<MethodDeclarationSyntax>().Select(x => this.ImplementMethod(x, requestInfo)));
        }

        private MethodDeclarationSyntax ImplementMethod(MethodDeclarationSyntax node, RequestInfo requestInfo)
        {
            var methodDeclaration = node.WithoutAttributes()
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithTypeParameterList(node.TypeParameterList.WithoutAttributes())
                                        .WithParameterList(node.ParameterList.WithoutAttributes())
                                        .WithBody(this.ImplementMethodBody(node, requestInfo))
                                        .WithSemicolonToken(MissingToken(SyntaxKind.SemicolonToken));

            return methodDeclaration;
        }



        private BlockSyntax ImplementMethodBody(MethodDeclarationSyntax node, RequestInfo requestInfo)
        {
            var newRequestInfo = requestInfo.WithMethod(node);



            // TODO.
            return Block();
        }


    }
}
