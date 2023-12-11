using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode.Common
{
    public struct Point2Big : IEquatable<Point2Big>
    {
        public static readonly Point2Big Zero = new Point2Big(0, 0);
        public static readonly Point2Big UnitX = new Point2Big(1, 0);
        public static readonly Point2Big UnitY = new Point2Big(0, 1);

        public readonly long X;
        public readonly long Y;

        public Point2Big(long x, long y)
        {
            X = x;
            Y = y;
        }

        public long Manhattan() => Math.Abs(X) + Math.Abs(Y);

        public bool Equals(Point2Big other) => (this == other);

        public override bool Equals(object obj) => (obj is Point2 other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public long Sum() => X + Y;
        public long Product() => X * Y;

        public static Point2Big operator +(Point2Big p) => p;
        public static Point2Big operator -(Point2Big p) => new Point2Big(-p.X, -p.Y);
        public static Point2Big operator ~(Point2Big p) => new Point2Big(p.Y, p.X);
        public static Point2Big operator +(Point2Big p, long i) => new Point2Big(p.X + i, p.Y + i);
        public static Point2Big operator +(Point2Big left, Point2Big right) => new Point2Big(left.X + right.X, left.Y + right.Y);
        public static Point2Big operator -(Point2Big p, long i) => new Point2Big(p.X - i, p.Y - i);
        public static Point2Big operator -(Point2Big left, Point2Big right) => new Point2Big(left.X - right.X, left.Y - right.Y);
        public static Point2Big operator *(Point2Big p, long i) => new Point2Big(p.X * i, p.Y * i);
        public static Point2Big operator *(Point2Big left, Point2Big right) => new Point2Big(left.X * right.X, left.Y * right.Y);
        public static Point2Big operator /(Point2Big p, long i) => new Point2Big(p.X / i, p.Y / i);
        public static Point2Big operator /(Point2Big left, Point2Big right) => new Point2Big(left.X / right.X, left.Y / right.Y);
        public static Point2Big operator %(Point2Big p, long i) => new Point2Big(p.X % i, p.Y % i);
        public static Point2Big operator %(Point2Big left, Point2Big right) => new Point2Big(left.X % right.X, left.Y % right.Y);
        public static bool operator ==(Point2Big left, Point2Big right) => (left.X == right.X && left.Y == right.Y);
        public static bool operator !=(Point2Big left, Point2Big right) => !(left == right);
        public static bool operator <(Point2Big left, Point2Big right) => (left.X < right.X && left.Y < right.Y);
        public static bool operator >(Point2Big left, Point2Big right) => (left.X > right.X && left.Y > right.Y);
        public static bool operator <=(Point2Big left, Point2Big right) => (left.X <= right.X && left.Y <= right.Y);
        public static bool operator >=(Point2Big left, Point2Big right) => (left.X >= right.X && left.Y >= right.Y);
    }
}
