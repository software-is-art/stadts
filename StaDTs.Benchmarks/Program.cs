// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Dunet;

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

    [Benchmark]
    public double Dunet() => DunetArea(new Shape.Circle(10));

    [Benchmark]
    public double Stadts() => Stadts(new StaDTs.Example.Shape.Circle(10));

    private static double DunetArea(Shape shape) => shape.Match(
            c => c.Radius,
            r => r.Length,
            t => t.Base
        );

    private static double Stadts<TShape>(in TShape shape) where TShape : StaDTs.Example.Shape<TShape> {
        return TShape.Match(
            in shape,
            (in circle) => circle.Radius,
            Rectangle,
            Triangle
        );
        T Circle<T>(in Shape.Circle circle) where T : double => circle.Radius;
        double Rectangle(in Shape.Rectangle rectangle) => rectangle.Length;
        double Triangle(in Shape.Triangle triangle) => triangle.Base;
     }
}

