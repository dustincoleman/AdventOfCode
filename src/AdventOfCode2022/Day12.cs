namespace AdventOfCode2022
{
    public class Day12
    {
        [Fact]
        public void Part1()
        {
            Puzzle puzzle = LoadPuzzle();
            Grid2<Node> map = puzzle.Map;

            FindShortestPaths(puzzle.Start, map);

            int shortestPath = map[puzzle.End].ShortestDistance;
            Assert.Equal(504, shortestPath);
        }

        [Fact]
        public void Part2()
        {
            Puzzle puzzle = LoadPuzzle();
            Grid2<Node> map = puzzle.Map;

            foreach (Point2 p in puzzle.Map.AllPoints)
            {
                if (map[p].Height == 0)
                {
                    FindShortestPaths(p, map);
                }
            }

            int shortestPath = map[puzzle.End].ShortestDistance;
            Assert.Equal(500, shortestPath);
        }

        private void FindShortestPaths(Point2 p, Grid2<Node> map, int distance = 0)
        {
            if (distance < map[p].ShortestDistance)
            {
                map[p].ShortestDistance = distance;

                foreach (Point2 adjacent in p.Adjacent(map.Bounds))
                {
                    if (map[adjacent].Height <= map[p].Height + 1)
                    {
                        FindShortestPaths(adjacent, map, distance + 1);
                    }
                }
            }
        }

        private Puzzle LoadPuzzle()
        {
            string[] lines = File.ReadAllLines("Day12.txt");
            Grid2<Node> map = new Grid2<Node>(lines[0].Length, lines.Length);
            Puzzle puzzle = new Puzzle() { Map = map };

            foreach (Point2 p in map.AllPoints)
            {
                char ch = lines[p.Y][p.X];

                if (ch == 'S')
                {
                    puzzle.Start = p;
                    ch = 'a';
                }
                else if (ch == 'E')
                {
                    puzzle.End = p;
                    ch = 'z';
                }

                map[p] = new Node() { Height = ch - 'a', ShortestDistance = int.MaxValue };
            }

            return puzzle;
        }

        private class Puzzle
        {
            internal Grid2<Node> Map;
            internal Point2 Start;
            internal Point2 End;
        }

        private class Node
        {
            internal int Height;
            internal int ShortestDistance;
        }
    }
}