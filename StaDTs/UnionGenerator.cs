using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StaDTs
{
    [Generator]
    public class UnionGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
            if (!(context.SyntaxContextReceiver is UnionAttributeSyntaxReceiver receiver))
                return;
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
            foreach (var union in receiver.Unions)
            {
                var model = context.Compilation.GetSemanticModel(union.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(union);
                var name = symbol.ToDisplayString();
                var members = union.Members.OfType<RecordDeclarationSyntax>();
                var source =
@$"
using StaDTs;

public interface {name}<TMember>
    where TMember : {name}<TMember>
{{
    static abstract void Match(in TMember member, {MembersRefActions()});
    static abstract TResult Match<TResult>(in TMember member, {MembersRefFuncs()});
}}

public partial abstract record {name} : {name}<{name}>
{{
    {MembersImplicitOps()}

    public static void Match(in TMember member, {MembersRefActions()}) => member.Match({MembersParameters()});
    public static TResult Match<TResult>(in TMember member, {MembersRefFuncs()}) => member.Match({MembersParameters()});

    protected abstract void Match({MembersRefActions()});
    protected abstract TResult Match<TResult>({MembersRefFuncs()});

    {Members()}
}}
";
                string MembersImplicitOps() => string.Join("\n", members.Select(m => $"public static implicit operator {name}(in {m.Identifier.ValueText} {m.Identifier.ValueText.ToCamelCase()}) => new {m.Identifier.ValueText}.Boxed(in {m.Identifier.ValueText});"));
                string MembersRefActions() => string.Join(", ", members.Select(m => $"RefAction<{m.Identifier.ValueText}> {m.Identifier.ValueText.ToCamelCase()}"));
                string MembersRefFuncs() => string.Join(", ", members.Select(m => $"RefFunc<{m.Identifier.ValueText}, TResult> {m.Identifier.ValueText.ToCamelCase()}"));
                string MembersParameters() => string.Join(", ", members.Select(m => $"{m.Identifier.ValueText.ToCamelCase()}"));
                string Members() => string.Join("\n", members.Select(m =>
@$"
public partial record struct {m.Identifier.ValueText} : {name}<{m.Identifier.ValueText}>
{{
    public static void Match(in {m.Identifier.ValueText} member, {MembersRefActions()}) => {m.Identifier.ValueText}(in member);
    public static TResult Match<TResult>(in {m.Identifier.ValueText} member, {MembersRefFuncs()}) => {m.Identifier.ValueText}(in member);

    public record Boxed : {name}
    {{
        private readonly {m.Identifier.ValueText} _value;
        public Boxed(in value) => _value = value;
        protected override void Match({MembersRefActions()}) => {m.Identifier.ValueText}(in _value);
        protected override TResult Match<TResult>({MembersRefFuncs()}) => {m.Identifier.ValueText}(in _value);
    }}
}}
"));
                Console.WriteLine(source);
                context.AddSource($"{name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new UnionAttributeSyntaxReceiver());
        }

        class UnionAttributeSyntaxReceiver : ISyntaxContextReceiver
        {
            public List<RecordDeclarationSyntax> Unions { get; } = new();

            public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
            {
                if (
                    context.Node is RecordDeclarationSyntax record
                    && record.AttributeLists.Count > 0
                )
                {
                    if (!Debugger.IsAttached)
                    {
                        Debugger.Launch();
                    }
                    foreach (
                        var attribute in record.AttributeLists.SelectMany(
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
                            Unions.Add(record);
                            return;
                        }
                    }
                }
            }
        }
    }
}
