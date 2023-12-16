using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public class Direction
    {
        public static readonly Direction North = new Direction(-Point2.UnitY, "North/Up");
        public static readonly Direction South = new Direction(Point2.UnitY, "South/Down");
        public static readonly Direction West = new Direction(-Point2.UnitX, "West/Left");
        public static readonly Direction East = new Direction(Point2.UnitX, "East/Right");

        public static readonly Direction Up = North;
        public static readonly Direction Down = South;
        public static readonly Direction Left = West;
        public static readonly Direction Right = East;

        internal readonly Point2 Unit;
        internal readonly string Name;

        private Direction(Point2 unit, string name)
        {
            Unit = unit;
            Name = name;
        }

        public Direction TurnLeft()
        {
            if (this == North)
            {
                return West;
            }
            if (this == South)
            {
                return East;
            }
            if (this == West)
            {
                return South;
            }
            if (this == East)
            {
                return North;
            }

            throw new Exception("Unknown Direction");
        }

        public Direction TurnRight()
        {
            if (this == North)
            {
                return East;
            }
            if (this == South)
            {
                return West;
            }
            if (this == West)
            {
                return North;
            }
            if (this == East)
            {
                return South;
            }

            throw new Exception("Unknown Direction");
        }

        public bool IsNorthOrSouth() => (this == North || this == South);
        public bool IsWestOrEast() => (this == West || this == East);
        public bool IsUpOrDown() => IsNorthOrSouth();
        public bool IsLeftOrRight() => IsWestOrEast();

        public override string ToString()
        {
            return Name;
        }
    }
}
