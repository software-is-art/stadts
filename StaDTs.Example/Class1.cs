

using static StaDTs.Example.Shape;

namespace StaDTs.Example;

[Union]
public abstract partial record Shape
{
    partial record struct Circle(double Radius);
    partial record struct Rectangle(double Length, double Width);
    partial record struct Triangle(double Base, double Height);
}

public delegate void CircleAction(in Circle value);
public delegate TResult CircleFunc<TResult>(in Circle value);

public delegate void RectangleAction(in Rectangle value);
public delegate TResult RectangleFunc<TResult>(in Rectangle value);

public delegate void TriangleAction(in Triangle value);
public delegate TResult TriangleFunc<TResult>(in Triangle value);

public partial interface Shape<TMatch> where TMatch : Shape<TMatch>
{
    public static abstract void Match(in TMatch match, CircleAction circle, RectangleAction rectangle, TriangleAction triangle);

    public static abstract TResult Match<TResult>(in TMatch match, CircleFunc<TResult> circle, RectangleFunc<TResult> rectangle, TriangleFunc<TResult> triangle);
}


public abstract partial record Shape
{
    public abstract void Match(CircleAction circle, RectangleAction rectangle, TriangleAction triangle);
    public abstract TResult Match<TResult>(CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle);

    private record CircleBoxed : Shape
    {
        private readonly Circle _circle;
        public CircleBoxed(in Circle Circle)
        {
            _circle = Circle;
        }
        public override void Match(CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            circle(in _circle);
        }

        public override TResult Match<TResult>(CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return circle(in _circle);
        }
    }
    private record RectangleBoxed : Shape
    {
        private readonly Rectangle _rectangle;
        public RectangleBoxed(in Rectangle Rectangle)
        {
            _rectangle = Rectangle;
        }
        public override void Match(CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            rectangle(in _rectangle);
        }

        public override TResult Match<TResult>(CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return rectangle(in _rectangle);
        }
    }
    private record TriangleBoxed : Shape
    {
        private readonly Triangle _triangle;
        public TriangleBoxed(in Triangle Triangle)
        {
            _triangle = Triangle;
        }
        public override void Match(CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            triangle(in _triangle);
        }

        public override TResult Match<TResult>(CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return triangle(in _triangle);
        }
    }

    public partial record struct Circle : Shape<Circle>
    {
        public static void Match(in Circle match, CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            circle(in match);
        }

        public static TResult Match<TResult>(in Circle match, CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return circle(in match);
        }

        public static implicit operator Shape(Circle circle) => new CircleBoxed(circle);
    }
    public partial record struct Rectangle : Shape<Rectangle>
    {
        public static void Match(in Rectangle match, CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            rectangle(in match);
        }

        public static TResult Match<TResult>(in Rectangle match, CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return rectangle(in match);
        }
    }
    public partial record struct Triangle : Shape<Triangle>
    {
        public static void Match(in Triangle match, CircleAction circle, RectangleAction rectangle, TriangleAction triangle)
        {
            triangle(in match);
        }

        public static TResult Match<TResult>(in Triangle match, CircleFunc<TResult> circle, RectangleFunc<TResult>rectangle, TriangleFunc<TResult> triangle)
        {
            return triangle(in match);
        }
    }
}

 


