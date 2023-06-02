using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace StaDTs;
[Generator]
public class IncrementalUnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
            "RefDelegates.g.cs",
            SourceText.From(
@"namespace StaDTs;

public delegate void RefAction<TItem>(in TItem value);
public delegate TResult RefFunc<TItem, TResult>(in TItem value);
", Encoding.UTF8)));

        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
     "UnionAttribute.g.cs",
     SourceText.From(
@"namespace StaDTs;

public class UnionAttribute : Attribute { }
", Encoding.UTF8)));

        var unions = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: IsSyntaxTargetForGeneration,
                transform: GetSemanticTargetForGeneration)
            .Where(static m => m is not null);

        var compilationAndUnions = context.CompilationProvider.Combine(unions.Collect());

        context.RegisterSourceOutput(compilationAndUnions, Execute);

        static bool IsSyntaxTargetForGeneration(SyntaxNode node, CancellationToken _) => node is RecordDeclarationSyntax record && record.AttributeLists.Count > 0;
        static RecordDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context, CancellationToken _)
        {
            var record = (RecordDeclarationSyntax)context.Node;
            foreach (var attribute in record.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
            {
                var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol;
                if (attributeSymbol?.ToDisplayString() == "StaDTs.Union")
                {
                    return record;
                }
            }
            return null;
        }
        static void Execute(SourceProductionContext context, (Compilation compilation, ImmutableArray<RecordDeclarationSyntax?> unions) input)
        {
            var (compilation, unions) = input;

            foreach (var union in unions)
            {
                if (union is null)
                {
                    continue;
                }
                var model = compilation.GetSemanticModel(union.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(union);
                if (symbol is null)
                {
                    continue;
                }    
                var name = symbol.ToDisplayString();
                var members = union.Members.OfType<RecordDeclarationSyntax>();
                if (members is null)
                {
                    continue;
                }
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
                context.AddSource($"{name}.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
    }
}
