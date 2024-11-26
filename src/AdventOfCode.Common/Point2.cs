using System.Numerics;

namespace AdventOfCode.Common
{
    public struct Point2<T> : IEquatable<Point2<T>> where T : INumber<T>
    {
        public static readonly Point2<T> Zero = new Point2<T>(T.Zero, T.Zero);
        public static readonly Point2<T> One = new Point2<T>(T.One, T.One);
        public static readonly Point2<T> UnitX = new Point2<T>(T.One, T.Zero);
        public static readonly Point2<T> UnitY = new Point2<T>(T.Zero, T.One);

        public readonly T X;
        public readonly T Y;

        public Point2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public static Point2<T> Parse(string input)
        {
            string[] parts = input.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries);
            return new Point2<T>(T.Parse(parts[0], provider: null), T.Parse(parts[1], provider: null));
        }

        public static Point2<T> Min(Point2<T> left, Point2<T> right)
        {
            return new Point2<T>(T.Min(left.X, right.X), T.Min(left.Y, right.Y));
        }

        public static Point2<T> Max(Point2<T> left, Point2<T> right)
        {
            return new Point2<T>(T.Max(left.X, right.X), T.Max(left.Y, right.Y));
        }

        public void Deconstruct(out T x, out T y)
        {
            x = X;
            y = Y;
        }

        public Point2 Sign() => new Point2(T.Sign(X), T.Sign(Y));

        public T Sum() => checked(X + Y);

        public T Product() => checked(X * Y);

        public T Manhattan() => checked(T.Abs(X) + T.Abs(Y));

        public bool IsUniform() => (X == Y);

        public bool Equals(Point2<T> other) => (this == other);

        public override bool Equals(object obj) => (obj is Point2<T> other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public Point2<U> As<U>() where U : INumber<U> => new Point2<U>(U.CreateChecked(X), U.CreateChecked(Y));

        public static Point2<T> operator +(Point2<T> p) => p;
        public static Point2<T> operator +(Point2<T> p, T i) => new Point2<T>(p.X + i, p.Y + i);
        public static Point2<T> operator +(Point2<T> p, Direction d) => p + d.Unit.As<T>();
        public static Point2<T> operator +(Point2<T> left, Point2<T> right) => new Point2<T>(left.X + right.X, left.Y + right.Y);

        public static Point2<T> operator -(Point2<T> p) => new Point2<T>(-p.X, -p.Y);
        public static Point2<T> operator -(Point2<T> p, T i) => new Point2<T>(p.X - i, p.Y - i);
        public static Point2<T> operator -(Point2<T> p, Direction d) => p - d.Unit.As<T>();
        public static Point2<T> operator -(Point2<T> left, Point2<T> right) => new Point2<T>(left.X - right.X, left.Y - right.Y);

        public static Point2<T> operator *(Point2<T> p, T i) => new Point2<T>(p.X * i, p.Y * i);
        public static Point2<T> operator *(Point2<T> left, Point2<T> right) => new Point2<T>(left.X * right.X, left.Y * right.Y);

        public static Point2<T> operator /(Point2<T> p, T i) => new Point2<T>(p.X / i, p.Y / i);
        public static Point2<T> operator /(Point2<T> left, Point2<T> right) => new Point2<T>(left.X / right.X, left.Y / right.Y);

        public static Point2<T> operator %(Point2<T> p, T i) => new Point2<T>(p.X % i, p.Y % i);
        public static Point2<T> operator %(Point2<T> left, Point2<T> right) => new Point2<T>(left.X % right.X, left.Y % right.Y);

        public static Point2<T> operator ~(Point2<T> p) => new Point2<T>(p.Y, p.X);

        public static bool operator ==(Point2<T> left, Point2<T> right) => (left.X == right.X && left.Y == right.Y);
        public static bool operator !=(Point2<T> left, Point2<T> right) => !(left == right);
        public static bool operator <(Point2<T> left, Point2<T> right) => (left.X < right.X && left.Y < right.Y);
        public static bool operator >(Point2<T> left, Point2<T> right) => (left.X > right.X && left.Y > right.Y);
        public static bool operator <=(Point2<T> left, Point2<T> right) => (left.X <= right.X && left.Y <= right.Y);
        public static bool operator >=(Point2<T> left, Point2<T> right) => (left.X >= right.X && left.Y >= right.Y);

        public static implicit operator (T X, T Y)(Point2<T> point) => (point.X, point.Y);
        public static implicit operator Point2<T>((T X, T Y) pair) => new Point2<T>(pair.X, pair.Y);
    }
}
