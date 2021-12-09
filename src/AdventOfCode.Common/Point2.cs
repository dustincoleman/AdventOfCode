using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public struct Point2
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

        public IEnumerable<Point2> Adjacent(Point2 bounds)
        {
            if (X > 0) yield return this - UnitX;
            if (Y > 0) yield return this - UnitY;
            if (X < bounds.X - 1) yield return this + UnitX;
            if (Y < bounds.Y - 1) yield return this + UnitY;
        }

        public static Point2 operator +(Point2 p) => p;
        public static Point2 operator -(Point2 p) => new Point2(-p.X, -p.Y);
        public static Point2 operator +(Point2 left, Point2 right) => new Point2(left.X + right.X, left.Y + right.Y);
        public static Point2 operator -(Point2 left, Point2 right) => new Point2(left.X - right.X, left.Y - right.Y);
    }
}
