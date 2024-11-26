
namespace AdventOfCode.Common
{
    public struct Rect2
    {
        public Rect2(Point2 lower, Point2 upper)
        {
            if (!(lower <= upper)) throw new ArgumentOutOfRangeException();
            this.Lower = lower;
            this.Upper = upper;
        }

        public Point2 Lower { get; }

        public Point2 Upper { get; }

        public long Size => (this.Upper - this.Lower).Product();

        public static Rect2 Normalize(Point2 p1, Point2 p2)
        {
            return new Rect2(
                new Point2(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y)),
                new Point2(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y)));
        }

        internal bool Contains(Rect2 other)
        {
            return (this.Lower <= other.Lower && this.Upper >= other.Upper);
        }

        internal bool Intersects(Rect2 other)
        {
            Point2 lowerMax = Point2.Max(this.Lower, other.Lower);
            Point2 upperMin = Point2.Min(this.Upper, other.Upper);
            return (upperMin >= lowerMax);
        }

        internal bool Intersects(Rect2 other, out Rect2 intersection)
        {
            Point2 lowerMax = Point2.Max(this.Lower, other.Lower);
            Point2 upperMin = Point2.Min(this.Upper, other.Upper);

            if (upperMin >= lowerMax)
            {
                intersection = new Rect2(lowerMax, upperMin);
                return true;
            }

            intersection = default;
            return false;
        }

        public IEnumerable<Point2<int>> AllPoints()
        {
            foreach (Point2 shift in Points.All(Upper - Lower))
            {
                yield return (Lower + shift);
            }
        }
    }
}
