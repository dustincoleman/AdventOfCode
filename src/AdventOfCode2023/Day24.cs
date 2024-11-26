namespace AdventOfCode2023;

public class Day24
{
    [Fact]
    public void Part1()
    {
        List<Stone2> puzzle = File.ReadAllLines("Day24.txt").Select(Stone2.Parse).ToList();
        Point2<double> minPos = new Point2<double>(200000000000000, 200000000000000);
        Point2<double> maxPos = new Point2<double>(400000000000000, 400000000000000);
        int answer = 0;

        for (int i = 0; i < puzzle.Count; i++)
        {
            for (int j = i + 1; j < puzzle.Count; j++)
            {
                Stone2 stone1 = puzzle[i];
                Stone2 stone2 = puzzle[j];

                double? t1 = FindIntersection(stone1, stone2);
                double? t2 = FindIntersection(stone2, stone1);

                if (t1.HasValue != t2.HasValue)
                {
                    throw new Exception("Unexpected");
                }

                if (t1.HasValue && t1.Value > 0 && t2.Value > 0)
                {
                    Point2<double> pt = stone1.At(t1.Value);
                    
                    if (pt >= minPos && pt <= maxPos)
                    {
                        answer++;
                    }
                }
            }
        }

        Assert.Equal(16779, answer);
    }

    [Fact]
    public void Part2()
    {
        int answer = 0;
        Assert.Equal(0, answer);
    }

    private double? FindIntersection(Stone2 stone1, Stone2 stone2)
    {
        Equation x = new Equation() { LeftLinear = stone1.Velocity.X, LeftConstant = stone1.Position.X, RightLinear = stone2.Velocity.X, RightConstant = stone2.Position.X };
        Equation y = new Equation() { LeftLinear = stone1.Velocity.Y, LeftConstant = stone1.Position.Y, RightLinear = stone2.Velocity.Y, RightConstant = stone2.Position.Y };

        // Ensure the linear nomials on the right side are signed the same
        if (x.RightLinear < 0)
        {
            x = x * -1;
        }
        if (y.RightLinear < 0)
        {
            y = y * -1;
        }

        // Make the linear nomials on the right side the same
        long lcm = MathHelpers.LeastCommonMultiple(x.RightLinear, y.RightLinear);
        x = x * (lcm / x.RightLinear);
        y = y * (lcm / y.RightLinear);

        // Subtract the equations
        Equation diff = x - y;
        if (diff.RightLinear != 0)
        {
            throw new Exception("Unexpected");
        }

        // Move the constant nomial from the left side to the right side
        diff = diff - diff.LeftConstant;
        if (diff.LeftConstant != 0 || diff.RightLinear != 0)
        {
            throw new Exception("Unexpected");
        }

        // Check if an intersection exists
        if (diff.LeftLinear == 0)
        {
            return null;
        }

        // Move the linear nomial from the left side to the right side to solve
        double t = double.Round((double)diff.RightConstant / diff.LeftLinear, 3);

        return t;
    }


    internal class Stone2
    {
        internal Point2<long> Position;
        internal Point2<long> Velocity;

        internal static Stone2 Parse(string line)
        {
            string[] parts = line.Split('@');
            return new Stone2()
            {
                Position = Point3<long>.Parse(parts[0]).XY,
                Velocity = Point3<long>.Parse(parts[1]).XY
            };
        }

        internal Point2<double> At(double time)
        {
            return (Velocity.As<double>() * time) + Position.As<double>();
        }
    }

    internal class Equation
    {
        private static StringBuilder sb = new StringBuilder();

        internal long LeftLinear { get; init; }
        internal long LeftConstant { get; init; }
        internal long RightLinear { get; init; }
        internal long RightConstant { get; init; }

        public static Equation operator -(Equation left, Equation right) =>
            new Equation()
            {
                LeftLinear = left.LeftLinear - right.LeftLinear,
                LeftConstant = left.LeftConstant - right.LeftConstant,
                RightLinear = left.RightLinear - right.RightLinear,
                RightConstant = left.RightConstant - right.RightConstant
            };

        public static Equation operator -(Equation e, long i) =>
            new Equation()
            {
                LeftLinear = e.LeftLinear,
                LeftConstant = e.LeftConstant - i,
                RightLinear = e.RightLinear,
                RightConstant = e.RightConstant - i
            };

        public static Equation operator *(Equation e, long i) =>
            new Equation()
            {
                LeftLinear = e.LeftLinear * i,
                LeftConstant = e.LeftConstant * i,
                RightLinear = e.RightLinear * i,
                RightConstant = e.RightConstant * i
            };

        public override string ToString()
        {
            if (sb.Length != 0)
            {
                throw new InvalidOperationException();
            }

            if (LeftLinear != 0)
            {
                if (LeftLinear < 0)
                {
                    sb.Append("-");
                }

                if (LeftLinear != 1)
                {
                    sb.Append(Math.Abs(LeftLinear));
                }

                sb.Append("λ");

                if (LeftConstant != 0)
                {
                    if (LeftConstant < 0)
                    {
                        sb.Append(" - ");
                    }
                    else
                    {
                        sb.Append(" + ");
                    }

                    sb.Append(Math.Abs(LeftConstant));
                }
            }
            else
            {
                sb.Append(LeftConstant);
            }

            sb.Append(" = ");

            if (RightLinear != 0)
            {
                if (RightLinear < 0)
                {
                    sb.Append("-");
                }

                if (RightLinear != 1)
                {
                    sb.Append(Math.Abs(RightLinear));
                }

                sb.Append("μ");

                if (RightConstant != 0)
                {
                    if (RightConstant < 0)
                    {
                        sb.Append(" - ");
                    }
                    else
                    {
                        sb.Append(" + ");
                    }

                    sb.Append(Math.Abs(RightConstant));
                }
            }
            else
            {
                sb.Append(RightConstant);
            }

            string str = sb.ToString();
            sb.Clear();

            return str;
        }
    }
}
