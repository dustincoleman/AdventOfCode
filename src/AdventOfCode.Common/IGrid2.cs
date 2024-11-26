using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public interface IGrid2
    {
        object this[int x, int y] { get; }

        object this[Point2 p] { get; }

        Point2 Bounds { get; }

        IEnumerable<Point2> AllPoints { get; }
    }
}
