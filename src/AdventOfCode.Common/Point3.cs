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

        public static IEnumerable<Point3<T>> GetPointsInRange(Point3<T> upper)
        {
            return GetPointsInRange(Zero, upper);
        }

        public static IEnumerable<Point3<T>> GetPointsInRange(Point3<T> lower, Point3<T> upper)
        {
            for (T z = lower.Z; z <= upper.Z; z++)
            {
                for (T y = lower.Y; y <= upper.Y; y++)
                {
                    for (T x = lower.X; x <= upper.X; x++)
                    {
                        yield return new Point3<T>(x, y, z);
                    }
                }
            }
        }

        public static Point3<T> Min(Point3<T> left, Point3<T> right)
        {
            return new Point3<T>(T.Min(left.X, right.X), T.Min(left.Y, right.Y), T.Min(left.Z, right.Z));
        }

        public static Point3<T> Max(Point3<T> left, Point3<T> right)
        {
            return new Point3<T>(T.Max(left.X, right.X), T.Max(left.Y, right.Y), T.Max(left.Z, right.Z));
        }

        public T Sum() => X + Y + Z;

        public T Product() => X * Y * Z;

        public T Manhattan() => T.Abs(X) + T.Abs(Y) + T.Abs(Z);

        public bool IsUniform() => (X == Y && Y == Z);

        public IEnumerable<Point3<T>> Adjacent()
        {
            yield return this - UnitX;
            yield return this - UnitY;
            yield return this - UnitZ;
            yield return this + UnitX;
            yield return this + UnitY;
            yield return this + UnitZ;
        }

        public Point3<T> Orient(Orientation3 orientation)
        {
            switch (orientation)
            {
                case Orientation3.PositiveX1:
                    return this;
                case Orientation3.PositiveX2:
                    return this.RotateXClockwise();
                case Orientation3.PositiveX3:
                    return this.RotateX180Degrees();
                case Orientation3.PositiveX4:
                    return this.RotateXCounterclockwise();

                case Orientation3.PositiveY1:
                    return this.RotateZCounterclockwise();
                case Orientation3.PositiveY2:
                    return this.RotateZCounterclockwise().RotateYClockwise();
                case Orientation3.PositiveY3:
                    return this.RotateZCounterclockwise().RotateY180Degrees();
                case Orientation3.PositiveY4:
                    return this.RotateZCounterclockwise().RotateYCounterclockwise();

                case Orientation3.PositiveZ1:
                    return this.RotateYClockwise();
                case Orientation3.PositiveZ2:
                    return this.RotateYClockwise().RotateZClockwise();
                case Orientation3.PositiveZ3:
                    return this.RotateYClockwise().RotateZ180Degrees();
                case Orientation3.PositiveZ4:
                    return this.RotateYClockwise().RotateZCounterclockwise();

                case Orientation3.NegativeX1:
                    return this.RotateY180Degrees();
                case Orientation3.NegativeX2:
                    return this.RotateY180Degrees().RotateXClockwise();
                case Orientation3.NegativeX3:
                    return this.RotateY180Degrees().RotateX180Degrees();
                case Orientation3.NegativeX4:
                    return this.RotateY180Degrees().RotateXCounterclockwise();

                case Orientation3.NegativeY1:
                    return this.RotateZClockwise();
                case Orientation3.NegativeY2:
                    return this.RotateZClockwise().RotateYClockwise();
                case Orientation3.NegativeY3:
                    return this.RotateZClockwise().RotateY180Degrees();
                case Orientation3.NegativeY4:
                    return this.RotateZClockwise().RotateYCounterclockwise();

                case Orientation3.NegativeZ1:
                    return this.RotateYCounterclockwise();
                case Orientation3.NegativeZ2:
                    return this.RotateYCounterclockwise().RotateZClockwise();
                case Orientation3.NegativeZ3:
                    return this.RotateYCounterclockwise().RotateZ180Degrees();
                case Orientation3.NegativeZ4:
                    return this.RotateYCounterclockwise().RotateZCounterclockwise();

                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation));
            }
        }

        public Point3<T> RotateXClockwise() => new Point3<T>(X, -Z, Y);
        public Point3<T> RotateXCounterclockwise() => new Point3<T>(X, Z, -Y);
        public Point3<T> RotateX180Degrees() => new Point3<T>(X, -Y, -Z);
        public Point3<T> RotateYClockwise() => new Point3<T>(-Z, Y, X);
        public Point3<T> RotateYCounterclockwise() => new Point3<T>(Z, Y, -X);
        public Point3<T> RotateY180Degrees() => new Point3<T>(-X, Y, -Z);
        public Point3<T> RotateZClockwise() => new Point3<T>(Y, -X, Z);
        public Point3<T> RotateZCounterclockwise() => new Point3<T>(-Y, X, Z);
        public Point3<T> RotateZ180Degrees() => new Point3<T>(-X, -Y, Z);

        public Point3<T> Rotate(Point3<T> times)
        {
            Point3<T> point3 = this;

            for (T x = T.Zero; x < times.X; x++)
            {
                point3 = point3.RotateXClockwise();
            }

            for (T y = T.Zero; y < times.Y; y++)
            {
                point3 = point3.RotateYClockwise();
            }

            for (T z = T.Zero; z < times.Z; z++)
            {
                point3 = point3.RotateZClockwise();
            }

            return point3;
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
