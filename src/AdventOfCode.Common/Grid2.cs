using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AdventOfCode.Common
{
    public class Grid2<T> : IEnumerable<T>, IEquatable<Grid2<T>>
    {
        private readonly T[,] grid;

        public Grid2(int xBound, int yBound)
            : this(new Point2(xBound, yBound))
        {
        }

        public Grid2(Point2 bounds)
        {
            this.grid = new T[bounds.X, bounds.Y];
            Bounds = bounds;
        }

        public T this[Point2 point]
        {
            get => this.grid[point.X, point.Y];
            set => this.grid[point.X, point.Y] = value;
        }

        public Point2 Bounds { get; }

        public IEnumerable<Point2> Points => Point2.Quadrant(Bounds);

        public static Grid2<T> Combine(Grid2<Grid2<T>> pieces)
        {
            Point2 pieceBounds = pieces[Point2.Zero].Bounds;

            if (pieces.Where(p => p.Bounds != pieceBounds).Any())
            {
                throw new ArgumentException("Pieces must be same size");
            }

            Grid2<T> output = new Grid2<T>(pieces.Bounds * pieceBounds);

            foreach (Point2 outerPoint in Point2.Quadrant(pieces.Bounds))
            {
                foreach (Point2 innerPoint in Point2.Quadrant(pieceBounds))
                {
                    output[(outerPoint * pieceBounds) + innerPoint] = pieces[outerPoint][innerPoint];
                }
            }

            return output;
        }

        public IEnumerable<T> Adjacent(Point2 point)
        {
            foreach (Point2 adjacentPoint in point.Adjacent(Bounds))
            {
                yield return this.grid[adjacentPoint.X, adjacentPoint.Y];
            }
        }

        public IEnumerable<Grid2<T>> Permutations()
        {
            Grid2<T> current;

            yield return (current = this);
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());

            yield return (current = this.FlipHorizontal());
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());

            yield return (current = this.FlipVertical());
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());
            yield return (current = current.Rotate());
        }

        public Grid2<Grid2<T>> Split(Point2 pieceSize)
        {
            if (Bounds % pieceSize != Point2.Zero)
            {
                throw new ArgumentException("Invalid piece size");
            }

            Grid2<Grid2<T>> pieces = new Grid2<Grid2<T>>(Bounds / pieceSize);

            foreach(Point2 point in Point2.Quadrant(pieces.Bounds))
            {
                pieces[point] = SubGrid(point * pieceSize, pieceSize);
            }

            return pieces;
        }

        public Grid2<T> SubGrid(Point2 origin, Point2 size)
        {
            Grid2<T> subGrid = new Grid2<T>(size);

            foreach (Point2 point in Point2.Quadrant(size))
            {
                subGrid[point] = this[origin + point];
            }

            return subGrid;
        }

        public Grid2<T> FlipHorizontal() => Transpose(Bounds, source => new Point2(source.X, Bounds.Y - source.Y - 1));

        public Grid2<T> FlipVertical() => Transpose(Bounds, source => new Point2(Bounds.X - source.X - 1, source.Y));

        public Grid2<T> Rotate() => Transpose(~Bounds, source => new Point2(Bounds.Y - source.Y - 1, source.X));

        public Grid2<T> Transpose(Point2 newBounds, Func<Point2, Point2> computeDestination)
        {
            Grid2<T> output = new Grid2<T>(newBounds);

            foreach (Point2 source in Point2.Quadrant(Bounds))
            {
                Point2 destination = computeDestination(source);
                output[destination] = this[source];
            }

            return output;
        }

        public IEnumerator<T> GetEnumerator() => this.grid.Cast<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public bool Equals([AllowNull] Grid2<T> other) => (this == other);

        public override bool Equals(object obj) => (obj is Grid2<T> other && this.Equals(other));

        public override int GetHashCode()
        {
            int shift = 0, hashcode = 0;

            foreach (T item in this)
            {
                hashcode ^= (item.GetHashCode() << (shift++ % 16));
            }

            return hashcode;
        }

        public static bool operator ==(Grid2<T> left, Grid2<T> right)
        {
            if (left is null && right is null) return true;

            if (left is null || right is null || left.Bounds != right.Bounds) return false;

            foreach (Point2 point in Point2.Quadrant(left.Bounds))
            {
                if (!EqualityComparer<T>.Default.Equals(left[point], right[point]))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Grid2<T> left, Grid2<T> right) => !(left == right);
    }
}
