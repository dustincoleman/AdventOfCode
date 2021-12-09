using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Grid2<T>
    {
        private readonly T[,] grid;

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

        public IEnumerable<T> Adjacent(Point2 point)
        {
            foreach (Point2 adjacentPoint in point.Adjacent(Bounds))
            {
                yield return this.grid[adjacentPoint.X, adjacentPoint.Y];
            }
        }
    }
}
