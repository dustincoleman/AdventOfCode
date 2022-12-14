namespace AdventOfCode2022
{
    public class Day14
    {
        private static readonly Point2 Origin = new Point2(500, 0);

        private const int CAVE_WIDTH = 1000;
        private const int CAVE_DEPTH = 200;

        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle();
            int count = RunPuzzle(puzzle, sand => (sand.Y > puzzle.LowestPoint));
            Assert.Equal(832, count);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = LoadPuzzle();
            int count = RunPuzzle(puzzle, sand => (sand == Origin)) + 1;
            Assert.Equal(27601, count);
        }

        private int RunPuzzle(Puzzle puzzle, Func<Point2, bool> stop)
        {
            Grid2<bool> cave = puzzle.Cave;
            Point2 sand;
            int count = 0;

            while (!stop(sand = FindNextRestingPlace(cave)))
            {
                cave[sand] = true;
                count++;
            }

            return count;
        }

        private Point2 FindNextRestingPlace(Grid2<bool> cave)
        {
            Point2[] moves = new Point2[]
            {
                Point2.UnitY,
                Point2.UnitY - Point2.UnitX,
                Point2.UnitY + Point2.UnitX
            };

            Point2 move;
            Point2 sand = Origin;

            while ((move = moves.FirstOrDefault(m => !cave[sand + m])) != Point2.Zero)
            {
                sand += move;
            }

            return sand;
        }

        private Puzzle LoadPuzzle()
        {
            Grid2<bool> cave = new Grid2<bool>(CAVE_WIDTH, CAVE_DEPTH);
            int lowest = 0;

            foreach (string line in File.ReadAllLines("Day14.txt"))
            {
                Point2[] points = line.Split(" -> ").Select(Point2.Parse).ToArray();

                for (int i = 0; i < points.Length - 1; i++)
                {
                    foreach (Point2 p in Point2.Line(points[i], points[i + 1]))
                    {
                        cave[p] = true;
                        lowest = Math.Max(p.Y, lowest);
                    }
                }
            }

            for (int x = 0; x < cave.Bounds.X; x++)
            {
                cave[x, lowest + 2] = true;
            }

            return new Puzzle()
            {
                Cave = cave,
                LowestPoint = lowest
            };
        }

        private class Puzzle
        {
            internal Grid2<bool> Cave;
            internal int LowestPoint;
        }
    }
}