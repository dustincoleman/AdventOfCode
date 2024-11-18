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

        public readonly Point2 Unit;
        public readonly string Name;

        private Direction(Point2 unit, string name)
        {
            Unit = unit;
            Name = name;
        }

        public static IEnumerable<Direction> All()
        {
            yield return North;
            yield return East;
            yield return South;
            yield return West;
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

        public Direction Reverse()
        {
            if (this == North)
            {
                return South;
            }
            if (this == South)
            {
                return North;
            }
            if (this == West)
            {
                return East;
            }
            if (this == East)
            {
                return West;
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

        public static Direction Parse(string str) => str.ToLower() switch
        {
            "n" or "north" or "u" or "up" => North,
            "s" or "south" or "d" or "down" => South,
            "w" or "west" or "l" or "left" => West,
            "e" or "east" or "r" or "right" => East,
            _ => throw new ArgumentException("Unknown Direction")
        };
    }
}
