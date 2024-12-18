namespace AdventOfCode2024
{
    public class Day18
    {
        [Fact]
        public void Part1()
        {
            List<Point2> puzzle = File.ReadAllLines("Day18.txt").Select(Point2.Parse).ToList();
            Grid2<Cell> grid = new Grid2<Cell>(71, 71);
            foreach (Point2 pt in grid.AllPoints)
            {
                grid[pt] = new Cell();
            }

            foreach (Point2 address in puzzle.Take(1024))
            {
                grid[address].IsCorrupt = true;
            }

            MinDistance(grid, Point2.Zero, 0);

            long result = grid[grid.Bounds - 1].MinDistance;
            Assert.Equal(302, result);
        }

        [Fact]
        public void Part2()
        {
            List<Point2> puzzle = File.ReadAllLines("Day18.txt").Select(Point2.Parse).ToList();
            Grid2<Cell> grid = new Grid2<Cell>(71, 71);
            foreach (Point2 pt in grid.AllPoints)
            {
                grid[pt] = new Cell();
            }

            foreach (Point2 address in puzzle.Take(2850))
            {
                grid[address].IsCorrupt = true;
            }

            Point2 answer = -Point2.One;

            foreach (Point2 address in puzzle.Skip(2850))
            {
                grid[address].IsCorrupt = true;

                foreach (Point2 pt in grid.AllPoints)
                {
                    grid[pt].MinDistance = int.MaxValue;
                }

                MinDistance(grid, Point2.Zero, 0);

                if (grid[grid.Bounds - 1].MinDistance == int.MaxValue)
                {
                    answer = address;
                    break;
                }
            }

            string result = $"{answer.X},{answer.Y}";
            Assert.Equal("24,32", result);
        }

        private void MinDistance(Grid2<Cell> grid, Point2 pos, int distance = 0)
        {
            if (grid[pos].IsCorrupt || grid[pos].MinDistance <= distance)
            {
                return;
            }

            grid[pos].MinDistance = distance;

            if (pos == grid.Bounds - 1)
            {
                return;
            }

            foreach (Point2 adj in grid.AdjacentPoints(pos))
            {
                MinDistance(grid, adj, distance + 1);
            }
        }

        private class Cell
        {
            internal bool IsCorrupt = false;
            internal int MinDistance = int.MaxValue;
        }
    }
}
