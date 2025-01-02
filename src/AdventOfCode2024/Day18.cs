namespace AdventOfCode2024
{
    public class Day18
    {
        [Fact]
        public void Part1()
        {
            List<Point2> puzzle = File.ReadAllLines("Day18.txt").Select(Point2.Parse).ToList();
            Grid2<bool> grid = new Grid2<bool>(71, 71);

            foreach (Point2 address in puzzle.Take(1024))
            {
                grid[address] = true;
            }

            long result = ShortestPath(grid).Count - 1;
            Assert.Equal(302, result);
        }

        [Fact]
        public void Part2()
        {
            List<Point2> puzzle = File.ReadAllLines("Day18.txt").Select(Point2.Parse).ToList();
            Grid2<bool> grid = new Grid2<bool>(71, 71);

            HashSet<Point2> path = null;
            Point2 answer = -Point2.One;

            foreach (Point2 address in puzzle)
            {
                grid[address] = true;

                if (path == null || path.Contains(address))
                {
                    path = ShortestPath(grid);

                    if (path == null)
                    {
                        answer = address;
                        break;
                    }
                }
            }

            string result = $"{answer.X},{answer.Y}";
            Assert.Equal("24,32", result);
        }

        private HashSet<Point2> ShortestPath(Grid2<bool> corruptedMemory)
        {
            Grid2<bool> visited = new Grid2<bool>(corruptedMemory.Bounds);
            Grid2<Point2?> previous = new Grid2<Point2?>(corruptedMemory.Bounds);

            Queue<Point2> queue = new Queue<Point2>();
            queue.Enqueue(Point2.Zero);

            while (queue.TryDequeue(out Point2 pt))
            {
                if (pt == corruptedMemory.Bounds - 1)
                {
                    HashSet<Point2> path = new HashSet<Point2<int>>() { pt };

                    while (previous[pt].HasValue)
                    {
                        pt = previous[pt].Value;
                        path.Add(pt);
                    }

                    return path;
                }

                if (!corruptedMemory[pt] && !visited[pt])
                {
                    visited[pt] = true;

                    foreach (Point2 next in corruptedMemory.AdjacentPoints(pt))
                    {
                        if (!visited[next])
                        {
                            previous[next] = pt;
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            return null;
        }
    }
}
