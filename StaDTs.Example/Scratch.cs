using System.Runtime.CompilerServices;
using static StaDTs.Example.Shape;

namespace StaDTs.Example;

[Union]
public partial record Shape
{
    public partial record struct Circle(double Radius);

    public partial record struct Rectangle(double Length, double Width);

    public partial record struct Triangle(double Base, double Height);
}
/*
public interface Shape<TShape> : IUnion<TShape, Circle, Rectangle, Triangle>
    where TShape : Shape<TShape> { }

public partial interface IUnion<TMatch, TA, TB, TC>
    where TMatch : IUnion<TMatch, TA, TB, TC>
    where TA : IUnion<TA, TA, TB, TC>
    where TB : IUnion<TB, TA, TB, TC>
    where TC : IUnion<TC, TA, TB, TC>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract void Match(
        in TMatch match,
        RefAction<TA> a,
        RefAction<TB> b,
        RefAction<TC> c
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract TResult Match<TResult>(
        in TMatch match,
        RefFunc<TA, TResult> a,
        RefFunc<TB, TResult> b,
        RefFunc<TC, TResult> c
    );
}

public abstract partial record Shape : Shape<Shape>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Shape(in Circle circle) => new Circle.Boxed(in circle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Shape(in Triangle circle) => new Triangle.Boxed(in circle);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Shape(in Rectangle circle) => new Rectangle.Boxed(in circle);

    public abstract void Match(
        RefAction<Circle> circle,
        RefAction<Rectangle> rectangle,
        RefAction<Triangle> triangle
    );

    public abstract TResult Match<TResult>(
        RefFunc<Circle, TResult> circle,
        RefFunc<Rectangle, TResult> rectangle,
        RefFunc<Triangle, TResult> triangle
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Match(
        in Shape match,
        RefAction<Circle> a,
        RefAction<Rectangle> b,
        RefAction<Triangle> c
    ) => match.Match(a, b, c);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Match<TResult>(
        in Shape match,
        RefFunc<Circle, TResult> a,
        RefFunc<Rectangle, TResult> b,
        RefFunc<Triangle, TResult> c
    ) => match.Match(a, b, c);

    public partial record struct Circle : Shape<Circle>
    {
        public record Boxed : Shape
        {
            private readonly Circle _value;

            public Boxed(in Circle Circle)
            {
                _value = Circle;
            }

            public override void Match(
                RefAction<Circle> circle,
                RefAction<Rectangle> rectangle,
                RefAction<Triangle> triangle
            ) => circle(in _value);

            public override TResult Match<TResult>(
                RefFunc<Circle, TResult> circle,
                RefFunc<Rectangle, TResult> rectangle,
                RefFunc<Triangle, TResult> triangle
            ) => circle(in _value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Match(
            in Circle match,
            RefAction<Circle> circle,
            RefAction<Rectangle> rectangle,
            RefAction<Triangle> triangle
        ) => circle(in match);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Match<TResult>(
            in Circle match,
            RefFunc<Circle, TResult> circle,
            RefFunc<Rectangle, TResult> rectangle,
            RefFunc<Triangle, TResult> triangle
        ) => circle(in match);
    }

    public partial record struct Rectangle : Shape<Rectangle>
    {
        public record Boxed : Shape
        {
            private readonly Rectangle _value;

            public Boxed(in Rectangle Rectangle)
            {
                _value = Rectangle;
            }

            public override void Match(
                RefAction<Circle> circle,
                RefAction<Rectangle> rectangle,
                RefAction<Triangle> triangle
            ) => rectangle(in _value);

            public override TResult Match<TResult>(
                RefFunc<Circle, TResult> circle,
                RefFunc<Rectangle, TResult> rectangle,
                RefFunc<Triangle, TResult> triangle
            ) => rectangle(in _value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Match(
            in Rectangle shape,
            RefAction<Circle> circle,
            RefAction<Rectangle> rectangle,
            RefAction<Triangle> triangle
        ) => rectangle(in shape);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Match<TResult>(
            in Rectangle shape,
            RefFunc<Circle, TResult> circle,
            RefFunc<Rectangle, TResult> rectangle,
            RefFunc<Triangle, TResult> triangle
        ) => rectangle(in shape);
    }

    public partial record struct Triangle : Shape<Triangle>
    {
        public record Boxed : Shape
        {
            private readonly Triangle _value;

            public Boxed(in Triangle Triangle)
            {
                _value = Triangle;
            }

            public override void Match(
                RefAction<Circle> circle,
                RefAction<Rectangle> rectangle,
                RefAction<Triangle> triangle
            ) => triangle(in _value);

            public override TResult Match<TResult>(
                RefFunc<Circle, TResult> circle,
                RefFunc<Rectangle, TResult> rectangle,
                RefFunc<Triangle, TResult> triangle
            ) => triangle(in _value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Match(
            in Triangle match,
            RefAction<Circle> circle,
            RefAction<Rectangle> rectangle,
            RefAction<Triangle> triangle
        )
        {
            triangle(in match);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Match<TResult>(
            in Triangle match,
            RefFunc<Circle, TResult> circle,
            RefFunc<Rectangle, TResult> rectangle,
            RefFunc<Triangle, TResult> triangle
        )
        {
            return triangle(in match);
        }
    }
}
*/
