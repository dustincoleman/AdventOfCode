using System.Numerics;

namespace AdventOfCode.Common
{
    public static class Points
    {
        public static IEnumerable<Point2<T>> All<T>(Point2<T> bounds) where T : INumber<T>
        {
            T xSign = (bounds.X >= T.Zero) ? T.One : -T.One;
            T ySign = (bounds.Y >= T.Zero) ? T.One : -T.One;

            for (T y = T.Zero; y < bounds.Y; y++)
            {
                for (T x = T.Zero; x < bounds.X; x++)
                {
                    yield return new Point2<T>(x * xSign, y * ySign);
                }
            }
        }

        public static IEnumerable<Point2<T>> Line<T>(Point2<T> left, Point2<T> right) where T : INumber<T>
        {
            Point2<T> step;

            if (left.X == right.X)
            {
                if (left.Y <= right.Y)
                {
                    step = Point2<T>.UnitY;
                }
                else
                {
                    step = -Point2<T>.UnitY;
                }
            }
            else if (left.Y == right.Y)
            {
                if (left.X <= right.X)
                {
                    step = Point2<T>.UnitX;
                }
                else
                {
                    step = -Point2<T>.UnitX;
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            yield return left;

            while (left != right)
            {
                left += step;
                yield return left;
            }
        }
    }
}
