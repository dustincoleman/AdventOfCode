using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class Point3Set : HashSet<Point3>
    {
        public Point3Set()
        {
        }

        public Point3Set(Point3Set set)
            : base(set)
        {
        }

        public Point3Set Orient(Orientation3 orientation)
        {
            Point3Set newSet = new Point3Set();

            foreach (Point3 point in this)
            {
                newSet.Add(point.Orient(orientation));
            }

            return newSet;
        }

        public Point3Set Shift(Point3 offset)
        {
            Point3Set newSet = new Point3Set();

            foreach (Point3 point in this)
            {
                newSet.Add(point + offset);
            }

            return newSet;
        }
    }
}
