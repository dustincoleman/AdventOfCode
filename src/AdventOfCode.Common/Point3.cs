using System.Numerics;

namespace AdventOfCode.Common
{
    public struct Point3<T> : IEquatable<Point3<T>> where T : INumber<T>
    {
        public static readonly Point3<T> Zero = new Point3<T>(T.Zero, T.Zero, T.Zero);
        public static readonly Point3<T> One = new Point3<T>(T.One, T.One, T.One);
        public static readonly Point3<T> UnitX = new Point3<T>(T.One, T.Zero, T.Zero);
        public static readonly Point3<T> UnitY = new Point3<T>(T.Zero, T.One, T.Zero);
        public static readonly Point3<T> UnitZ = new Point3<T>(T.Zero, T.Zero, T.One);

        public readonly T X;
        public readonly T Y;
        public readonly T Z;

        public Point3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point2<T> XY => (X, Y);
        public Point2<T> XZ => (X, Z);
        public Point2<T> YZ => (Y, Z);

        public static Point3<T> Parse(string input)
        {
            string[] parts = input.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries);
            return new Point3<T>(T.Parse(parts[0], provider: null), T.Parse(parts[1], provider: null), T.Parse(parts[2], provider: null));
        }

        public static Point3<T> Min(Point3<T> left, Point3<T> right)
        {
            return new Point3<T>(T.Min(left.X, right.X), T.Min(left.Y, right.Y), T.Min(left.Z, right.Z));
        }

        public static Point3<T> Max(Point3<T> left, Point3<T> right)
        {
            return new Point3<T>(T.Max(left.X, right.X), T.Max(left.Y, right.Y), T.Max(left.Z, right.Z));
        }

        public T Sum() => checked(X + Y + Z);

        public T Product() => checked(X * Y * Z);

        public T Manhattan() => checked(T.Abs(X) + T.Abs(Y) + T.Abs(Z));

        public bool IsUniform() => (X == Y && Y == Z);

        public Point3<T> Cross(Point3<T> other)
        {
            return new Point3<T>(
                x: checked(this.Y * other.Z - this.Z * other.Y),
                y: checked(this.Z * other.X - this.X * other.Z),
                z: checked(this.X * other.Y - this.Y * other.X));
        }

        public void Deconstruct(out T x, out T y, out T z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public bool Equals(Point3<T> other) => (this == other);

        public override bool Equals(object obj) => (obj is Point3<T> other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public Point3<U> As<U>() where U : INumber<U> => new Point3<U>(U.CreateChecked(X), U.CreateChecked(Y), U.CreateChecked(Z));

        public static Point3<T> operator +(Point3<T> p) => p;
        public static Point3<T> operator -(Point3<T> p) => new Point3<T>(-p.X, -p.Y, -p.Z);
        public static Point3<T> operator +(Point3<T> p, T i) => new Point3<T>(p.X + i, p.Y + i, p.Z + i);
        public static Point3<T> operator +(Point3<T> left, Point3<T> right) => new Point3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public static Point3<T> operator -(Point3<T> p, T i) => new Point3<T>(p.X - i, p.Y - i, p.Z - i);
        public static Point3<T> operator -(Point3<T> left, Point3<T> right) => new Point3<T>(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        public static Point3<T> operator *(Point3<T> p, T i) => new Point3<T>(p.X * i, p.Y * i, p.Z * i);
        public static Point3<T> operator *(Point3<T> left, Point3<T> right) => new Point3<T>(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        public static Point3<T> operator /(Point3<T> p, T i) => new Point3<T>(p.X / i, p.Y / i, p.Z / i);
        public static Point3<T> operator /(Point3<T> left, Point3<T> right) => new Point3<T>(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        public static Point3<T> operator %(Point3<T> p, T i) => new Point3<T>(p.X % i, p.Y % i, p.Z % i);
        public static Point3<T> operator %(Point3<T> left, Point3<T> right) => new Point3<T>(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
        public static bool operator ==(Point3<T> left, Point3<T> right) => (left.X == right.X && left.Y == right.Y && left.Z == right.Z);
        public static bool operator !=(Point3<T> left, Point3<T> right) => !(left == right);
        public static bool operator <(Point3<T> left, Point3<T> right) => (left.X < right.X && left.Y < right.Y && left.Z < right.Z);
        public static bool operator <=(Point3<T> left, Point3<T> right) => (left.X <= right.X && left.Y <= right.Y && left.Z <= right.Z);
        public static bool operator >(Point3<T> left, Point3<T> right) => (left.X > right.X && left.Y > right.Y && left.Z > right.Z);
        public static bool operator >=(Point3<T> left, Point3<T> right) => (left.X >= right.X && left.Y >= right.Y && left.Z >= right.Z);

        public static implicit operator (T X, T Y, T Z)(Point3<T> point) => (point.X, point.Y, point.Z);
        public static implicit operator Point3<T>((T X, T Y, T Z) tuple) => new Point3<T>(tuple.X, tuple.Y, tuple.Z);
    }
}
