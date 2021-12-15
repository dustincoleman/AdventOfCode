using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    internal class Grid2Enumerator<T> : IEnumerator<T>
    {
        private readonly Grid2<T> grid;
        private readonly IEnumerator<Point2> pointEnumerator;

        internal Grid2Enumerator(Grid2<T> grid)
        {
            this.grid = grid;
            this.pointEnumerator = grid.Points.GetEnumerator();
        }

        public T Current => this.grid[pointEnumerator.Current];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            return this.pointEnumerator.MoveNext();
        }

        public void Reset()
        {
            this.pointEnumerator.Reset();
        }

        public void Dispose()
        {
            this.pointEnumerator.Dispose();
        }
    }
}
