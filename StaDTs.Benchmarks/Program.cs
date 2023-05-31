// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dunet;
using StaDTs.Example;

BenchmarkRunner.Run<StaDTsBenchmarks>();

[Union]
public partial record Shape
{
    public partial record Circle(double Radius);

    public partial record Rectangle(double Length, double Width);

    public partial record Triangle(double Base, double Height);
}

[MemoryDiagnoser]
public class StaDTsBenchmarks
{
    /*
    [Benchmark]
    public double Dunet()
    {
        var rectangle = new Shape.Rectangle(10, 10);
        return DunetArea(rectangle);
    }
    */
    /*

    [Benchmark]
    public double Stadts()
    {
        var rectangle = new StaDTs.Example.Shape.Rectangle(10, 10);
        return Stadts(in rectangle);
    }

    [Benchmark]
    public double StadtsBoxed()
    {
        StaDTs.Example.Shape rectangle = new StaDTs.Example.Shape.Rectangle(10, 10);
        return Stadts(rectangle);
    }

    private static double DunetArea(Shape shape) =>
        shape.Match(c => c.Radius, r => r.Length, t => t.Base);

    private static double Stadts<TShape>(in TShape shape) where TShape : StaDTs.Example.Shape<TShape>
    {
        return TShape.Match(
            in shape,
            (in StaDTs.Example.Shape.Circle circle) => circle.Radius,
            (in StaDTs.Example.Shape.Rectangle rectangle) => rectangle.Length,
            (in StaDTs.Example.Shape.Triangle triangle) => triangle.Base
        );
    }
    */
}
