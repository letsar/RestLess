using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DoLess.Rest.Tasks.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DoLess.Rest.Tasks
{
    internal class RestClientGenerator : CSharpSyntaxRewriter
    {
        private RequestInfo requestInfo;

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
                this.requestInfo = new RequestInfo(node);

                var className = $"{Constants.RestClientPrefix}{node.Identifier.ValueText}";
                var classDeclaration = ClassDeclaration(className)
                                      .WithModifiers(node.Modifiers)
                                      .WithTypeParameterList(node.TypeParameterList)
                                      .WithConstraintClauses(node.ConstraintClauses)
                                      .WithMembers(ImplementMethods(node.Members))
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

        private SyntaxList<MemberDeclarationSyntax> ImplementMethods(SyntaxList<MemberDeclarationSyntax> syntaxList)
        {
            return List<MemberDeclarationSyntax>(syntaxList.OfType<MethodDeclarationSyntax>().Select(x => this.ImplementMethod(x)));
        }

        private MethodDeclarationSyntax ImplementMethod(MethodDeclarationSyntax node)
        {
            var methodDeclaration = node.WithoutAttributes()
                                        .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                        .WithTypeParameterList(node.TypeParameterList.WithoutAttributes())
                                        .WithParameterList(node.ParameterList.WithoutAttributes())
                                        .WithBody(this.ImplementMethodBody(node))
                                        .WithSemicolonToken(MissingToken(SyntaxKind.SemicolonToken));

            return methodDeclaration;
        }



        private BlockSyntax ImplementMethodBody(MethodDeclarationSyntax node)
        {
            var newRequestInfo = this.requestInfo.WithMethod(node);



            // TODO.
            return Block();
        }


    }
}
