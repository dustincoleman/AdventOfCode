namespace AdventOfCode2024
{
    public class Day10
    {
        [Fact]
        public void Part1()
        {
            Grid2<int> puzzle = PuzzleFile.ReadAsGrid("Day10.txt", ch => int.Parse(ch.ToString()));
            int answer = 0;

            foreach (Point2 pt in puzzle.AllPoints.Where(p => puzzle[p] == 0))
            {
                answer += CountTrailheads(puzzle, pt, new Grid2<bool>(puzzle.Bounds));
            }

            Assert.Equal(593, answer);
        }

        [Fact]
        public void Part2()
        {
            Grid2<int> puzzle = PuzzleFile.ReadAsGrid("Day10.txt", ch => int.Parse(ch.ToString()));
            int answer = 0;

            foreach (Point2 pt in puzzle.AllPoints.Where(p => puzzle[p] == 0))
            {
                answer += CountTrailheads(puzzle, pt, null);
            }

            Assert.Equal(1192, answer);
        }

        private int CountTrailheads(Grid2<int> puzzle, Point2 pos, Grid2<bool> visited)
        {
            if (visited != null)
            {
                if (visited[pos])
                    return 0;
                visited[pos] = true;
            }
            if (puzzle[pos] == 9)
            {
                return 1;
            }

            int cnt = 0;

            foreach (Point2 next in puzzle.AdjacentPoints(pos))
            {
                if (puzzle[next] == puzzle[pos] + 1)
                {
                    cnt += CountTrailheads(puzzle, next, visited);
                }
            }

            return cnt;
        }
    }
}
