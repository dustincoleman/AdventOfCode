﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public static class Point2Extensions
    {
        public static IEnumerable<Point2<T>> Adjacent<T>(this Point2<T> pt) where T : INumber<T>
        {
            yield return pt - Point2<T>.UnitX;
            yield return pt - Point2<T>.UnitY;
            yield return pt + Point2<T>.UnitX;
            yield return pt + Point2<T>.UnitY;
        }

        public static IEnumerable<Point2<T>> Adjacent<T>(this Point2<T> pt, Point2<T> bounds) where T : INumber<T>
        {
            if (pt.X > T.Zero) yield return pt - Point2<T>.UnitX;
            if (pt.Y > T.Zero) yield return pt - Point2<T>.UnitY;
            if (pt.X < bounds.X - T.One) yield return pt + Point2<T>.UnitX;
            if (pt.Y < bounds.Y - T.One) yield return pt + Point2<T>.UnitY;
        }

        public static IEnumerable<Point2<T>> Surrounding<T>(this Point2<T> pt) where T : INumber<T>
        {
            yield return pt - Point2<T>.UnitY - Point2<T>.UnitX; // Top Left
            yield return pt - Point2<T>.UnitY; // Top
            yield return pt - Point2<T>.UnitY + Point2<T>.UnitX; // Top Right
            yield return pt + Point2<T>.UnitX; // Right
            yield return pt + Point2<T>.UnitY + Point2<T>.UnitX; // Bottom Right
            yield return pt + Point2<T>.UnitY; // Bottom
            yield return pt + Point2<T>.UnitY - Point2<T>.UnitX; // Bottom Left
            yield return pt - Point2<T>.UnitX; // Left
        }

        public static IEnumerable<Point2<T>> Surrounding<T>(this Point2<T> pt, Point2<T> bounds) where T : INumber<T>
        {
            if (pt.Y > T.Zero)
            {
                if (pt.X > T.Zero) yield return pt - Point2<T>.UnitY - Point2<T>.UnitX; // Top Left
                yield return pt - Point2<T>.UnitY; // Top
                if (pt.X < bounds.X - T.One) yield return pt - Point2<T>.UnitY + Point2<T>.UnitX; // Top Right
            }
            if (pt.X < bounds.X - T.One) yield return pt + Point2<T>.UnitX; // Right
            if (pt.Y < bounds.Y - T.One)
            {
                if (pt.X < bounds.X - T.One) yield return pt + Point2<T>.UnitY + Point2<T>.UnitX; // Bottom Right
                yield return pt + Point2<T>.UnitY; // Bottom
                if (pt.X > T.Zero) yield return pt + Point2<T>.UnitY - Point2<T>.UnitX; // Bottom Left

            }
            if (pt.X > T.Zero) yield return pt - Point2<T>.UnitX; // Left
        }

        public static IEnumerable<Point2<T>> Reachable<T>(this Point2<T> pt, T manhattanDistance) where T : INumber<T>
        {
            T top = pt.Y - manhattanDistance;
            T bottom = pt.Y + manhattanDistance;

            for (T y = top; y <= bottom; y++)
            {
                T remainingDistance = manhattanDistance - T.Abs(pt.Y - y);
                T left = pt.X - remainingDistance;
                T right = pt.X + remainingDistance;

                for (T x = left; x <= right; x++)
                {
                    if (x != pt.X || y != pt.Y)
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        public static IEnumerable<Point2<T>> Reachable<T>(this Point2<T> pt, T manhattanDistance, Point2<T> bounds) where T : INumber<T>
        {
            T top = T.Max(T.Zero, pt.Y - manhattanDistance);
            T bottom = T.Min(bounds.Y - T.One, pt.Y + manhattanDistance);

            for (T y = top; y <= bottom; y++)
            {
                T remainingDistance = manhattanDistance - T.Abs(pt.Y - y);
                T left = T.Max(T.Zero, pt.X - remainingDistance);
                T right = T.Min(bounds.X - T.One, pt.X + remainingDistance);

                for (T x = left; x <= right; x++)
                {
                    if (x != pt.X || y != pt.Y)
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        public static IEnumerable<Point2<T>> LineTo<T>(this Point2<T> pt, Point2<T> other) where T : INumber<T>
        {
            if (pt.X != other.X && pt.Y != other.Y)
            {
                throw new InvalidOperationException("Only supported for vertical and horizontal lines");
            }

            Point2<T> increment = pt.SignToward(other);

            yield return pt;

            while (pt != other)
            {
                pt += increment;
                yield return pt;
            }
        }

        public static Point2<T> SignToward<T>(this Point2<T> pt, Point2<T> other) where T : INumber<T>
        {
            return (other - pt).Sign().As<T>();
        }
    }
}
