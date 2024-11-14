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
            string[] parts = input.Split(',');
            return new Point2<T>(T.Parse(parts[0], provider: null), T.Parse(parts[1], provider: null));
        }

        public static IEnumerable<Point2<T>> Line(Point2<T> left, Point2<T> right)
        {
            Point2<T> step;

            if (left.X == right.X)
            {
                if (left.Y <= right.Y)
                {
                    step = UnitY;
                }
                else
                {
                    step = -UnitY;
                }
            }
            else if (left.Y == right.Y)
            {
                if (left.X <= right.X)
                {
                    step = UnitX;
                }
                else
                {
                    step = -UnitX;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            yield return left;

            while (left != right)
            {
                left += step;
                yield return left;
            }
        }

        public static IEnumerable<Point2<T>> Quadrant(Point2<T> bounds)
        {
            T xSign = (bounds.X >= T.Zero) ? T.One : -T.One;
            T ySign = (bounds.Y >= T.Zero) ? T.One : -T.One;

            for (T y = T.Zero; y < bounds.Y; y++)
            {
                for (T x = T.Zero; x < bounds.X; x++)
                {
                    yield return new Point2<T>(x * xSign, y * ySign);
                }
            }
        }

        public static Point2<T> Min(Point2<T> left, Point2<T> right)
        {
            return new Point2<T>(T.Min(left.X, right.X), T.Min(left.Y, right.Y));
        }

        public static Point2<T> Max(Point2<T> left, Point2<T> right)
        {
            return new Point2<T>(T.Max(left.X, right.X), T.Max(left.Y, right.Y));
        }

        public IEnumerable<Point2<T>> Adjacent()
        {
            yield return this - UnitX;
            yield return this - UnitY;
            yield return this + UnitX;
            yield return this + UnitY;
        }

        public IEnumerable<Point2<T>> Adjacent(Point2<T> bounds)
        {
            if (X > T.Zero) yield return this - UnitX;
            if (Y > T.Zero) yield return this - UnitY;
            if (X < bounds.X - T.One) yield return this + UnitX;
            if (Y < bounds.Y - T.One) yield return this + UnitY;
        }

        public IEnumerable<Point2<T>> Surrounding()
        {
            yield return this - UnitY - UnitX; // Top Left
            yield return this - UnitY; // Top
            yield return this - UnitY + UnitX; // Top Right
            yield return this + UnitX; // Right
            yield return this + UnitY + UnitX; // Bottom Right
            yield return this + UnitY; // Bottom
            yield return this + UnitY - UnitX; // Bottom Left
            yield return this - UnitX; // Left
        }

        public IEnumerable<Point2<T>> Surrounding(Point2<T> bounds)
        {
            if (Y > T.Zero)
            {
                if (X > T.Zero) yield return this - UnitY - UnitX; // Top Left
                yield return this - UnitY; // Top
                if (X < bounds.X - T.One) yield return this - UnitY + UnitX; // Top Right
            }
            if (X < bounds.X - T.One) yield return this + UnitX; // Right
            if (Y < bounds.Y - T.One)
            {
                if (X < bounds.X - T.One) yield return this + UnitY + UnitX; // Bottom Right
                yield return this + UnitY; // Bottom
                if (X > T.Zero) yield return this + UnitY - UnitX; // Bottom Left

            }
            if (X > T.Zero) yield return this - UnitX; // Left
        }

        public Point2 Sign() => new Point2(T.Sign(X), T.Sign(Y));

        public T Sum() => X + Y;

        public T Product() => X * Y;

        public T Manhattan() => T.Abs(X) + T.Abs(Y);

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
