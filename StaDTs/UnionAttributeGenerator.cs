using Microsoft.CodeAnalysis;

namespace StaDTs
{
    [Generator]
    public class UnionAttributeGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var source =
                @"
namespace StaDTs;

public class UnionAttribute : Attribute { }
";
            context.AddSource("UnionAttribute.g.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context) { }
    }
}
