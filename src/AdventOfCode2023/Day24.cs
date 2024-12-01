using System.Numerics;

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
        List<Stone3> puzzle = File.ReadAllLines("Day24.txt").Select(Stone3.Parse).ToList();
        List<Equation7> equations = new List<Equation7>();

        BuildEquations(equations, puzzle[0], puzzle[1]);
        BuildEquations(equations, puzzle[0], puzzle[2]);
        SolveSystem(equations);

        long answer = equations.Take(3).Sum(eq => Convert.ToInt64(eq.Answer));
        Assert.Equal(0, answer);
    }

    private double? FindIntersection(Stone2 stone1, Stone2 stone2)
    {
        Equation2 x = new Equation2() { LeftLinear = stone1.Velocity.X, LeftConstant = stone1.Position.X, RightLinear = stone2.Velocity.X, RightConstant = stone2.Position.X };
        Equation2 y = new Equation2() { LeftLinear = stone1.Velocity.Y, LeftConstant = stone1.Position.Y, RightLinear = stone2.Velocity.Y, RightConstant = stone2.Position.Y };

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
        Equation2 diff = x - y;
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

    private void BuildEquations(List<Equation7> equations, Stone3 stone1, Stone3 stone2)
    {
        // I can't take credit for this one. Chuck shared the following learning:
        //
        // Intersection points: p + vt = p' + v't  =>  (p - p') = (v' - v)t
        // T is just a scalar value, so they are colinear: (p - p') X (v' - v) = 0  =>  (p X v) = (p' X v) + (p X v') - (p' X v')
        //
        // Since (p X v) is the same, regardless of which intersection point, the right hand is equatable between all intersections:
        // (p1 X v) + (p X v1) - (p1 X v1) = (p2 X v) + (p X v2) - (p2 X v2)
        //
        // Simplifying:
        // (p1 X v) - (p2 X v) + (p X v1) - (p X v2) = (p1 X v1) - (p2 X v2)  [Solvable to Right]
        // ((p1 - p2) X v) + (p X (v1 - v2))         = (p1 X v1) - (p2 X v2)  [X Distributive]
        // (v X (p2 - p1)) + (p X (v1 - v2))         = (p1 X v1) - (p2 X v2)  [X Anticommutative]
        // (p X (v1 - v2)) + (v X (p2 - p1))         = (p1 X v1) - (p2 X v2)  [p before v]
        //
        // By component: let pDiff = p2 - p1, vDiff = v1 - v2, and cDiff = (p1 X v1) - (p2 X v2).
        // We have: (p X vDiff) + (v X pDiff) = cDiff
        //
        // x:  (p.x * 0)       + (p.y * vDiff.z) - (p.z * vDiff.y) + (v.x * 0) +       (v.y * pDiff.z) - (v.z * pDiff.y) = cDiff.x
        // y: -(p.x * vDiff.z) + (p.y * 0)       + (p.z * vDiff.x) - (v.x * pDiff.z) + (v.y * 0)       + (v.z * pDiff.x) = cDiff.y
        // z:  (p.x * vDiff.y) - (p.y * vDiff.x) + (p.z * 0)       + (v.x * pDiff.y) - (v.y * pDiff.x) + (v.z * 0)       = cDiff.z

        Point3<long> pDiff = stone2.Position - stone1.Position;
        Point3<long> vDiff = stone1.Velocity - stone2.Velocity;
        Point3<long> cDiff = stone1.Position.Cross(stone1.Velocity) - stone2.Position.Cross(stone2.Velocity);

        equations.Add(
            new Equation7()
            {
                Y = vDiff.Z,
                Z = -vDiff.Y,
                Vy = pDiff.Z,
                Vz = -pDiff.Y,
                Answer = cDiff.X
            });

        equations.Add(
            new Equation7()
            {
                X = -vDiff.Z,
                Z = vDiff.X,
                Vx = -pDiff.Z,
                Vz = pDiff.X,
                Answer = cDiff.Y
            });

        equations.Add(
            new Equation7()
            {
                X = vDiff.Y,
                Y = -vDiff.X,
                Vx = pDiff.Y,
                Vz = -pDiff.X,
                Answer = cDiff.Z
            });
    }

    private void SolveSystem(List<Equation7> equations)
    {
        // Reduce to row echelon form
        int pivotRow = 0;
        int pivotColumn = 0;

        while (pivotRow < 6 && pivotColumn < 7)
        {
            int rowMax = RowMax(pivotRow, pivotColumn, equations);

            if (equations[rowMax][pivotColumn] == 0)
            {
                pivotColumn++;
            }
            else
            {
                (equations[pivotRow], equations[rowMax]) = (equations[rowMax], equations[pivotRow]);

                for (int row = pivotRow + 1; row < 6; row++)
                {
                    double div = equations[row][pivotColumn] / equations[pivotRow][pivotColumn];

                    equations[row][pivotColumn] = 0;

                    for (int col = pivotColumn + 1; col < 7; col++)
                    {
                        equations[row][col] = equations[row][col] - equations[pivotRow][col] * div;
                    }
                }

                pivotRow++;
                pivotColumn++;
            }
        }

        // Solve
        for (int pivot = 5; pivot >= 0; pivot--)
        {
            Equation7 eq = equations[pivot];

            for (int col = 5; col > pivot; col--)
            {
                double subst = eq[col] * equations[col].Answer;
                eq.Answer -= subst;
                eq[col] = 0;
            }

            long answer = Convert.ToInt64(eq.Answer / eq[pivot]);
            eq[pivot] = 1;
            eq.Answer = Convert.ToDouble(answer);
        }
    }

    private int RowMax(int pivotRow, int pivotColumn, List<Equation7> equations)
    {
        int rowMax = pivotRow;
        double max = 0;

        for (int row = pivotRow; row < equations.Count; row++)
        {
            double value = Math.Abs(equations[row][pivotColumn]);
            if (value > max)
            {
                rowMax = row;
                max = value;
            }
        }

        return rowMax;
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

    internal class Stone3
    {
        internal Point3<long> Position;
        internal Point3<long> Velocity;

        internal static Stone3 Parse(string line)
        {
            string[] parts = line.Split('@');
            return new Stone3()
            {
                Position = Point3<long>.Parse(parts[0]),
                Velocity = Point3<long>.Parse(parts[1])
            };
        }

        internal Point3<double> At(double time)
        {
            return (Velocity.As<double>() * time) + Position.As<double>();
        }
    }

    internal class Equation2
    {
        private static StringBuilder sb = new StringBuilder();

        internal long LeftLinear { get; init; }
        internal long LeftConstant { get; init; }
        internal long RightLinear { get; init; }
        internal long RightConstant { get; init; }

        public static Equation2 operator -(Equation2 left, Equation2 right) =>
            new Equation2()
            {
                LeftLinear = left.LeftLinear - right.LeftLinear,
                LeftConstant = left.LeftConstant - right.LeftConstant,
                RightLinear = left.RightLinear - right.RightLinear,
                RightConstant = left.RightConstant - right.RightConstant
            };

        public static Equation2 operator -(Equation2 e, long i) =>
            new Equation2()
            {
                LeftLinear = e.LeftLinear,
                LeftConstant = e.LeftConstant - i,
                RightLinear = e.RightLinear,
                RightConstant = e.RightConstant - i
            };

        public static Equation2 operator *(Equation2 e, long i) =>
            new Equation2()
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

    internal class Equation7
    {
        internal double X;
        internal double Y;
        internal double Z;
        internal double Vx;
        internal double Vy;
        internal double Vz;
        internal double Answer;

        internal double this[int pos]
        {
            get
            {
                switch (pos)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return Vx;
                    case 4: return Vy;
                    case 5: return Vz;
                    case 6: return Answer;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (pos)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: Vx = value; break;
                    case 4: Vy = value; break;
                    case 5: Vz = value; break;
                    case 6: Answer = value; break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string ToString() => $"X:{X}, Y:{Y}, Z:{Z}, Vx:{Vx}, Vy:{Vy}, Vz:{Vz} = {Answer}";
    }
}
