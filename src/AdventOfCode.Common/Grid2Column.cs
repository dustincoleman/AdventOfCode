﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace AdventOfCode.Common
{
    public class Grid2Column<T> : IEnumerable<T>, IReadOnlyList<T>, IEquatable<Grid2Column<T>>
    {
        public Grid2Column(Grid2<T> grid, int index)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            Index = (index >= 0 && index <= Grid.Bounds.X) ? index : throw new ArgumentOutOfRangeException(nameof(index));
        }

        public Grid2<T> Grid { get; }

        public IEnumerable<Point2> AllPoints => Points.Line(new Point2(Index, 0), new Point2(Index, Grid.Bounds.Y - 1));

        public int Index { get; }

        public int Count => Grid.Bounds.Y;

        public T this[int y]
        {
            get => Grid[Index, y];
            set => Grid[Index, y] = value;
        }

        public Point2 Point(int y) => new Point2(Index, y);

        public IEnumerable<T> Adjacent(int y) => Grid.Adjacent(Point(y));

        public IEnumerable<Point2> AdjacentPoints(int y) => Grid.AdjacentPoints(Point(y));

        public IEnumerable<T> Surrounding(int y) => Grid.Surrounding(Point(y));

        public IEnumerable<Point2> SurroundingPoints(int y) => Grid.SurroundingPoints(Point(y));

        public IEnumerator<T> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> Enumerate()
        {
            for (int y = 0; y < Grid.Bounds.Y; y++)
            {
                yield return Grid[Index, y];
            }
        }

        public bool Equals(Grid2Column<T> other)
        {
            if (other == null)
            {
                return false;
            }

            bool equal = Count.Equals(other.Count);

            for (int y = 0; equal && y < Count; y++)
            {
                equal = EqualityComparer<T>.Default.Equals(this[y], other[y]);
            }

            return equal;
        }
    }

    public class Grid2ColumnCollection<T> : IEnumerable<Grid2Column<T>>, IReadOnlyList<Grid2Column<T>>
    {
        public Grid2ColumnCollection(Grid2<T> grid)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
        }

        public Grid2Column<T> this[int index] => new Grid2Column<T>(Grid, index);

        public Grid2<T> Grid { get; }

        public int Count => Grid.Bounds.X;

        public IEnumerator<Grid2Column<T>> GetEnumerator() => Enumerate().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<Grid2Column<T>> Enumerate()
        {
            for (int x = 0; x < Grid.Bounds.X; x++)
            {
                yield return new Grid2Column<T>(Grid, x);
            }
        }
    }
}
