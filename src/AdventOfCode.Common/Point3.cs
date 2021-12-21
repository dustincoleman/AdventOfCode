using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public struct Point3 : IEquatable<Point3>
    {
        public static readonly Point3 Zero = new Point3(0, 0, 0);
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

        public Point3 Orient(Orientation3 orientation)
        {
            switch (orientation)
            {
                case Orientation3.PositiveX1:
                    return this.Rotate(UnitX * 0);
                case Orientation3.PositiveX2:
                    return this.Rotate(UnitX * 1);
                case Orientation3.PositiveX3:
                    return this.Rotate(UnitX * 2);
                case Orientation3.PositiveX4:
                    return this.Rotate(UnitX * 3);

                case Orientation3.PositiveY1:
                    return this.Rotate(UnitZ * 3).Rotate(UnitY * 0);
                case Orientation3.PositiveY2:
                    return this.Rotate(UnitZ * 3).Rotate(UnitY * 1);
                case Orientation3.PositiveY3:
                    return this.Rotate(UnitZ * 3).Rotate(UnitY * 2);
                case Orientation3.PositiveY4:
                    return this.Rotate(UnitZ * 3).Rotate(UnitY * 3);

                case Orientation3.PositiveZ1:
                    return this.Rotate(UnitY * 1).Rotate(UnitZ * 0);
                case Orientation3.PositiveZ2:
                    return this.Rotate(UnitY * 1).Rotate(UnitZ * 1);
                case Orientation3.PositiveZ3:
                    return this.Rotate(UnitY * 1).Rotate(UnitZ * 2);
                case Orientation3.PositiveZ4:
                    return this.Rotate(UnitY * 1).Rotate(UnitZ * 3);

                case Orientation3.NegativeX1:
                    return this.Rotate(UnitY * 2).Rotate(UnitX * 0);
                case Orientation3.NegativeX2:
                    return this.Rotate(UnitY * 2).Rotate(UnitX * 1);
                case Orientation3.NegativeX3:
                    return this.Rotate(UnitY * 2).Rotate(UnitX * 2);
                case Orientation3.NegativeX4:
                    return this.Rotate(UnitY * 2).Rotate(UnitX * 3);

                case Orientation3.NegativeY1:
                    return this.Rotate(UnitZ * 1).Rotate(UnitY * 0);
                case Orientation3.NegativeY2:
                    return this.Rotate(UnitZ * 1).Rotate(UnitY * 1);
                case Orientation3.NegativeY3:
                    return this.Rotate(UnitZ * 1).Rotate(UnitY * 2);
                case Orientation3.NegativeY4:
                    return this.Rotate(UnitZ * 1).Rotate(UnitY * 3);

                case Orientation3.NegativeZ1:
                    return this.Rotate(UnitY * 3).Rotate(UnitZ * 0);
                case Orientation3.NegativeZ2:
                    return this.Rotate(UnitY * 3).Rotate(UnitZ * 1);
                case Orientation3.NegativeZ3:
                    return this.Rotate(UnitY * 3).Rotate(UnitZ * 2);
                case Orientation3.NegativeZ4:
                    return this.Rotate(UnitY * 3).Rotate(UnitZ * 3);

                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation));
            }
        }

        public Point3 RotateX() => new Point3(X, -Z, Y);

        public Point3 RotateY() => new Point3(-Z, Y, X);

        public Point3 RotateZ() => new Point3(Y, -X, Z);

        public Point3 Rotate(Point3 times)
        {
            Point3 point3 = this;

            for (int x = 0; x < times.X; x++)
            {
                point3 = point3.RotateX();
            }

            for (int y = 0; y < times.Y; y++)
            {
                point3 = point3.RotateY();
            }

            for (int z = 0; z < times.Z; z++)
            {
                point3 = point3.RotateZ();
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
    }
}
