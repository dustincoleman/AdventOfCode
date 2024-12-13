using static AdventOfCode2024.Day13;

namespace AdventOfCode2024
{
    public class Day13
    {
        [Fact]
        public void Part1()
        {
            List<Machine> puzzle = LoadPuzzle();
            long result = Solve(puzzle);
            Assert.Equal(29711, result);
        }

        [Fact]
        public void Part2()
        {
            List<Machine> puzzle = LoadPuzzle(10000000000000);
            long result = Solve(puzzle);
            Assert.Equal(94955433618919, result);
        }

        private List<Machine> LoadPuzzle(long toAdd = 0)
        {
            string[][] groups = PuzzleFile.ReadAllLineGroups("Day13.txt");
            List<Machine> machines = new List<Machine>();

            foreach (string[] line in groups)
            {
                string x = line[0].Replace("Button A: X+", "").Replace("Y+", "");
                string y = line[1].Replace("Button B: X+", "").Replace("Y+", "");
                string prize = line[2].Replace("Prize: X=", "").Replace("Y=", "");
                machines.Add(new Machine(Point2<long>.Parse(x), Point2<long>.Parse(y), Point2<long>.Parse(prize) + toAdd));
            }

            return machines;
        }

        private long Solve(List<Machine> puzzle)
        {
            long result = 0;

            foreach (Machine machine in puzzle)
            {
                List<Equation3> system = new List<Equation3>()
                {
                    new Equation3() { A = machine.A.X, B = machine.B.X, Answer = machine.Prize.X },
                    new Equation3() { A = machine.A.Y, B = machine.B.Y, Answer = machine.Prize.Y },
                };

                SolveSystem(system);

                if (system[0].Answer > 0 && system[1].Answer > 0)
                {
                    long a = Convert.ToInt64(system[0].Answer);
                    long b = Convert.ToInt64(system[1].Answer);

                    Point2<long> destination = machine.A * a + machine.B * b;
                    if (destination == machine.Prize)
                    {
                        result += a * 3 + b;
                    }
                }
            }

            return result;
        }

        private void SolveSystem(List<Equation3> equations)
        {
            // Reduce to row echelon form
            int pivotRow = 0;
            int pivotColumn = 0;

            while (pivotRow < 2 && pivotColumn < 3)
            {
                int rowMax = RowMax(pivotRow, pivotColumn, equations);

                if (equations[rowMax][pivotColumn] == 0)
                {
                    pivotColumn++;
                }
                else
                {
                    (equations[pivotRow], equations[rowMax]) = (equations[rowMax], equations[pivotRow]);

                    for (int row = pivotRow + 1; row < 2; row++)
                    {
                        double div = equations[row][pivotColumn] / equations[pivotRow][pivotColumn];

                        equations[row][pivotColumn] = 0;

                        for (int col = pivotColumn + 1; col < 3; col++)
                        {
                            equations[row][col] = equations[row][col] - equations[pivotRow][col] * div;
                        }
                    }

                    pivotRow++;
                    pivotColumn++;
                }
            }

            // Solve
            for (int pivot = 1; pivot >= 0; pivot--)
            {
                Equation3 eq = equations[pivot];

                for (int col = 1; col > pivot; col--)
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

        private int RowMax(int pivotRow, int pivotColumn, List<Equation3> equations)
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

        private record Machine(Point2<long> A, Point2<long> B, Point2<long> Prize);

        private class Equation3
        {
            internal double A;
            internal double B;
            internal double Answer;

            internal double this[int pos]
            {
                get
                {
                    switch (pos)
                    {
                        case 0: return A;
                        case 1: return B;
                        case 2: return Answer;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
                set
                {
                    switch (pos)
                    {
                        case 0: A = value; break;
                        case 1: B = value; break;
                        case 2: Answer = value; break;
                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            public override string ToString() => $"A:{A:0.0000}, B:{B:0.0000} = {Answer:0.0000}";
        }
    }
}
