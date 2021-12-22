using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class VirtualGrid3Region<T>
    {
        public VirtualGrid3Region(Point3 lowerBounds, Point3 upperBounds, T value = default(T))
        {
            this.LowerBounds = lowerBounds;
            this.UpperBounds = upperBounds;
            this.Value = value;
        }

        public Point3 LowerBounds { get; }

        public Point3 UpperBounds { get; }

        public T Value { get; set; }

        public long Size => (this.UpperBounds - this.LowerBounds + Point3.One).Product();

        internal bool Contains(VirtualGrid3Region<T> other)
        {
            return (this.LowerBounds.AllLessThanOrEqual(other.LowerBounds) && this.UpperBounds.AllGreaterThanOrEqual(other.UpperBounds));
        }

        internal bool Intersects(VirtualGrid3Region<T> other, out VirtualGrid3Region<T> intersection)
        {
            Point3 lowerBounds = Point3.Max(this.LowerBounds, other.LowerBounds);
            Point3 upperBounds = Point3.Min(this.UpperBounds, other.UpperBounds);

            if (upperBounds.AllGreaterThanOrEqual(lowerBounds))
            {
                intersection = new VirtualGrid3Region<T>(lowerBounds, upperBounds);
                return true;
            }

            intersection = null;
            return false;
        }

        internal IEnumerable<VirtualGrid3Region<T>> Without(VirtualGrid3Region<T> subregion)
        {
            Point3[] ranges = new Point3[6];

            ranges[0] = this.LowerBounds;
            ranges[1] = subregion.LowerBounds - 1;
            ranges[2] = subregion.LowerBounds;
            ranges[3] = subregion.UpperBounds;
            ranges[4] = subregion.UpperBounds + 1;
            ranges[5] = this.UpperBounds;

            for (int z = 0; z < 5; z += 2)
            {
                if (ranges[z].Z > ranges[z + 1].Z)
                {
                    continue;
                }

                for (int y = 0; y < 5; y += 2)
                {
                    if (ranges[y].Y > ranges[y + 1].Y)
                    {
                        continue;
                    }

                    for (int x = 0; x < 5; x += 2)
                    {
                        if (ranges[x].X > ranges[x + 1].X)
                        {
                            continue;
                        }

                        if (x != 2 || y != 2 || z != 2)
                        {
                            Point3 lowerBounds = new Point3(ranges[x].X, ranges[y].Y, ranges[z].Z);
                            Point3 upperBounds = new Point3(ranges[x + 1].X, ranges[y + 1].Y, ranges[z + 1].Z);

                            yield return new VirtualGrid3Region<T>(lowerBounds, upperBounds, Value);
                        }
                    }
                }
            }
        }
    }
}
