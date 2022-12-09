using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode.Common
{
    public class Grid2Row<T> : IEnumerable<T>
    {
        public Grid2Row(Grid2<T> grid, int index)
        {
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            Index = (index >= 0 && index <= Grid.Bounds.X) ? index : throw new ArgumentOutOfRangeException(nameof(index));
        }

        public Grid2<T> Grid { get; }

        public int Index { get; }

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
