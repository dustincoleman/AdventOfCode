using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grid2Visualizer.Remote
{
    [Serializable]
    internal class StringGrid2 : IGrid2
    {
        int boundsX;
        int boundsY;
        string[,] data;

        internal StringGrid2(IGrid2 source)
        {
            this.boundsX = source.Bounds.X;
            this.boundsY = source.Bounds.Y;
            this.data = new string[this.boundsX, this.boundsY];

            foreach (Point2 point in source.Points)
            {
                data[point.X, point.Y] = source[point].ToString();
            }
        }

        public object this[Point2 p] => data[p.X, p.Y];

        public object this[int x, int y] => data[x, y];

        public Point2 Bounds => new Point2(boundsX, boundsY);

        public IEnumerable<Point2> Points => Point2.Quadrant(Bounds);
    }
}
