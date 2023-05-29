using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace StaDTs
{
    [Generator]
    public class UnionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is UnionAttributeSyntaxReceiver receiver))
                return;

            foreach (var union in receiver.Unions) { }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new UnionAttributeSyntaxReceiver());
        }

        class UnionAttributeSyntaxReceiver : ISyntaxContextReceiver
        {
            public List<InterfaceDeclarationSyntax> Unions { get; } = new();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (
                    context.Node is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                    && interfaceDeclarationSyntax.AttributeLists.Count > 0
                )
                {
                    foreach (
                        var attribute in interfaceDeclarationSyntax.AttributeLists.SelectMany(
                            attributeList => attributeList.Attributes
                        )
                    )
                    {
                        var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
                        if (
                            attributeSymbol != null
                            && attributeSymbol.ContainingNamespace.ToDisplayString()
                                + "."
                                + attributeSymbol.Name
                                == "StaDTs.Union"
                        )
                        {
                            Unions.Add(interfaceDeclarationSyntax);
                            return;
                        }
                    }
                }
            }
        }
    }
}
