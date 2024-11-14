namespace AdventOfCode.Common
{
    public struct Rect3
    {
        public Rect3(Point3 lower, Point3 upper)
        {
            if (!(lower <= upper)) throw new ArgumentOutOfRangeException();
            this.Lower = lower;
            this.Upper = upper;
        }

        public Point3 Lower { get; }

        public Point3 Upper { get; }

        public long Size => (this.Upper - this.Lower + Point3.One).Product();

        public static Rect3 Normalize(Point3 p1, Point3 p2)
        {
            return new Rect3(
                new Point3(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y), Math.Min(p1.Z, p2.Z)),
                new Point3(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y), Math.Max(p1.Z, p2.Z)));
        }

        internal bool Contains(Rect3 other)
        {
            return (this.Lower <= other.Lower && this.Upper >= other.Upper);
        }

        internal bool Intersects(Rect3 other, out Rect3 intersection)
        {
            Point3 lowerMax = Point3.Max(this.Lower, other.Lower);
            Point3 upperMin = Point3.Min(this.Upper, other.Upper);

            if (upperMin >= lowerMax)
            {
                intersection = new Rect3(lowerMax, upperMin);
                return true;
            }

            intersection = default;
            return false;
        }

        internal IEnumerable<Rect3> Without(Rect3 subregion)
        {
            if (!this.Contains(subregion)) throw new ArgumentOutOfRangeException();

            Point3[] ranges = new Point3[6];

            ranges[0] = this.Lower;
            ranges[1] = subregion.Lower - 1;
            ranges[2] = subregion.Lower;
            ranges[3] = subregion.Upper;
            ranges[4] = subregion.Upper + 1;
            ranges[5] = this.Upper;

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
                            yield return new Rect3(lowerBounds, upperBounds);
                        }
                    }
                }
            }
        }
    }
}
