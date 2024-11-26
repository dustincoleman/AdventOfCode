using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public static class Point3Extensions
    {
        public static IEnumerable<Point3<T>> Adjacent<T>(this Point3<T> pt) where T : INumber<T>
        {
            yield return pt - Point3<T>.UnitX;
            yield return pt - Point3<T>.UnitY;
            yield return pt - Point3<T>.UnitZ;
            yield return pt + Point3<T>.UnitX;
            yield return pt + Point3<T>.UnitY;
            yield return pt + Point3<T>.UnitZ;
        }

        public static Point3<T> Orient<T>(this Point3<T> pt, Orientation3 orientation) where T : INumber<T>
        {
            switch (orientation)
            {
                case Orientation3.PositiveX1:
                    return pt;
                case Orientation3.PositiveX2:
                    return pt.RotateXClockwise();
                case Orientation3.PositiveX3:
                    return pt.RotateX180Degrees();
                case Orientation3.PositiveX4:
                    return pt.RotateXCounterclockwise();

                case Orientation3.PositiveY1:
                    return pt.RotateZCounterclockwise();
                case Orientation3.PositiveY2:
                    return pt.RotateZCounterclockwise().RotateYClockwise();
                case Orientation3.PositiveY3:
                    return pt.RotateZCounterclockwise().RotateY180Degrees();
                case Orientation3.PositiveY4:
                    return pt.RotateZCounterclockwise().RotateYCounterclockwise();

                case Orientation3.PositiveZ1:
                    return pt.RotateYClockwise();
                case Orientation3.PositiveZ2:
                    return pt.RotateYClockwise().RotateZClockwise();
                case Orientation3.PositiveZ3:
                    return pt.RotateYClockwise().RotateZ180Degrees();
                case Orientation3.PositiveZ4:
                    return pt.RotateYClockwise().RotateZCounterclockwise();

                case Orientation3.NegativeX1:
                    return pt.RotateY180Degrees();
                case Orientation3.NegativeX2:
                    return pt.RotateY180Degrees().RotateXClockwise();
                case Orientation3.NegativeX3:
                    return pt.RotateY180Degrees().RotateX180Degrees();
                case Orientation3.NegativeX4:
                    return pt.RotateY180Degrees().RotateXCounterclockwise();

                case Orientation3.NegativeY1:
                    return pt.RotateZClockwise();
                case Orientation3.NegativeY2:
                    return pt.RotateZClockwise().RotateYClockwise();
                case Orientation3.NegativeY3:
                    return pt.RotateZClockwise().RotateY180Degrees();
                case Orientation3.NegativeY4:
                    return pt.RotateZClockwise().RotateYCounterclockwise();

                case Orientation3.NegativeZ1:
                    return pt.RotateYCounterclockwise();
                case Orientation3.NegativeZ2:
                    return pt.RotateYCounterclockwise().RotateZClockwise();
                case Orientation3.NegativeZ3:
                    return pt.RotateYCounterclockwise().RotateZ180Degrees();
                case Orientation3.NegativeZ4:
                    return pt.RotateYCounterclockwise().RotateZCounterclockwise();

                default:
                    throw new ArgumentOutOfRangeException(nameof(orientation));
            }
        }

        public static Point3<T> RotateXClockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(pt.X, -pt.Z, pt.Y);
        public static Point3<T> RotateXCounterclockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(pt.X, pt.Z, -pt.Y);
        public static Point3<T> RotateX180Degrees<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(pt.X, -pt.Y, -pt.Z);
        public static Point3<T> RotateYClockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(-pt.Z, pt.Y, pt.X);
        public static Point3<T> RotateYCounterclockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(pt.Z, pt.Y, -pt.X);
        public static Point3<T> RotateY180Degrees<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(-pt.X, pt.Y, -pt.Z);
        public static Point3<T> RotateZClockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(pt.Y, -pt.X, pt.Z);
        public static Point3<T> RotateZCounterclockwise<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(-pt.Y, pt.X, pt.Z);
        public static Point3<T> RotateZ180Degrees<T>(this Point3<T> pt) where T : INumber<T> => new Point3<T>(-pt.X, -pt.Y, pt.Z);

        public static Point3<T> Rotate<T>(this Point3<T> pt, Point3<T> times) where T : INumber<T>
        {
            for (T x = T.Zero; x < times.X; x++)
            {
                pt = pt.RotateXClockwise();
            }

            for (T y = T.Zero; y < times.Y; y++)
            {
                pt = pt.RotateYClockwise();
            }

            for (T z = T.Zero; z < times.Z; z++)
            {
                pt = pt.RotateZClockwise();
            }

            return pt;
        }
    }
}
