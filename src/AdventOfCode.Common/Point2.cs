using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode.Common
{
    public struct Point2 : IEquatable<Point2>
    {
        public static readonly Point2 Zero = new Point2(0, 0);
        public static readonly Point2 UnitX = new Point2(1, 0);
        public static readonly Point2 UnitY = new Point2(0, 1);

        public readonly int X;
        public readonly int Y;

        public Point2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static IEnumerable<Point2> Quadrant(Point2 bounds)
        {
            int xSign = (bounds.X >= 0) ? 1 : -1;
            int ySign = (bounds.Y >= 0) ? 1 : -1;

            for (int y = 0; y < bounds.Y; y++)
            {
                for (int x = 0; x < bounds.X; x++)
                {
                    yield return new Point2(x * xSign, y * ySign);
                }
            }
        }

        public static Point2 Min(Point2 left, Point2 right)
        {
            return new Point2(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
        }

        public static Point2 Max(Point2 left, Point2 right)
        {
            return new Point2(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
        }

        public IEnumerable<Point2> Adjacent(Point2 bounds)
        {
            if (X > 0) yield return this - UnitX;
            if (Y > 0) yield return this - UnitY;
            if (X < bounds.X - 1) yield return this + UnitX;
            if (Y < bounds.Y - 1) yield return this + UnitY;
        }

        public IEnumerable<Point2> Surrounding(Point2 bounds)
        {
            if (Y > 0)
            {
                if (X > 0) yield return this - UnitY - UnitX; // Top Left
                yield return this - UnitY; // Top
                if (X < bounds.X - 1) yield return this - UnitY + UnitX; // Top Right
            }
            if (X < bounds.X - 1) yield return this + UnitX; // Right
            if (Y < bounds.Y - 1)
            {
                if (X < bounds.X - 1) yield return this + UnitY + UnitX; // Bottom Right
                yield return this + UnitY; // Bottom
                if (X > 0) yield return this + UnitY - UnitX; // Bottom Left

            }
            if (X > 0) yield return this - UnitX; // Left
        }

        public Point2 Sign() => new Point2(Math.Sign(X), Math.Sign(Y));

        public int Manhattan() => Math.Abs(X) + Math.Abs(Y);

        public bool Equals(Point2 other) => (this == other);

        public override bool Equals(object obj) => (obj is Point2 other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public int Sum() => X + Y;
        public int Product() => X * Y;

        public static Point2 operator +(Point2 p) => p;
        public static Point2 operator -(Point2 p) => new Point2(-p.X, -p.Y);
        public static Point2 operator ~(Point2 p) => new Point2(p.Y, p.X);
        public static Point2 operator +(Point2 p, int i) => new Point2(p.X + i, p.Y + i);
        public static Point2 operator +(Point2 left, Point2 right) => new Point2(left.X + right.X, left.Y + right.Y);
        public static Point2 operator -(Point2 p, int i) => new Point2(p.X - i, p.Y - i);
        public static Point2 operator -(Point2 left, Point2 right) => new Point2(left.X - right.X, left.Y - right.Y);
        public static Point2 operator *(Point2 p, int i) => new Point2(p.X * i, p.Y * i);
        public static Point2 operator *(Point2 left, Point2 right) => new Point2(left.X * right.X, left.Y * right.Y);
        public static Point2 operator /(Point2 p, int i) => new Point2(p.X / i, p.Y / i);
        public static Point2 operator /(Point2 left, Point2 right) => new Point2(left.X / right.X, left.Y / right.Y);
        public static Point2 operator %(Point2 p, int i) => new Point2(p.X % i, p.Y % i);
        public static Point2 operator %(Point2 left, Point2 right) => new Point2(left.X % right.X, left.Y % right.Y);
        public static bool operator ==(Point2 left, Point2 right) => (left.X == right.X && left.Y == right.Y);
        public static bool operator !=(Point2 left, Point2 right) => !(left == right);
        public static bool operator <(Point2 left, Point2 right) => (left.X < right.X && left.Y < right.Y);
        public static bool operator >(Point2 left, Point2 right) => (left.X > right.X && left.Y > right.Y);
        public static bool operator <=(Point2 left, Point2 right) => (left.X <= right.X && left.Y <= right.Y);
        public static bool operator >=(Point2 left, Point2 right) => (left.X >= right.X && left.Y >= right.Y);
    }
}
