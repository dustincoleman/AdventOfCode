using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public struct Point3 : IEquatable<Point3>
    {
        public static readonly Point3 Zero = new Point3(0, 0, 0);
        public static readonly Point3 One = new Point3(1, 1, 1);
        public static readonly Point3 UnitX = new Point3(1, 0, 0);
        public static readonly Point3 UnitY = new Point3(0, 1, 0);
        public static readonly Point3 UnitZ = new Point3(0, 0, 1);

        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static IEnumerable<Point3> GetPointsInRange(Point3 upper)
        {
            return GetPointsInRange(Zero, upper);
        }

        public static IEnumerable<Point3> GetPointsInRange(Point3 lower, Point3 upper)
        {
            for (int z = lower.Z; z <= upper.Z; z++)
            {
                for (int y = lower.Y; y <= upper.Y; y++)
                {
                    for (int x = lower.X; x <= upper.X; x++)
                    {
                        yield return new Point3(x, y, z);
                    }
                }
            }
        }

        public static Point3 Min(Point3 left, Point3 right)
        {
            return new Point3(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y), Math.Min(left.Z, right.Z));
        }

        public static Point3 Max(Point3 left, Point3 right)
        {
            return new Point3(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y), Math.Max(left.Z, right.Z));
        }

        public int Sum() => X + Y + Z;

        public long Product() => (long)X * (long)Y * (long)Z;

        public IEnumerable<Point3> Adjacent()
        {
            yield return this - UnitX;
            yield return this - UnitY;
            yield return this - UnitZ;
            yield return this + UnitX;
            yield return this + UnitY;
            yield return this + UnitZ;
        }

        public Point3 Orient(Orientation3 orientation)
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

        public Point3 RotateXClockwise() => new Point3(X, -Z, Y);
        public Point3 RotateXCounterclockwise() => new Point3(X, Z, -Y);
        public Point3 RotateX180Degrees() => new Point3(X, -Y, -Z);
        public Point3 RotateYClockwise() => new Point3(-Z, Y, X);
        public Point3 RotateYCounterclockwise() => new Point3(Z, Y, -X);
        public Point3 RotateY180Degrees() => new Point3(-X, Y, -Z);
        public Point3 RotateZClockwise() => new Point3(Y, -X, Z);
        public Point3 RotateZCounterclockwise() => new Point3(-Y, X, Z);
        public Point3 RotateZ180Degrees() => new Point3(-X, -Y, Z);

        public Point3 Rotate(Point3 times)
        {
            Point3 point3 = this;

            for (int x = 0; x < times.X; x++)
            {
                point3 = point3.RotateXClockwise();
            }

            for (int y = 0; y < times.Y; y++)
            {
                point3 = point3.RotateYClockwise();
            }

            for (int z = 0; z < times.Z; z++)
            {
                point3 = point3.RotateZClockwise();
            }

            return point3;
        }

        public int Manhattan() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public bool Equals(Point3 other) => (this == other);

        public override bool Equals(object obj) => (obj is Point3 other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static Point3 operator +(Point3 p) => p;
        public static Point3 operator -(Point3 p) => new Point3(-p.X, -p.Y, -p.Z);
        public static Point3 operator +(Point3 p, int i) => new Point3(p.X + i, p.Y + i, p.Z + i);
        public static Point3 operator +(Point3 left, Point3 right) => new Point3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        public static Point3 operator -(Point3 p, int i) => new Point3(p.X - i, p.Y - i, p.Z - i);
        public static Point3 operator -(Point3 left, Point3 right) => new Point3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        public static Point3 operator *(Point3 p, int i) => new Point3(p.X * i, p.Y * i, p.Z * i);
        public static Point3 operator *(Point3 left, Point3 right) => new Point3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        public static Point3 operator /(Point3 p, int i) => new Point3(p.X / i, p.Y / i, p.Z / i);
        public static Point3 operator /(Point3 left, Point3 right) => new Point3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        public static Point3 operator %(Point3 p, int i) => new Point3(p.X % i, p.Y % i, p.Z % i);
        public static Point3 operator %(Point3 left, Point3 right) => new Point3(left.X % right.X, left.Y % right.Y, left.Z % right.Z);
        public static bool operator ==(Point3 left, Point3 right) => (left.X == right.X && left.Y == right.Y && left.Z == right.Z);
        public static bool operator !=(Point3 left, Point3 right) => !(left == right);
        public static bool operator <(Point3 left, Point3 right) => (left.X < right.X && left.Y < right.Y && left.Z < right.Z);
        public static bool operator <=(Point3 left, Point3 right) => (left.X <= right.X && left.Y <= right.Y && left.Z <= right.Z);
        public static bool operator >(Point3 left, Point3 right) => (left.X > right.X && left.Y > right.Y && left.Z > right.Z);
        public static bool operator >=(Point3 left, Point3 right) => (left.X >= right.X && left.Y >= right.Y && left.Z >= right.Z);

        public static implicit operator (int X, int Y, int Z)(Point3 point) => (point.X, point.Y, point.Z);
        public static implicit operator Point3((int X, int Y, int Z) tuple) => new Point3(tuple.X, tuple.Y, tuple.Z);
    }
}
