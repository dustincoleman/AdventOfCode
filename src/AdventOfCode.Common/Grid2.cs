using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Grid2
    {
        private readonly int[,] grid;

        public Grid2(Point2 bounds)
        {
            this.grid = new int[bounds.X, bounds.Y];
            Bounds = bounds;
        }

        public int this[Point2 point]
        {
            get => this.grid[point.X, point.Y];
            set => this.grid[point.X, point.Y] = value;
        }

        public Point2 Bounds { get; }

        public IEnumerable<int> Adjacent(Point2 point)
        {
            foreach (Point2 adjacentPoint in point.Adjacent(Bounds))
            {
                yield return this.grid[adjacentPoint.X, adjacentPoint.Y];
            }
        }
    }
}
