using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace AdventOfCode.Common
{
    public struct Line2<T> : IEquatable<Line2<T>> where T : INumber<T>
    {
        public readonly Point2<T> First;
        public readonly Point2<T> Second;

        public Line2(Point2<T> first, Point2<T> second)
        {
            First = first;
            Second = second;
        }

        public bool IsPoint => (First == Second);

        public bool IsVertical => (!IsPoint && First.X == Second.X);

        public bool IsHorizontal => (!IsPoint && First.Y == Second.Y);

        public T X => (IsPoint || IsVertical) ? First.X : throw new InvalidOperationException("Line is not vertical or a point");

        public T Y => (IsPoint || IsHorizontal) ? First.Y : throw new InvalidOperationException("Line is not horizontal or a point");

        public Direction Direction =>
            (IsVertical) ? ((First.Y < Second.Y) ? Direction.South : Direction.North) : // Graphics orientation
            (IsHorizontal) ? ((First.X < Second.X) ? Direction.East : Direction.West) :
            throw new InvalidOperationException("Only supported for vertical and horizontal lines");

        public T Length =>
            (IsVertical) ? T.Abs(First.Y - Second.Y) : 
            (IsHorizontal) ? T.Abs(First.X - Second.X) :
            throw new InvalidOperationException("Only supported for vertical and horizontal lines");

        public IEnumerable<Point2<T>> AllPoints => First.LineTo(Second);

        public bool Contains(Point2<T> point)
        {
            return
                IsPoint ? (point == First) :
                IsVertical ? (point.X == First.X && ((point.Y >= First.Y && point.Y <= Second.Y) || (point.Y <= First.Y && point.Y >= Second.Y))) :
                IsHorizontal ? (point.Y == First.Y && ((point.X >= First.X && point.X <= Second.X) || (point.X <= First.X && point.X >= Second.X))) :
                throw new InvalidOperationException("Only supported for vertical and horizontal lines");
        }

        public bool Equals(Line2<T> other) => (this == other);

        public override bool Equals([NotNullWhen(true)] object obj) => (obj is Line2<T> other && this.Equals(other));

        public override int GetHashCode() => HashCode.Combine(First, Second);

        public Line2<T> Reverse() => new Line2<T>(Second, First);

        public static bool operator ==(Line2<T> left, Line2<T> right) => (left.First == right.First && left.Second == right.Second);
        public static bool operator !=(Line2<T> left, Line2<T> right) => !(left == right);
    }
}
