namespace AdventOfCode2024
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day06.txt");
            Point2 start = puzzle.AllPoints.First(pt => Direction.ParseOrDefault(puzzle[pt]) != null);
            Direction direction = Direction.Parse(puzzle[start]);

            int answer = Visited(puzzle, start, direction).Count;
            Assert.Equal(5409, answer);
        }

        [Fact]
        public void Part2()
        {
            Grid2<char> puzzle = PuzzleFile.ReadAsGrid("Day06.txt");
            Point2 start = puzzle.AllPoints.First(pt => Direction.ParseOrDefault(puzzle[pt]) != null);
            Direction direction = Direction.Parse(puzzle[start]);

            int answer = 0;

            foreach (Point2 obstruction in Visited(puzzle, start, direction))
            {
                if (obstruction != start && HasCycle(puzzle, start, direction, obstruction))
                {
                    answer++;
                }
            }

            Assert.Equal(2022, answer);
        }

        private HashSet<Point2> Visited(Grid2<char> puzzle, Point2 pos, Direction direction)
        {
            HashSet<Point2> visited = new HashSet<Point2>();

            while (puzzle.InBounds(pos))
            {
                visited.Add(pos);

                Point2 next = pos + direction;
                if (!puzzle.InBounds(next))
                {
                    return visited;
                }

                while (puzzle[next] == '#')
                {
                    direction = direction.TurnRight();
                    next = pos + direction;

                    if (!puzzle.InBounds(next))
                    {
                        return visited;
                    }
                }

                pos = next;
            }

            return visited;
        }

        private bool HasCycle(Grid2<char> puzzle, Point2 pos, Direction direction, Point2 obstruction)
        {
            Grid2<BitVector> visited = new Grid2<BitVector>(puzzle.Bounds);

            while (puzzle.InBounds(pos))
            {
                if (visited[pos][(int)direction])
                {
                    return true;
                }

                BitVector bitVector = visited[pos];
                bitVector[(int)direction] = true;
                visited[pos] = bitVector;

                Point2 next = pos + direction;
                if (!puzzle.InBounds(next))
                {
                    return false;
                }

                while (puzzle[next] == '#' || next == obstruction)
                {
                    direction = direction.TurnRight();
                    next = pos + direction;

                    if (!puzzle.InBounds(next))
                    {
                        return false;
                    }
                }

                pos = next;
            }

            return false;
        }
    }
}
