using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdventOfCode.Common
{
    public class Grid2<T> : IGrid2, IEnumerable<T>, IEquatable<Grid2<T>>
    {
        private readonly T[,] grid;
        private readonly Func<Point2, Point2> pointTransform;
        private readonly Func<T, T> valueTransform;

        private int? hashcode;

        public Grid2(int xBound, int yBound)
            : this(new Point2(xBound, yBound))
        {
        }

        public Grid2(Point2 bounds)
        {
            this.grid = new T[bounds.X, bounds.Y];
            this.Bounds = bounds;
        }

        private Grid2(T[,] grid, Point2 bounds, Func<Point2, Point2> pointTransform, Func<T, T> valueTransform)
        {
            this.grid = grid;
            this.Bounds = bounds;
            this.pointTransform = pointTransform;
            this.valueTransform = valueTransform;
        }

        public T this[int x, int y]
        {
            get => this[new Point2(x, y)];
            set => this[new Point2(x, y)] = value;
        }

        public T this[Point2 point]
        {
            get
            {
                if (this.pointTransform != null)
                {
                    point = this.pointTransform(point);
                }
                
                if (this.valueTransform != null)
                {
                    return this.valueTransform(this.grid[point.X, point.Y]);
                }

                return this.grid[point.X, point.Y];
            }
            set
            {
                if (this.valueTransform != null)
                {
                    throw new InvalidOperationException("Cannot set values on a transformed grid");
                }

                if (this.pointTransform != null)
                {
                    point = this.pointTransform(point);
                }

                this.grid[point.X, point.Y] = value;
                this.hashcode = null;
            }
        }

        public Point2 Bounds { get; }

        public IEnumerable<Point2> Points => Point2.Quadrant(Bounds);

        public IEnumerable<Point2> EdgePoints => Points.Where(IsEdge);

        public IEnumerable<Point2> InteriorPoints => Points.Where(p => !IsEdge(p));

        public Grid2ColumnCollection<T> Columns => new Grid2ColumnCollection<T>(this);

        public Grid2RowCollection<T> Rows => new Grid2RowCollection<T>(this);

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

        public bool IsEdge(Point2 point)
        {
            return (point.X == 0 || point.X == Bounds.X - 1 || point.Y == 0 || point.Y == Bounds.Y - 1);
        }

        public bool InBounds(Point2 point)
        {
            return (point.X >= 0 && point.X < Bounds.X && point.Y >= 0 && point.Y < Bounds.Y);
        }

        public IEnumerable<T> Adjacent(Point2 point)
        {
            foreach (Point2 adjacentPoint in point.Adjacent(Bounds))
            {
                yield return this.grid[adjacentPoint.X, adjacentPoint.Y];
            }
        }

        public IEnumerable<Point2> AdjacentPoints(Point2 point) => point.Adjacent(Bounds);

        public IEnumerable<T> Surrounding(Point2 point)
        {
            foreach (Point2 surroundingPoint in point.Surrounding(Bounds))
            {
                yield return this.grid[surroundingPoint.X, surroundingPoint.Y];
            }
        }

        public IEnumerable<Point2> SurroundingPoints(Point2 point) => point.Surrounding(Bounds);

        public IEnumerable<T> TraverseLeft(Point2 point)
        {
            int x = point.X;
            while (--x >= 0)
            {
                yield return this.grid[x, point.Y];
            }
        }

        public IEnumerable<T> TraverseRight(Point2 point)
        {
            int x = point.X;
            while (++x < Bounds.X)
            {
                yield return this.grid[x, point.Y];
            }
        }

        public IEnumerable<T> TraverseUp(Point2 point)
        {
            int y = point.Y;
            while (--y >= 0)
            {
                yield return this.grid[point.X, y];
            }
        }

        public IEnumerable<T> TraverseDown(Point2 point)
        {
            int y = point.Y;
            while (++y < Bounds.Y)
            {
                yield return this.grid[point.X, y];
            }
        }

        public Grid2<T> SurroundWith(T value)
        {
            Grid2<T> newGrid = new Grid2<T>(Bounds + 2);

            foreach (Point2 point in newGrid.Points)
            {
                if (newGrid.IsEdge(point))
                {
                    newGrid[point] = value;
                }
                else
                {
                    newGrid[point] = this[point - 1];
                }
            }

            return newGrid;
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

        public Grid2<T> FlipHorizontal() => Transpose(Bounds, view => new Point2(view.X, Bounds.Y - view.Y - 1));

        public Grid2<T> FlipVertical() => Transpose(Bounds, view => new Point2(Bounds.X - view.X - 1, view.Y));

        public Grid2<T> Rotate() => Transpose(~Bounds, view => new Point2(view.Y, Bounds.Y - view.X - 1));

        public Grid2<T> RotateCCW() => Transpose(~Bounds, view => new Point2(Bounds.X - view.Y - 1, view.X));

        public Grid2<T> Rotate180() => Transpose(Bounds, view => new Point2(Bounds.X - view.X - 1, Bounds.Y - view.Y - 1));

        public Grid2<T> Transpose(Point2 newBounds, Func<Point2, Point2> transpose)
        {
            return new Grid2<T>(
                this.grid, 
                newBounds, 
                (this.pointTransform != null) ? p => transpose(this.pointTransform(p)) : transpose, 
                this.valueTransform);
        }

        public Grid2<T> Transform(Func<T, T> transform)
        {
            return new Grid2<T>(
                this.grid, 
                this.Bounds, 
                this.pointTransform,
                (this.valueTransform != null) ? v => transform(this.valueTransform(v)) : transform);
        }

        public IEnumerator<T> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public IEnumerable<T> Enumerate()
        {
            foreach (Point2 p in Points)
            {
                yield return this[p];
            }
        }

        public bool Equals(Grid2<T> other) => (this == other);

        public override bool Equals(object obj) => (obj is Grid2<T> other && this.Equals(other));

        public override int GetHashCode()
        {
            if (!this.hashcode.HasValue)
            {
                int temp = 0;

                foreach (T item in this)
                {
                    temp = HashCode.Combine(temp, item);
                }

                this.hashcode = temp;
            }

            return this.hashcode.Value;
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

        object IGrid2.this[Point2 p] => this[p];

        object IGrid2.this[int x, int y] => this[x, y];
    }
}
