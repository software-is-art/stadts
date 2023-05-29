using Microsoft.CodeAnalysis;

namespace StaDTs
{
    [Generator]
    public class RefDelegatesGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var source =
                @"
namespace StaDTs;

public delegate void RefAction<TItem>(in TItem value);
public delegate TResult RefFunc<TItem, TResult>(in TItem value);
";
            context.AddSource("RefDelegates.g.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
