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
                LinearEquationSystem system = new LinearEquationSystem()
                {
                    new LinearEquation(machine.A.X, machine.B.X, machine.Prize.X),
                    new LinearEquation(machine.A.Y, machine.B.Y, machine.Prize.Y ),
                };

                if (system.SolveAsInteger() && system[0].Answer >= 0 && system[1].Answer >= 0)
                {
                    result += (long)system[0].Answer * 3 + (long)system[1].Answer;
                }
            }

            return result;
        }

        private record Machine(Point2<long> A, Point2<long> B, Point2<long> Prize);
    }
}
