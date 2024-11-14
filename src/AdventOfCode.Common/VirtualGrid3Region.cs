using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Common
{
    public class VirtualGrid3Region<T>
    {
        public VirtualGrid3Region(Point3 lowerBounds, Point3 upperBounds, T value = default)
            : this(new Rect3(lowerBounds, upperBounds), value)
        {
        }

        public VirtualGrid3Region(Rect3 bounds, T value = default)
        {
            this.Bounds = bounds;
            this.Value = value;
        }

        public Rect3 Bounds { get; }

        public T Value { get; set; }

        public long Size => this.Bounds.Size;

        internal bool Contains(VirtualGrid3Region<T> other) => this.Bounds.Contains(other.Bounds);

        internal bool Intersects(VirtualGrid3Region<T> other, out VirtualGrid3Region<T> intersection)
        {
            Point3 lowerBounds = Point3.Max(this.Bounds.Lower, other.Bounds.Lower);
            Point3 upperBounds = Point3.Min(this.Bounds.Upper, other.Bounds.Upper);

            if (upperBounds >= lowerBounds)
            {
                intersection = new VirtualGrid3Region<T>(lowerBounds, upperBounds);
                return true;
            }

            intersection = null;
            return false;
        }

        internal IEnumerable<VirtualGrid3Region<T>> Without(Rect3 subregion)
        {
            foreach (Rect3 rect in this.Bounds.Without(subregion))
            {
                yield return new VirtualGrid3Region<T>(rect, Value);
            }
        }
    }
}
