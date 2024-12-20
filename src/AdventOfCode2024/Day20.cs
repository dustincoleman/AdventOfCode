namespace AdventOfCode2024
{
    public class Day20
    {
        [Fact]
        public void Part1()
        {
            Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day20.txt", ch => new Cell(ch));
            long result = Solve(puzzle, 2);
            Assert.Equal(1448, result);
        }

        [Fact]
        public void Part2()
        {
            Grid2<Cell> puzzle = PuzzleFile.ReadAsGrid("Day20.txt", ch => new Cell(ch));
            long result = Solve(puzzle, 20);
            Assert.Equal(1017615, result);
        }

        private long Solve(Grid2<Cell> puzzle, int cheatDistance)
        {
            Point2 pos = puzzle.AllPoints.First(pt => puzzle[pt].IsStart);
            int distance = 0;

            // Compute all the distances from the start
            while (!puzzle[pos].IsEnd)
            {
                puzzle[pos].Distance = distance++;
                pos = puzzle.AdjacentPoints(pos).First(pt => !puzzle[pt].IsWall && puzzle[pt].Distance == -1);
            }

            puzzle[pos].Distance = distance;

            // Reset pos to the start
            pos = puzzle.AllPoints.First(pt => puzzle[pt].IsStart);

            // Count the cheats that save at least 100 picoseconds
            long result = 0;

            while (!puzzle[pos].IsEnd)
            {
                distance = puzzle[pos].Distance;

                // Count the cheats from this position
                foreach (Point2 cheat in puzzle.ReachablePoints(pos, cheatDistance).Where(pt => !puzzle[pos].IsWall))
                {
                    if (puzzle[cheat].Distance - distance - (pos - cheat).Manhattan() >= 100)
                    {
                        result++;
                    }
                }

                // Move to the next
                pos = puzzle.AdjacentPoints(pos).First(pt => !puzzle[pt].IsWall && puzzle[pt].Distance > distance);
            }

            return result;
        }

        private class Cell(char ch)
        {
            internal int Distance = -1;

            internal bool IsWall => (ch == '#');
            internal bool IsStart => (ch == 'S');
            internal bool IsEnd => (ch == 'E');
        }
    }
}
