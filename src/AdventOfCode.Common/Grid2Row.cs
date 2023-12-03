using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Common
{
    public class Grid2Row<T> : IEnumerable<T>, IReadOnlyList<T>
    {
        public Grid2Row(Grid2<T> grid, int index)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            Index = (index >= 0 && index <= Grid.Bounds.Y) ? index : throw new ArgumentOutOfRangeException(nameof(index));
        }

        public Grid2<T> Grid { get; }

        public IEnumerable<Point2> Points => Point2.Line(new Point2(0, Index), new Point2(Grid.Bounds.X - 1, Index));

        public int Index { get; }

        public int Count => Grid.Bounds.X;

        public T this[int x] => Grid[x, Index];

        public Point2 Point(int x) => new Point2(x, Index);

        public IEnumerable<T> Adjacent(int x) => Grid.Adjacent(Point(x));

        public IEnumerable<Point2> AdjacentPoints(int x) => Grid.AdjacentPoints(Point(x));

        public IEnumerable<T> Surrounding(int x) => Grid.Surrounding(Point(x));

        public IEnumerable<Point2> SurroundingPoints(int x) => Grid.SurroundingPoints(Point(x));

        public IEnumerator<T> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> Enumerate()
        {
            for (int x = 0; x < Grid.Bounds.X; x++)
            {
                yield return Grid[x, Index];
            }
        }
    }

    public class Grid2RowCollection<T> : IEnumerable<Grid2Row<T>>
    {
        public Grid2RowCollection(Grid2<T> grid)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        public Grid2Row<T> this[int index] => new Grid2Row<T>(Grid, index);

        public Grid2<T> Grid { get; }

        public IEnumerator<Grid2Row<T>> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<Grid2Row<T>> Enumerate()
        {
            for (int y = 0; y < Grid.Bounds.Y; y++)
            {
                yield return new Grid2Row<T>(Grid, y);
            }
        }
    }
}
